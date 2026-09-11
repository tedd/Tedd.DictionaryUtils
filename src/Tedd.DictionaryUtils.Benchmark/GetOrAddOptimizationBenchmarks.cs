using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Tedd;
using Tedd.Archive;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class GetOrAddOptimizationBenchmarks
{
    private int[]? _keys;

    [Params(100, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _keys = Enumerable.Range(0, Size).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int GetOrAddArchive()
    {
        var dict = new Dictionary<int, int>();
        int sum = 0;
        for (int i = 0; i < _keys.Length; i++)
        {
            sum += Tedd.Archive.AddExtensions.GetOrAdd(dict, _keys[i], () => _keys[i]);
        }
        return sum;
    }

    [Benchmark]
    public int GetOrAddNew()
    {
        var dict = new Dictionary<int, int>();
        int sum = 0;
        for (int i = 0; i < _keys.Length; i++)
        {
            sum += Tedd.AddExtensions.GetOrAdd(dict, _keys[i], () => _keys[i]);
        }
        return sum;
    }
}
