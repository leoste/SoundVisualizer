namespace Megasonic.Conditions
{
    abstract class ConditionsWithLineImageAndVideoDependencies : Conditions
    {
        public ConditionsWithLineImageAndVideoDependencies(Control controlledControl) : base(controlledControl) { }

        public void SetImageSelectedTrue()
        {
            imageSelected = true;
            TryInvoke();
        }
        public void SetLineSelectedTrue()
        {
            lineSelected = true;
            TryInvoke();
        }

        public void SetVideoNotRenderedFalse()
        {
            videoRendered = true;
            TryInvokeGone();
        }

        private bool imageSelected = false;
        private bool lineSelected = false;

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
            if (imageSelected && lineSelected)
            {
                InvokeConditionsMet();
            }
        }
    }
}
