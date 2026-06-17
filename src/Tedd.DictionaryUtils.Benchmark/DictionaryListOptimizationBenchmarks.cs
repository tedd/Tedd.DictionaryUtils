using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Tedd;
using Tedd.Archive;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class DictionaryListOptimizationBenchmarks
{
    private int[]? _intArray;

    [Params(100, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _intArray = Enumerable.Range(0, Size).ToArray();
    }

    [Benchmark(Baseline = true)]
    public Dictionary<int, int> ToDictionarySafeArchive() => Tedd.Archive.DictionaryUtilsExtensions.ToDictionarySafe(_intArray, x => x);

    [Benchmark]
    public Dictionary<int, int> ToDictionarySafeNew() => Tedd.DictionaryUtilsExtensions.ToDictionarySafe(_intArray, x => x);

    [Benchmark]
    public Dictionary<int, List<int>> ToDictionaryListArchive() => Tedd.Archive.DictionaryListUtilsExtensions.ToDictionaryList(_intArray, x => x);

    [Benchmark]
    public Dictionary<int, List<int>> ToDictionaryListNew() => Tedd.DictionaryListUtilsExtensions.ToDictionaryList(_intArray, x => x);
}
