using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megasonic.Conditions
{
    internal class BackgroundCustomizationConditions : Conditions
    {
        public BackgroundCustomizationConditions(Control controlledControl) : base(controlledControl, true) { }

        public void SetVideoNotRenderedFalse()
        {
            videoRendered = true;
            TryInvokeGone();
        }

        private bool videoRendered = false;

        private void TryInvokeGone()
        {
            if (videoRendered)
            {
                InvokeConditionsGone();
            }
        }
    }
}
