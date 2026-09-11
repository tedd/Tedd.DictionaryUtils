using BenchmarkDotNet.Running;

namespace Tedd.DictionaryUtils.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<GetOrAddOptimizationBenchmarks>();
        }
    }
}
