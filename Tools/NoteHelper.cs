
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class NoteHelper
    {
        public static readonly double baseFrequency = 440;

        public static float GetNoteDistance(int comparedFrequency)
        {
            float result;

            double divided = comparedFrequency / baseFrequency;
            double logged = Math.Log(divided, 2);
            result = (float)(logged * 12);

            return result;
        }

        public static int GetNoteFrequency(float noteDistance)
        {
            int result;

            double divided = noteDistance / 12f;
            double exponented = Math.Pow(divided, 2);
            result = (int)(exponented * baseFrequency);

            return result;
        }
    }
}
