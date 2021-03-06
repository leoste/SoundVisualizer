﻿using System;
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
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using Svg;

namespace GreatVideoMaker
{
    class VideoRenderer : Processer
    {
        private string filepath;
        private string svgFilepath;
        private SoundAnalyzer sound;
        private Size frameSize;
        private int barRelation;
        private float colorStartDegree;
        private float colorLengthDegree;
        private int decayExponent;
        private int decayTime;
        private int maxNoteBorder;
        private int minNoteBorder;

        public event EventHandler<ProgressEventArgs> OnProgress;
        public event EventHandler OnComplete;

        public VideoRenderer(SoundAnalyzer sound, string filepath, string svgFilepath, Size frameSize,
            int barRelation = 128,
            float colorStartDegree = 0, // 216, 160
            float colorLengthDegree = 180, // 60, 60
            int decayExponent = 10,
            int decayTime = 5,
            int minNoteBorder = -36,
            int maxNoteBorder = 48)
        {
            this.filepath = filepath;
            this.svgFilepath = svgFilepath;
            this.sound = sound;
            this.frameSize = frameSize;
            this.barRelation = barRelation;
            this.colorStartDegree = colorStartDegree;
            this.colorLengthDegree = colorLengthDegree;
            this.decayExponent = decayExponent;
            this.decayTime = decayTime;
            this.minNoteBorder = minNoteBorder;
            this.maxNoteBorder = maxNoteBorder;
        }

        IEnumerable<IVideoFrame> CreateFrames()
        {
            float columnRelation = 1f / barRelation;
            float scaleThingy = colorLengthDegree / 6;

            float halfHeight = frameSize.Height / 2f;
            float columnWidth = frameSize.Width * columnRelation;
            float columnHalfWidth = columnWidth / 2;
            float noteMinoffset = -(sound.MinimumNote - minNoteBorder);
            float noteMaxOffset = (sound.MaximumNote - maxNoteBorder);
            float visibleNoteSpan = sound.NotesTotalLength - noteMinoffset - noteMaxOffset;
            bool startIsntSet = true;
            bool endIsntSet = true;

            float scalex = frameSize.Width / visibleNoteSpan;
            float basey = (sound.MaximumAmplitude - sound.MinimumAmplitude);
            float scaley = frameSize.Width / 7f / basey; //width is 7x height ...
            //float scalealpha = 255 / basey;

            //color preparation for image calculation            
            Color[] colors = new Color[frameSize.Width]; //theres only really point in calculating color for each pixel
            float colorRelation = visibleNoteSpan / colors.Length;
            Hsv hsv = new Hsv
            {
                S = 1,
                V = 1
            };
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
            float decayCountMargin = (float)decayExponent / decayTime;

            // maybe want to do curve fluxuations or something cool
            PointF[] curvePoints;
            SvgDocument document = SvgDocument.Open(svgFilepath);
            using (GraphicsPath path = SvgConverter.ToGraphicsPath(document))
            {

                float unit = frameSize.Width / 7;
                path.Transform(new Matrix(path.GetBounds(), new PointF[] {
                    new PointF(unit, unit),
                    new PointF(frameSize.Width - unit, unit),
                    new PointF(unit, frameSize.Height - unit)
                }));
                using (Matrix mx = new Matrix(1, 0, 0, 1, 0, 0))
                {
                    path.Flatten(mx, 0.1f);
                    curvePoints = path.PathPoints;
                }
            }
            // calculate length of the curve
            CurveOperations.CalculateLength(curvePoints, out double curveLength, out double[] curveLengths);
            double definition = frameSize.Width / barRelation * frameSize.Width / curveLength;

            // this function IS also threadsafe now!!! doesnt modify anything anymore
            PointF[] GetSourcePoints(int index)
            {
                // first get the points that we do know
                PointF[] sourcePoints = new PointF[visibleLength];
                for (int k = 0; k < visibleLength; k++)
                {
                    int correctedIndex = k + startIndex;
                    float baseh = sound.Frames[index].frequencies[correctedIndex];

                    for (int l = 1; l < Math.Min(index, decayTime); l++)
                    {
                        float oldBaseh = sound.Frames[index - l].frequencies[correctedIndex];
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
                    sourcePoints[k] = new PointF(x[correctedIndex], h);
                }
                return sourcePoints;
            }

            // this function IS threadsafe!!! doesnt modify anything
            BitmapVideoFrameWrapper GetFrame(PointF[] sourcePoints)
            {
                PointF[] uniformPoints = CurveOperations.SpecifyHorizontally(sourcePoints, definition);

                CurveMorpher curve = new CurveMorpher(curvePoints, uniformPoints, curveLength, curveLengths, false);

                // i dont use "using" cause bitmap needs to stay for a while until its really used, then i dispose it
                Bitmap bitmap = new Bitmap(frameSize.Width, frameSize.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Black);

                    for (int k = 0; k < uniformPoints.Length; k++)
                    {
                        Color c = colors[(int)uniformPoints[k].X];
                        Brush brush = new SolidBrush(c);

                        PointF a = curve.Matrix.Points[k];
                        PointF b = curve.Matrix.GetSecondPoint(k, uniformPoints[k].Y);
                        g.DrawLine(new Pen(brush, columnWidth), a, b);
                    }
                }
                BitmapVideoFrameWrapper wrapper = new BitmapVideoFrameWrapper(bitmap);
                return wrapper;
            }

            ConcurrentDictionary<int, BitmapVideoFrameWrapper> dictionary = new ConcurrentDictionary<int, BitmapVideoFrameWrapper>();
            List<BackgroundWorker> bgws = new List<BackgroundWorker>();
            int takeIndex = 0;
            int takes = sound.Frames.Length;
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

                    PointF[] sourcePoints = GetSourcePoints(index);
                    BitmapVideoFrameWrapper wrapper = GetFrame(sourcePoints);

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
