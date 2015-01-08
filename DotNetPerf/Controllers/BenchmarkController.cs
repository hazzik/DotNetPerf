using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Http;
using BenchmarkDotNet;
using DotNetPerf.Models;
using DotNetPerf.Test;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp.RuntimeBinder;

namespace DotNetPerf.Controllers
{
    public class BenchmarkController : ApiController
    {
        public BenchmarkResult[] Execute(BenchmarkRequest benchmark)
        {
            var sources = new List<string> {B};
            if (benchmark.Tests != null)
                sources.AddRange(benchmark.Tests.Select((test, index) => MakeBenchmarkMethod(test.Name, index, test.Code)));
            var assembly = CompileRaw(sources);

            var domain = AppDomain.CreateDomain("RunDomain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

            var runner = (IBenchmarkRunner)domain.CreateInstanceAndUnwrap(typeof(BenchmarkRunner).Assembly.FullName, typeof(BenchmarkRunner).FullName);

            return (BenchmarkResult[])runner.Run(assembly, "B");
        }

        private static byte[] CompileRaw( IEnumerable<string> sources)
        {
            var compilation = CSharpCompilation.Create("Benchmark" + Guid.NewGuid().ToString("N"),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: sources.Select(source => CSharpSyntaxTree.ParseText(source)),
                references: new[]
                {
                    MetadataReference.CreateFromAssembly(typeof (object).Assembly),
                    MetadataReference.CreateFromAssembly(typeof (RuntimeBinderException).Assembly),
                    MetadataReference.CreateFromAssembly(typeof (DynamicAttribute).Assembly),
                    MetadataReference.CreateFromAssembly(typeof (BenchmarkCompetition).Assembly)
                });

            EmitResult emitResult;

            using (var ms = new MemoryStream())
            {
                emitResult = compilation.Emit(ms);

                if (emitResult.Success)
                {
                    ms.Flush();
                    return ms.ToArray();
                }
            }

            var message = String.Join("\r\n", emitResult.Diagnostics);
            throw new ApplicationException(message);
        }

        private static readonly string B = @"using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet;

[Serializable]
public partial class B : BenchmarkCompetition {}";

        private static string MakeBenchmarkMethod(string name, int index, string code)
        {
            const string benchmarkMethodTemplate = @"using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet;

public partial class B : BenchmarkCompetition
{{
    [BenchmarkMethod(""{0}"")]public void BenchmarkMethod{1} () 
    {{
        {2};
    }} 
}}";
            return String.Format(benchmarkMethodTemplate, name, index, code);
        }
    }
}