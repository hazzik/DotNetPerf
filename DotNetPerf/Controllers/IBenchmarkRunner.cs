namespace DotNetPerf.Test
{
    public interface IBenchmarkRunner
    {
        object Run(byte[] assembly, string typeName);
    }
}