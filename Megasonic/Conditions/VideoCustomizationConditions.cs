namespace Megasonic.Conditions
{
    class VideoCustomizationConditions : Conditions
    {
        public VideoCustomizationConditions(Control controlledControl, SoundCustomizationConditions soundCustomizationConditions, SoundAnalyzeConditions soundAnalyzeConditions) : base(controlledControl)
        {
            soundCustomizationConditions.ConditionsMetEvent += SoundCustomizationConditions_ConditionsMetEvent;
            soundAnalyzeConditions.ConditionsMetEvent += SoundAnalyzeConditions_ConditionsMetEvent;
        }

        public void SetSoundAnalyzedTrue()
        {
            soundAnalyzed = true;
            TryInvoke();
        }

        private bool soundAnalyzed = false;
        private bool soundCustomizationConditionsMet = false;
        private bool soundAnalyzeConditionsMet = false;

        private void TryInvoke()
        {
            if (soundAnalyzed && soundCustomizationConditionsMet && soundAnalyzeConditionsMet)
            {
                InvokeConditionsMet();
            }
        }

        private void SoundCustomizationConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundCustomizationConditionsMet = true;
            TryInvoke();
        }

        private void SoundAnalyzeConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundAnalyzeConditionsMet = true;
            TryInvoke();
        }
    }
}
