using FFMpegCore;
using FFMpegCore.Pipes;
using FFMpegCore.Extend;
using ColorMine.ColorSpaces;
using System.Collections.Concurrent;
using System.ComponentModel;
using FFMpegCore.Enums;

namespace Tools
{
    public class VideoRenderer : Processer
    {
        private string filepath;
        private string svgFilepath;
        private string imageFilepath;
        private string title;
        private SoundAnalyzer sound;
        private Size frameSize;
        private int barRelation;
        private int barMaxAngle;
        private float colorStartDegree;
        private float colorLengthDegree;
        private int decayExponent;
        private int decayTime;
        private int maxNoteBorder;
        private int minNoteBorder;
        private int titleHeightA;
        private int titleHeightB;

        public event EventHandler<ProgressEventArgs>? OnProgress;
        public event EventHandler? OnComplete;

        public PointF[] CurvePoints { get { return curvePoints; } }
        public Bitmap Background { get { return new Bitmap(background); } }
        public int MaxIndex { get; private set; }

        public VideoRenderer(SoundAnalyzer sound, string filepath, string svgFilepath, string imageFilepath, string title, Size frameSize,
            int barRelation = 128,
            int barMaxAngle = 0,
            float colorStartDegree = 0, // 216, 160
            float colorLengthDegree = 180, // 60, 60
            int decayExponent = 10,
            int decayTime = 5,
            int minNoteBorder = -36,
            int maxNoteBorder = 48,
            int titleHeightA = 2,
            int titleHeightB = 3)
        {
            this.filepath = filepath;
            this.svgFilepath = svgFilepath;
            this.imageFilepath = imageFilepath;
            this.title = title;
            this.sound = sound;
            this.frameSize = frameSize;
            this.barRelation = barRelation;
            this.barMaxAngle = barMaxAngle;
            this.colorStartDegree = colorStartDegree;
            this.colorLengthDegree = colorLengthDegree;
            this.decayExponent = decayExponent;
            this.decayTime = decayTime;
            this.minNoteBorder = minNoteBorder;
            this.maxNoteBorder = maxNoteBorder;
            this.titleHeightA = titleHeightA;
            this.titleHeightB = titleHeightB;

            MaxIndex = sound.Frames.Length;

            Prepare();
        }

        float scaleThingy;

        float columnWidth;
        float columnHalfWidth;
        float noteMinoffset;
        float noteMaxOffset;
        float visibleNoteSpan;
        bool startIsntSet;
        bool endIsntSet;

        float scalex;
        float basey;
        float scaley;
        //float scalealpha;

        Color[] colors;

        float[] x;
        int frequencyStartIndex;
        int endIndex;

        int visibleLength;

        float decayCountMargin;

        PointF[] curvePoints;
        byte[] curveTypes;
        double curveLength;
        double[] curveLengths;
        double definition;

        Bitmap background;
        private bool disposedValue;        
        
        // TODO: split into soft and hard prepare. soft prepare is really lightweight and runs on initialization, hard prepare runs only when render button clicked.
        // sound processing related stuff is hard preparations. taking them away from initialization means that need to make different intermediate picture rendering
        // methods to still allow for previews.
        void Prepare()
        {
            scaleThingy = DrawingAids.GetScaleThingy(colorLengthDegree);

            columnWidth = DrawingAids.GetColumnWidth(frameSize.Width, barRelation);
            columnHalfWidth = columnWidth / 2;
            noteMinoffset = -(sound.MinimumNote - minNoteBorder);
            noteMaxOffset = (sound.MaximumNote - maxNoteBorder);
            visibleNoteSpan = sound.NotesTotalLength - noteMinoffset - noteMaxOffset;
            startIsntSet = true;
            endIsntSet = true;

            scalex = frameSize.Width / visibleNoteSpan;
            basey = (sound.MaximumAmplitude - sound.MinimumAmplitude);
            scaley = frameSize.Width / 7f / basey; //width is 7x height ...
            //scalealpha = 255 / basey;

            colors = DrawingAids.GetColors(frameSize.Width, scaleThingy, visibleNoteSpan, noteMinoffset, colorStartDegree);

            //calculate x, find edges for notes that are actually visible
            x = new float[sound.FrequencyCount];
            frequencyStartIndex = 0; //index of first note we can start on
            endIndex = sound.FrequencyCount; // index of last note used
            for (int i = 0; i < sound.FrequencyCount; i++)
            {
                x[i] = (sound.NoteSpans[i].start - sound.MinimumNote - noteMinoffset) * scalex - columnHalfWidth;
                if (startIsntSet)
                {
                    if (sound.NoteSpans[i].start >= minNoteBorder)
                    {
                        frequencyStartIndex = i;
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

            visibleLength = endIndex - frequencyStartIndex;

            // anti-spaz measures
            decayCountMargin = (float)decayExponent / decayTime;

            (curvePoints, curveTypes) = DrawingAids.GetCurve(svgFilepath, frameSize.Width, frameSize.Height);

            (curveLength, curveLengths, definition) = DrawingAids.GetCurveProperties(curvePoints, frameSize.Width, barRelation);

            background = DrawingAids.GetBackgroundWithTitle(imageFilepath, frameSize.Width, frameSize.Height, title, titleHeightA, titleHeightB);
        }

        // this function IS also threadsafe now!!! doesnt modify anything anymore
        public PointF[] getProcessedFrequencyPoints(int frameIndex)
        {
            // first get the points that we do know
            PointF[] frequencyPoints = new PointF[visibleLength];
            for (int k = 0; k < visibleLength; k++)
            {
                int correctedFrequencyIndex = k + frequencyStartIndex;
                float baseh = sound.Frames[frameIndex].frequencies[correctedFrequencyIndex];

                for (int l = 1; l < Math.Min(frameIndex, decayTime); l++)
                {
                    float oldBaseh = sound.Frames[frameIndex - l].frequencies[correctedFrequencyIndex];
                    if (oldBaseh >= baseh)
                    {
                        float decayCounts_k = decayExponent - l * decayCountMargin;
                        float decays_k = (float)Math.Log(decayCounts_k, decayExponent) * oldBaseh;

                        if (decays_k > baseh)
                        {
                            baseh = decays_k;
                        }
                    }
                }

                float h = baseh * scaley;
                frequencyPoints[k] = new PointF(x[correctedFrequencyIndex], h);
            }

            // really bad hackfix that loses very slight definition and is inaccurate, but....
            // visually isnt that different + its easier than reworking everything to not have this issue
            frequencyPoints[0].X = 0;

            return frequencyPoints;
        }

        // this function IS threadsafe!!! doesnt modify anything
        public BitmapVideoFrameWrapper GetFrame(PointF[] frequencyPoints)
        {
            Bitmap bitmap = DrawingAids.GetFrame(frequencyPoints, curvePoints, curveTypes, curveLength, curveLengths, background, colors, barMaxAngle, definition, columnWidth);            
            BitmapVideoFrameWrapper wrapper = new(bitmap);
            return wrapper;
        }

        IEnumerable<IVideoFrame> CreateFrames()
        {
            ConcurrentDictionary<int, BitmapVideoFrameWrapper> dictionary = new ConcurrentDictionary<int, BitmapVideoFrameWrapper>();
            List<BackgroundWorker> bgws = new List<BackgroundWorker>();
            int takeIndex = 0;
            int takes = MaxIndex;
            object lockLock = new object();

            // prevent memory usage exploding when renderer hangs up
            int frameRenderBuffer = RenderInfo.ProcessorCount * 2;
            AutoResetEvent[] loopLocks = new AutoResetEvent[takes];
            AutoResetEvent[] workLocks = new AutoResetEvent[takes];

            for (int i = 0; i < takes; i++) loopLocks[i] = new AutoResetEvent(false);
            for (int i = 0; i < frameRenderBuffer; i++) workLocks[i] = new AutoResetEvent(true);
            for (int i = frameRenderBuffer; i < workLocks.Length; i++) workLocks[i] = new AutoResetEvent(false);

            void Bgw_DoWork(object sender, DoWorkEventArgs e)
            {
                bool quit;

                while (true)
                {
                    // get index of what will be worked on
                    int index = 0;
                    lock (lockLock)
                    {
                        if (takeIndex < takes)
                        {
                            index = takeIndex;
                            takeIndex++;
                            quit = false;
                        }
                        else quit = true;
                    }

                    if (quit) break;

                    // wait for permission from main loop so memory wouldnt explode
                    workLocks[index].WaitOne();
                    workLocks[index].Dispose();

                    PointF[] frequencyPoints = getProcessedFrequencyPoints(index);
                    BitmapVideoFrameWrapper wrapper = GetFrame(frequencyPoints);

                    bool success = dictionary.TryAdd(index, wrapper);
                    AnalyzeSuccess(success);

                    // allow frame to be used
                    loopLocks[index].Set();
                }
            }

            void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                BackgroundWorker bgw = sender as BackgroundWorker;

                lock (bgws)
                {
                    bgws.Remove(bgw);
                    bgw.Dispose();
                    if (bgws.Count == 0)
                    {
                        
                    }
                }
            }

            void AnalyzeSuccess(bool success)
            {
                if (!success) throw new Exception("oi ei");
            }

            for (int i = 0; i < RenderInfo.ProcessorCount - 1; i++)
            {
                BackgroundWorker bgw = new BackgroundWorker();
                bgws.Add(bgw);
                bgw.DoWork += Bgw_DoWork;
                bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
                bgw.RunWorkerAsync();
            }

            for (int i = 0; i < takes; i++)
            {
                loopLocks[i].WaitOne();
                loopLocks[i].Dispose();
                BitmapVideoFrameWrapper wrapper;
                bool success = dictionary.TryRemove(i, out wrapper);
                AnalyzeSuccess(success);
                yield return wrapper;
                wrapper.Dispose();
                int setIndex = i + frameRenderBuffer;
                if (setIndex < takes) workLocks[setIndex].Set();
            }
        }

        public void StartProcess()
        {
            Task task = new Task(() =>
            {                
                //CreateFrames().ToList(); // artificial "rendering" for testing
                var videoFramesSource = new RawVideoPipeSource(CreateFrames()) { FrameRate = sound.FrameRate };
                FFMpegArguments
                    .FromPipeInput(videoFramesSource)
                    .AddFileInput(new FileInfo(sound.SourceFilePath))
                    .OutputToFile(filepath, true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithConstantRateFactor(21)
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithVariableBitrate(4)
                        .WithVideoFilters(filterOptions => filterOptions
                            .Scale(frameSize)
                        )
                        // ffmpeg picks default highest, yuv444p. but most players cant use it, so have to pick lower
                        .ForcePixelFormat("yuv420p")
                    )
                    .NotifyOnProgress(new Action<TimeSpan>((TimeSpan t) => {
                        OnProgress?.Invoke(this,
                            new ProgressEventArgs((int)t.TotalSeconds, (int)sound.TotalLength.TotalSeconds));
                    }))
                    .ProcessSynchronously();

                OnComplete?.Invoke(this, EventArgs.Empty);
            });
            task.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    background.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~VideoRenderer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
