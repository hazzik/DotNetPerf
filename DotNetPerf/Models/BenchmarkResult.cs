using System;
using System.ComponentModel;

namespace DotNetPerf.Models
{
    [Serializable]
    public class BenchmarkResult
    {
        public string Name;
        public long MinTicks;
        public long MaxTicks;
        public long MedianTicks;
        public double StandardDeviationTicks;
        public double Error;
    }
}