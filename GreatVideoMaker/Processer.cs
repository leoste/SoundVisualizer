using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatVideoMaker
{
    interface Processer
    {
        void StartProcess();
        event EventHandler<ProgressEventArgs> OnProgress;
        event EventHandler OnComplete;
    }

    class ProgressEventArgs : EventArgs
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
