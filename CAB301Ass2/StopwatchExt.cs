using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAB301Ass2
{
    public static class StopwatchExt
    {
        public static string GetTimeString(this Stopwatch stopwatch, int numberofDigits = 1)
        {
            double time = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
            if (time > 1)
                return Math.Round(time, numberofDigits) + ",s";
            if (time > 1e-3)
                return Math.Round(1e3 * time, numberofDigits) + ",ms";
            if (time > 1e-6)
                return Math.Round(1e6 * time, numberofDigits) + ",us";
            if (time > 1e-9)
                return Math.Round(1e9 * time, numberofDigits) + ",ns";
            return stopwatch.ElapsedTicks + " ticks";
        }
    }
}
