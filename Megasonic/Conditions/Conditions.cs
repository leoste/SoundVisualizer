namespace Megasonic.Conditions
{
    // TODO
    // b) add event args with bool: conditions met or not

    abstract class Conditions
    {
        protected Conditions(Control controlledControl, bool controlEnabled = false)
        {
            this.controlledControl = controlledControl;
            this.controlledControl.Enabled = controlEnabled;
        }

        internal event EventHandler? ConditionsMetEvent;
        internal event EventHandler? ConditionsGoneEvent;

        private Control controlledControl;

        protected void InvokeConditionsMet()
        {
            controlledControl.Enabled = true;
            ConditionsMetEvent?.Invoke(this, new EventArgs());
        }

        protected void InvokeConditionsGone()
        {
            controlledControl.Enabled = false;
            ConditionsGoneEvent?.Invoke(this, new EventArgs());
        }
    }
}
