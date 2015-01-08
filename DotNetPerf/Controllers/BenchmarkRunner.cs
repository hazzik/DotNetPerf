using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet;
using DotNetPerf.Models;

namespace DotNetPerf.Test
{
    public sealed class BenchmarkRunner : MarshalByRefObject, IBenchmarkRunner
    {
        public object Run(byte[] assembly, string type)
        {
            var competition = (BenchmarkCompetition) Assembly.Load(assembly).CreateInstance(type);
            competition.Run();
            return competition.Tasks.Select(x => new BenchmarkResult
            {
                Name = x.Name,
                MinTicks = x.Info.Result.MinTicks,
                MaxTicks = x.Info.Result.MaxTicks,
                MedianTicks = x.Info.Result.MedianTicks,
                StandardDeviationTicks = x.Info.Result.StandardDeviationTicks,
                Error = x.Info.Result.Error
            }).ToArray();
        }
    }
}