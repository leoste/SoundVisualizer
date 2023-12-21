using Tools;

namespace Megasonic.Conditions
{
    internal class SoundAnalyzeConditions : Conditions
    {
        public SoundAnalyzeConditions(Control controlledControl, SoundCustomizationConditions soundCustomizationConditions) : base(controlledControl)
        {
            soundCustomizationConditions.ConditionsMetEvent += SoundCustomizationConditions_ConditionsMetEvent;
        }

        public void SetSoundSelectedTrue()
        {
            soundSelected = true;
            TryInvokeMet();
        }

        public void SetSoundNotAnalyzedFalse()
        {
            soundAnalyzed = true;
            TryInvokeGone();
        }

        private bool soundSelected = false;
        private bool soundCustomizationConditionsMet = false;
        private bool soundAnalyzed = false;

        private void TryInvokeMet()
        {
            if (soundSelected && soundCustomizationConditionsMet)
            {
                InvokeConditionsMet();
            }
        }

        private void SoundCustomizationConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundCustomizationConditionsMet = true;
            TryInvokeMet();
        }

        private void TryInvokeGone()
        {
            if (soundAnalyzed)
            {
                InvokeConditionsGone();
            }
        }
    }
}
