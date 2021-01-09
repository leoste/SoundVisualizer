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
            int startIndex = 0; //index of first note we can start on
            int endIndex = sound.FrequencyCount;
            bool startIsntSet = true;
            bool endIsntSet = true;

            float scalex = sound.FrameSize.Width / visibleNoteSpan;
            //float basey = (float)Math.Log(sound.MaximumAmplitude - sound.MinimumAmplitude, log);
            float basey = (sound.MaximumAmplitude - sound.MinimumAmplitude);
            float scaley = sound.FrameSize.Width / 3.5f / basey; //width is 7x height ... but since im doing mirrored, 3.5 is used
            float scalealpha = 255 / basey;

            //preparation for image calculation
            Color[] colors = new Color[sound.FrequencyCount];
            float[] x = new float[sound.FrequencyCount];
            float[] w = new float[sound.FrequencyCount];
            Hsv hsv = new Hsv();
            hsv.S = 1;
            hsv.V = 1;
            for (int i = 0; i < sound.FrequencyCount; i++)
            {
                //color
                //double thingy = sound.NoteSpans[i].start % 12 / 12;
                double thingy = (sound.NoteSpans[i].start + noteMinoffset) % 12;
                if (thingy < 0) thingy = 12 + thingy; // correct negative numbers
                if (thingy > 6) thingy = 12 - thingy; // make it go backwards
                thingy = thingy * scaleThingy; //divide only by 6 cause thats the max number can go to thanks to backwards
                thingy = (thingy + colorStartDegree) % 360;
                //thingy *= lengthColorDegree;

                hsv.H = thingy;
                IRgb rgb = hsv.ToRgb();
                Color c = Color.FromArgb(255, (int)rgb.R, (int)rgb.G, (int)rgb.B);
                colors[i] = c;

                x[i] = (sound.NoteSpans[i].start - sound.MinimumNote - noteMinoffset) * scalex - columnHalfWidth;
                //w[i] = sound.NoteSpans[i].length * scalex;
                w[i] = columnWidth;

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

            // anti-spaz measures
            float[] decays = new float[sound.FrequencyCount];
            float[] decayPeaks = new float[sound.FrequencyCount];
            float[] decayCounts = new float[sound.FrequencyCount];
            float decayCountMargin = decayExponent / decayTime;

            //this is for note-based visualization
            for (int i = 0; i < sound.Frames.Length; i++)
            {
                using (Bitmap bitmap = new Bitmap(sound.FrameSize.Width, sound.FrameSize.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.Black);

                        for (int k = startIndex; k < endIndex; k++)
                        {
                            //float baseh = (float)Math.Max(Math.Log(sound.Frames[i].frequencies[k], log), 0); //delete negative data lol
                            float baseh = sound.Frames[i].frequencies[k];

                            //decays[k] = (float)Math.Log(baseh, decayAmount);
                            if (decayCounts[k] > 0)
                            {
                                decays[k] = (float)Math.Log(decayCounts[k], decayExponent) * decayPeaks[k];
                                decayCounts[k] -= decayCountMargin;
                            }

                            if (baseh >= decays[k]) //if new is bigger overwrite decay
                            {
                                decayPeaks[k] = baseh;
                                decayCounts[k] = decayTime;
                                //decays[k] = baseh; // not necessary cause wont be used in this cycle
                            }
                            else baseh = decays[k]; // if new is smaller show decay

                            float h = baseh * scaley;
                            float y = halfHeight - h * 0.5f;
                            Color c = colors[k];
                            Brush brush = new SolidBrush(c);
                            //Brush brush = new SolidBrush(Color.FromArgb((int)(baseh * scalealpha), c)); // for scaling alpha
                            g.FillRectangle(brush, x[k], y, w[k], h);
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

        public IEnumerable<IVideoFrame> SimulateProcess() // this is to simulate video but not actually render as file
        {
            return CreateFrames();
        }
    }
}
