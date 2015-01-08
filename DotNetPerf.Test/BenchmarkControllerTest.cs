using DotNetPerf.Controllers;
using DotNetPerf.Models;
using NUnit.Framework;

namespace DotNetPerf.Test
{
    [TestFixture]
    public class BenchmarkControllerTest
    {
        [Test]
        public void RunCompiledBenchmarkMethod()
        {
            var results = new BenchmarkController().Execute(new BenchmarkRequest
            {
                Tests = new[]
                {
                    new BenchmarkUnit
                    {
                        Name = "Simple",
                        Code = "var i = 0; i++;"
                    },
                }
            });
            Assert.That(results.Length, Is.EqualTo(1));
            Assert.That(results[0].Name, Is.EqualTo("Simple"));
        }
    }
}