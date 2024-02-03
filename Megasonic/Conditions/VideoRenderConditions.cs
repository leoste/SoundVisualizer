using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Megasonic.Conditions
{
    class VideoRenderConditions : Conditions
    {
        public VideoRenderConditions(Control controlledControl, VideoCustomizationConditions videoCustomizationConditions) : base(controlledControl)
        {
            videoCustomizationConditions.ConditionsMetEvent += VideoCustomizationConditions_ConditionsMetEvent;
        }

        public void SetVideoSelectedTrue()
        {
            videoSelected = true;
            TryInvoke();
        }

        private bool videoSelected = false;
        private bool videoCustomizationConditionsMet = false;

        private void TryInvoke()
        {
            if (videoSelected && videoCustomizationConditionsMet)
            {
                InvokeConditionsMet();
            }
        }

        private void VideoCustomizationConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            videoCustomizationConditionsMet = true;
            TryInvoke();
        }
    }
}
