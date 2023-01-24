using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megasonic
{
    // TODO
    // a) replace get/set with functions to only set to true, dont allow setting to false
    // b) add event args with bool: conditions met or not

    abstract class Conditions
    {
        public abstract event EventHandler? ConditionsMetEvent;
    }

    class SoundAnalyzeConditions : Conditions
    {
        private bool soundSelected = false;

        public bool SoundSelected
        {
            get { return soundSelected; }
            set
            {
                soundSelected = value;
                if (soundSelected)
                {
                    ConditionsMetEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        public override event EventHandler? ConditionsMetEvent;

        public SoundAnalyzeConditions()
        {
            
        }
    }

    class VideoConditions : Conditions
    {
        private bool soundAnalyzed = false;
        private bool imageSelected = false;
        private bool lineSelected = false;

        public bool SoundAnalyzed
        {
            get { return soundAnalyzed; }
            set
            {
                soundAnalyzed = value;
                TryInvoke();
            }
        }
        public bool ImageSelected
        {
            get { return imageSelected; }
            set
            {
                imageSelected = value;
                TryInvoke();
            }
        }
        public bool LineSelected
        {
            get { return lineSelected; }
            set
            {
                lineSelected = value;
                TryInvoke();
            }
        }

        public override event EventHandler? ConditionsMetEvent;

        private void TryInvoke()
        {
            if (SoundAnalyzed && ImageSelected && LineSelected)
            {
                ConditionsMetEvent?.Invoke(this, new EventArgs());
            }
        }

        public VideoConditions()
        {
            
        }
    }

    class VideoRenderConditions : Conditions
    {
        private bool videoSelected = false;

        public bool VideoSelected
        {
            get { return videoSelected; }
            set
            {
                videoSelected = value;
                if (VideoSelected)
                {
                    ConditionsMetEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        public override event EventHandler? ConditionsMetEvent;
    }
}
