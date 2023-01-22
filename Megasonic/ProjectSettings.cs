using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megasonic
{
    class ProjectSettings
    {
        public VideoSourceSettings VideoSourceSettings { get; set; } = new VideoSourceSettings();
        public SoundSourceSettings SoundSourceSettings { get; set; } = new SoundSourceSettings();
        public VideoOutputSettings VideoOutputSettings { get; set; } = new VideoOutputSettings();
    }

    class VideoSourceSettings 
    {
        public string ImageFile { get; set; } = "";
        public string LineFile { get; set; } = "";
        public Size FrameSize { get; set; } = new Size();
    }

    class SoundSourceSettings
    {
        public string SoundFile { get; set; } = "";
        public int Framerate { get; set; }
        public int Lookahead { get; set; }
        public string Window { get; set; } = "";
    }

    class VideoOutputSettings
    {
        public string VideoFile { get; set; } = "";
        public Range NoteRange { get; set; } = new Range();
        public int DecayExponent { get; set; }
        public int DecayTime { get; set; }
        public int ColorStart { get; set; }
        public int ColorLength { get; set; }
        public int BarWidth { get; set; }
        public string Title { get; set; } = "";
    }
}
