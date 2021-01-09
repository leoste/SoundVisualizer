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
        private static int width = 1280;
        private static int height = 720;

        private string filepath;
        private SoundAnalyzer sound;

        private float log;
        private int barRelation;
        private float startColorDegree;
        private float lengthColorDegree;

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public VideoRenderer(string filepath, SoundAnalyzer sound,
            int barRelation = 64,
            float startColorDegree = 0, // 216, 160
            float lengthColorDegree = 60) // 60, 60
        {
            this.filepath = filepath;
            this.sound = sound;
            log = 2; // not really used tho
            this.barRelation = barRelation;
            this.startColorDegree = startColorDegree;
            this.lengthColorDegree = lengthColorDegree;
        }
        
        IEnumerable<IVideoFrame> CreateFrames()
        {
            float columnRelation = 1f / barRelation;
            float scaleThingy = lengthColorDegree / 6;

            float halfHeight = height / 2f;
            float columnWidth = width * columnRelation;
            float columnHalfWidth = columnWidth / 2;
            int noteMinBorder = -36; // anything lower than that wont be rendered
            int noteMaxBorder = 48; // anything higher than that wont be rendered
            float noteMinoffset = -(sound.MinimumNote - noteMinBorder);
            float noteMaxOffset = (sound.MaximumNote - noteMaxBorder);
            float visibleNoteSpan = sound.NotesTotalLength - noteMinoffset - noteMaxOffset;
            int startIndex = 0; //index of first note we can start on
            int endIndex = sound.FrequencyCount;
            bool startIsntSet = true;
            bool endIsntSet = true;

            float scalex = width / visibleNoteSpan;
            //float basey = (float)Math.Log(sound.MaximumAmplitude - sound.MinimumAmplitude, log);
            float basey = (sound.MaximumAmplitude - sound.MinimumAmplitude);
            float scaley = width / 3.5f / basey; //width is 7x height ... but since im doing mirrored, 3.5 is used
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
                thingy = (thingy + startColorDegree) % 360;
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
                    if (sound.NoteSpans[i].start >= noteMinBorder)
                    {
                        startIndex = i;
                        startIsntSet = false;
                    }
                }
                else if (endIsntSet)
                {
                    if (sound.NoteSpans[i].start >= noteMaxBorder)
                {
                        endIndex = i;
                        endIsntSet = false;
                    }
                }
            }

            //this is for note-based visualization
            for (int i = 0; i < sound.Frames.Length; i++)
            {
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.Black);

                        for (int k = startIndex; k < endIndex; k++)
                        {
                            //float baseh = (float)Math.Max(Math.Log(sound.Frames[i].frequencies[k], log), 0); //delete negative data lol
                            float baseh = sound.Frames[i].frequencies[k];
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
    }
}
