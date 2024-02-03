namespace Megasonic.Conditions
{
    class SoundCustomizationConditions : Conditions
    {
        public SoundCustomizationConditions(Control controlledControl) : base(controlledControl, true) { }

        public void SetSoundNotAnalyzedFalse()
        {
            soundAnalyzed = true;
            TryInvokeGone();
        }

        private bool soundAnalyzed = false;

        private void TryInvokeGone()
        {
            if (soundAnalyzed)
            {
                InvokeConditionsGone();
            }
        }
    }
}
