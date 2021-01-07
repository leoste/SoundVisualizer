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

        private int width = 1280;
        private int height = 720;

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public VideoRenderer(string filepath, SoundAnalyzer sound)
        {
            this.filepath = filepath;
            this.sound = sound;
        }
        
        IEnumerable<IVideoFrame> CreateFrames()
        {
            float scalex = width / sound.NotesTotalLength;
            float scaley = height / (sound.MaximumAmplitude - sound.MinimumAmplitude);

            //preparation for image calculation
            SolidBrush[] brushes = new SolidBrush[sound.FrequencyCount];
            float[] x = new float[sound.FrequencyCount];
            float[] w = new float[sound.FrequencyCount];
            Hsv hsv = new Hsv();
            hsv.S = 1;
            hsv.V = 1;
            for (int i = 0; i < sound.FrequencyCount; i++)
            {
                //color
                double thingy = sound.NoteSpans[i].start % 12 / 12;
                if (thingy < 0) thingy = 1 + thingy;
                hsv.H = 360 * thingy;
                IRgb rgb = hsv.ToRgb();
                Color c = Color.FromArgb(255, (int)rgb.R, (int)rgb.G, (int)rgb.B);
                brushes[i] = new SolidBrush(c);

                x[i] = (sound.NoteSpans[i].start - sound.MinimumNote) * scalex;
                w[i] = sound.NoteSpans[i].length * scalex;
            }

            //this is for note-based visualization
            for (int i = 0; i < sound.Frames.Length; i++)
            {
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.White);

                        for (int k = 0; k < sound.Frames[i].frequencies.Length; k++)
                        {
                            float h = sound.Frames[i].frequencies[k] * scaley;
                            float y = height - h;
                            g.FillRectangle(brushes[k], x[k], y, w[k], h);
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
