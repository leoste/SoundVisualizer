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
        public int FrameRate { get; set; }
        public int Lookahead { get; set; }
        public string Window { get; set; } = "";
    }

    class VideoOutputSettings
    {
        public string VideoFile { get; set; } = "";
        public int NoteRangeStart { get; set; }
        public int NoteRangeEnd { get; set; }
        public int DecayExponent { get; set; }
        public int DecayTime { get; set; }
        public int ColorStart { get; set; }
        public int ColorLength { get; set; }
        public int BarWidth { get; set; }
        public int BarMaxAngle { get; set; }
        public string Title { get; set; } = "";
        public int TitleHeightA { get; set; }
        public int TitleHeightB { get; set; }
    }
}
