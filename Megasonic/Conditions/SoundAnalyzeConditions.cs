using Tools;

namespace Megasonic.Conditions
{
    internal class SoundAnalyzeConditions : Conditions
    {
        public SoundAnalyzeConditions(Control controlledControl) : base(controlledControl) { }

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
        private bool soundAnalyzed = false;

        private void TryInvokeMet()
        {
            if (soundSelected)
            {
                InvokeConditionsMet();
            }
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
