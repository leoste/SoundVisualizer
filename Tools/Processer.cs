namespace Tools
{
    interface Processer
    {
        void StartProcess();
        event EventHandler<ProgressEventArgs>? OnProgress;
        event EventHandler? OnComplete;
    }

    public class ProgressEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public int Total { get; private set; }

        public ProgressEventArgs(int value, int total)
        {
            Value = value;
            Total = total;
        }
    }
}
