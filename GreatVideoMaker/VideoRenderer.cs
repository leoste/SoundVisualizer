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
            //only take certain amount of frequencies for rendering
            int maxFrequencyCount = 44100; //for frequencies needs to be way lower, like 4000, but for notes its ok

            float scaley = height / (sound.MaximumFrequency - sound.MinimumFrequency);

            for (int i = 0; i < sound.Frames.Length; i++)
            {
                int frequencyCount = Math.Min(maxFrequencyCount, sound.Frames[i].frequencies.Length);

                Bitmap bitmap = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Brush b = new SolidBrush(Color.Red);
                    g.Clear(Color.White);

                    float scalex = width / (float)frequencyCount;

                    for (int k = 0; k < frequencyCount; k++)
                    {
                        float x = k * scalex;

                        if (x < 0) throw new Exception("wtf");                        

                        float h = sound.Frames[i].notes[k] * scaley;
                        g.FillRectangle(b, x, height - h, scalex, h);
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
