namespace Megasonic.Conditions
{
    class ForegroundCustomizationConditions : Conditions
    {
        public ForegroundCustomizationConditions(Control controlledControl) : base(controlledControl) { }

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

        private bool imageSelected = false;
        private bool lineSelected = false;

        private void TryInvoke()
        {
            if (imageSelected && lineSelected)
            {
                InvokeConditionsMet();
            }
        }
    }
}
