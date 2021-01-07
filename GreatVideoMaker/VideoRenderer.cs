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
            float scaley = height / (sound.MaximumAmplitude - sound.MinimumAmplitude);

            //this is for note-based visualization
            for (int i = 0; i < sound.Frames.Length; i++)
            {
                Bitmap bitmap = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);

                    float scalex = width / sound.NotesTotalLength;
                    for (int k = 0; k < sound.Frames[i].frequencies.Length; k++)
                    {
                        double thingy = sound.NoteSpans[k].start % 12 / 12;
                        if (thingy < 0) thingy = 1 + thingy;
                        Hsv hsv = new Hsv();
                        hsv.S = 1;
                        hsv.V = 1;
                        hsv.H = 360 * thingy;
                        IRgb rgb = hsv.ToRgb();
                        Color c = Color.FromArgb(255, (int)rgb.R, (int)rgb.G, (int)rgb.B);
                        Brush b = new SolidBrush(c);

                        float x = (sound.NoteSpans[k].start - sound.MinimumNote) * scalex;
                        float w = sound.NoteSpans[k].length * scalex;
                        float h = sound.Frames[i].frequencies[k] * scaley;
                        float y = height - h;
                        g.FillRectangle(b, x, y, w, h);
                    }
                }

                BitmapVideoFrameWrapper wrapper = new BitmapVideoFrameWrapper(bitmap);
                yield return wrapper;
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
