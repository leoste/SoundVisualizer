namespace Megasonic.Conditions
{
    class SoundCustomizationConditions : Conditions
    {
        public SoundCustomizationConditions(Control controlledControl, ForegroundCustomizationConditions foregroundCustomizationConditions) : base(controlledControl)
        {
            foregroundCustomizationConditions.ConditionsMetEvent += ForegroundCustomizationConditions_ConditionsMetEvent;
        }

        public void SetSoundNotAnalyzedFalse()
        {
            soundAnalyzed = true;
            TryInvokeGone();
        }

        private bool foregroundCustomizationConditionsMet = false;
        private bool soundAnalyzed = false;

        private void ForegroundCustomizationConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            foregroundCustomizationConditionsMet = true;
            TryInvokeMet();
        }

        private void TryInvokeMet()
        {
            if (foregroundCustomizationConditionsMet)
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
