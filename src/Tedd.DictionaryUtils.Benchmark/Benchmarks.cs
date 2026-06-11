using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Tedd;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class DictionaryBenchmarks
{
    private int[]? _intArray;
    private List<int>? _intList;

    [Params(100, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _intArray = Enumerable.Range(0, Size).ToArray();
        _intList = _intArray.ToList();
    }

    [Benchmark(Baseline = true)]
    public Dictionary<int, int> ToDictionaryLinq() => _intArray.ToDictionary(x => x);

    [Benchmark]
    public Dictionary<int, int> ToDictionarySafeArray() => _intArray.ToDictionarySafe(x => x);

    [Benchmark]
    public Dictionary<int, int> ToDictionarySafeList() => _intList.ToDictionarySafe(x => x);
}
