using BenchmarkDotNet.Running;
using System;

namespace Tedd.DictionaryUtils.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DictionaryListOptimizationBenchmarks>();
        }
    }
}
