using Tools;

namespace Megasonic.Conditions
{
    class VideoCustomizationConditions : Conditions
    {
        public VideoCustomizationConditions(Control controlledControl, ForegroundCustomizationConditions foregroundCustomizationConditions, SoundAnalyzeConditions soundAnalyzeConditions) : base(controlledControl)
        {
            foregroundCustomizationConditions.ConditionsMetEvent += ForegroundCustomizationConditions_ConditionsMetEvent;
            soundAnalyzeConditions.ConditionsMetEvent += SoundAnalyzeConditions_ConditionsMetEvent;
        }

        public void SetSoundAnalyzedTrue()
        {
            soundAnalyzed = true;
            TryInvoke();
        }

        public void SetVideoNotRenderedFalse()
        {
            videoRendered = true;
            TryInvokeGone();
        }

        private bool soundAnalyzed = false;
        private bool foregroundCustomizationConditionsMet = false;
        private bool soundAnalyzeConditionsMet = false;
        private bool videoRendered = false;

        private void TryInvokeGone()
        {
            if (videoRendered)
            {
                InvokeConditionsGone();
            }
        }

        private void TryInvoke()
        {
            if (soundAnalyzed && foregroundCustomizationConditionsMet && soundAnalyzeConditionsMet)
            {
                InvokeConditionsMet();
            }
        }

        private void ForegroundCustomizationConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            foregroundCustomizationConditionsMet = true;
            TryInvoke();
        }

        private void SoundAnalyzeConditions_ConditionsMetEvent(object? sender, EventArgs e)
        {
            soundAnalyzeConditionsMet = true;
            TryInvoke();
        }
    }
}
