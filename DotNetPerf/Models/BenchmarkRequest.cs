namespace DotNetPerf.Models
{
    public class BenchmarkRequest
    {
        public BenchmarkUnit SetUp { get; set; }

        public BenchmarkUnit[] Tests { get; set; }
        public BenchmarkUnit TearDown { get; set; }
        public string[] AdditionalClasses { get; set; }
    }
}