using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Pipes;
using FFMpegCore.Extend;
using System.Drawing;
using System.IO;
using ColorMine.ColorSpaces;
using System.Drawing.Drawing2D;

namespace GreatVideoMaker
{
    class VideoRenderer : Processer
    {
        private string filepath;
        private SoundAnalyzer sound;

        private float log;
        private int barRelation;
        private float colorStartDegree;
        private float colorLengthDegree;
        private int decayExponent;
        private int decayTime;
        private int maxNoteBorder;
        private int minNoteBorder;

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public VideoRenderer(string filepath, SoundAnalyzer sound,
            int barRelation = 128,
            float startColorDegree = 0, // 216, 160
            float lengthColorDegree = 180, // 60, 60
            int decayExponent = 10,
            int decayTime = 5,
            int minNoteBorder = -36,
            int maxNoteBorder = 48)
        {
            this.filepath = filepath;
            this.sound = sound;
            log = 2; // not really used tho
            this.barRelation = barRelation;
            this.colorStartDegree = startColorDegree;
            this.colorLengthDegree = lengthColorDegree;
            this.decayExponent = decayExponent;
            this.decayTime = decayTime;
            this.minNoteBorder = minNoteBorder;
            this.maxNoteBorder = maxNoteBorder;
        }

        IEnumerable<IVideoFrame> CreateFrames()
        {
            float columnRelation = 1f / barRelation;
            float scaleThingy = colorLengthDegree / 6;

            float halfHeight = sound.FrameSize.Height / 2f;
            float columnWidth = sound.FrameSize.Width * columnRelation;
            float columnHalfWidth = columnWidth / 2;
            float noteMinoffset = -(sound.MinimumNote - minNoteBorder);
            float noteMaxOffset = (sound.MaximumNote - maxNoteBorder);
            float visibleNoteSpan = sound.NotesTotalLength - noteMinoffset - noteMaxOffset;
            bool startIsntSet = true;
            bool endIsntSet = true;

            float scalex = sound.FrameSize.Width / visibleNoteSpan;
            float basey = (sound.MaximumAmplitude - sound.MinimumAmplitude);
            float scaley = sound.FrameSize.Width / 7f * 2 / basey; //width is 7x height ... but multiply with 2 cause its mirrored
            //float scalealpha = 255 / basey;

            //color preparation for image calculation            
            Color[] colors = new Color[sound.FrameSize.Width]; //theres only really point in calculating color for each pixel
            float colorRelation = visibleNoteSpan / colors.Length;
            Hsv hsv = new Hsv();
            hsv.S = 1;
            hsv.V = 1;
            for (int i = 0; i < colors.Length; i++)
            {
                double thingy = (i * colorRelation + noteMinoffset) % 12;
                if (thingy < 0) thingy = 12 + thingy; // correct negative numbers
                if (thingy > 6) thingy = 12 - thingy; // make it go backwards
                thingy = thingy * scaleThingy; //divide only by 6 cause thats the max number can go to thanks to backwards
                thingy = (thingy + colorStartDegree) % 360;
                hsv.H = thingy;
                IRgb rgb = hsv.ToRgb();
                Color c = Color.FromArgb(255, (int)rgb.R, (int)rgb.G, (int)rgb.B);
                colors[i] = c;
            }

            //calculate x, find edges for notes that are actually visible
            float[] x = new float[sound.FrequencyCount];
            int startIndex = 0; //index of first note we can start on
            int endIndex = sound.FrequencyCount; // index of last note used
            for (int i = 0; i < sound.FrequencyCount; i++)
            {
                x[i] = (sound.NoteSpans[i].start - sound.MinimumNote - noteMinoffset) * scalex - columnHalfWidth;
                if (startIsntSet)
                {
                    if (sound.NoteSpans[i].start >= minNoteBorder)
                    {
                        startIndex = i;
                        startIsntSet = false;
                    }
                }
                else if (endIsntSet)
                {
                    if (sound.NoteSpans[i].start >= maxNoteBorder)
                    {
                        endIndex = i;
                        endIsntSet = false;
                    }
                }
            }

            int visibleLength = endIndex - startIndex;

            // anti-spaz measures
            float[] decays = new float[visibleLength];
            float[] decayPeaks = new float[visibleLength];
            float[] decayCounts = new float[visibleLength];
            float decayCountMargin = decayExponent / decayTime;

            //this is for note-based visualization
            for (int i = 0; i < sound.Frames.Length; i++)
            {
                // first get the points that we do know
                PointF[] sourcePoints = new PointF[visibleLength];
                for (int k = 0; k < visibleLength; k++)
                {
                    int correctedIndex = k + startIndex;
                    float baseh = sound.Frames[i].frequencies[correctedIndex];

                    // MAYBE calculating the decays before running points simulation could ruin the outcome points a bit
                    // but its a bit hard to change order so i'll leave it be for now, it probably doesnt do much
                    if (decayCounts[k] > 0)
                    {
                        decays[k] = (float)Math.Log(decayCounts[k], decayExponent) * decayPeaks[k];
                        decayCounts[k] -= decayCountMargin;
                    }
                    if (baseh >= decays[k]) //if new is bigger overwrite decay
                    {
                        decayPeaks[k] = baseh;
                        decayCounts[k] = decayTime;
                    }
                    else baseh = decays[k]; // if new is smaller show decay

                    float h = baseh * scaley;
                    sourcePoints[k] = new PointF(x[correctedIndex], h);
                }

                // generate real points that will be drawn. essentially we have now filled in the missing gaps that there otherwise would be
                // but also minimized amount of points at higher end where a lot of them share the same space (so more efficient rendering)
                PointF[] uniformPoints;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddCurve(sourcePoints);
                    // use a unit matrix to get points per pixel https://stackoverflow.com/questions/52433314/extracting-points-coordinatesx-y-from-a-curve-c-sharp
                    using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                    {
                        path.Flatten(mx, 0.1f);
                        uniformPoints = path.PathPoints;
                    }
                }

                using (Bitmap bitmap = new Bitmap(sound.FrameSize.Width, sound.FrameSize.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.Black);

                        for (int k = 0; k < uniformPoints.Length; k++)
                        {
                            Color c = colors[(int)uniformPoints[k].X];
                            Brush brush = new SolidBrush(c);

                            float h = uniformPoints[k].Y;
                            float y = halfHeight - h * 0.5f;
                            g.FillRectangle(brush, uniformPoints[k].X, y, columnWidth, h);
                        }
                    }
                    BitmapVideoFrameWrapper wrapper = new BitmapVideoFrameWrapper(bitmap);
                    yield return wrapper;                    
                }
            }
        }

        public void StartProcess()
        {
            Task task = new Task(() =>
            {
                var videoFramesSource = new RawVideoPipeSource(CreateFrames()) { FrameRate = sound.FrameRate };
                FFMpegArguments
                    .FromPipeInput(videoFramesSource)
                    .AddFileInput(new FileInfo(sound.SourceFilePath))
                    .OutputToFile(filepath)
                    .NotifyOnProgress(new Action<TimeSpan>((TimeSpan t) => {
                        if (OnProgress != null) OnProgress.Invoke(this,
                            new ProgressEventArgs((int)t.TotalSeconds, (int)sound.TotalLength.TotalSeconds));
                    }))
                    .ProcessSynchronously();

                if (OnComplete != null) OnComplete.Invoke(this, EventArgs.Empty);
            });
            task.Start();
        }
    }
}
