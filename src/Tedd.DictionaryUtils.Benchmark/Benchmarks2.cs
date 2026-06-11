using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class DictionaryOptimizationBenchmarks
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
    public Dictionary<int, int> ContainsKeyAndAdd()
    {
        var d = new Dictionary<int, int>(_intArray.Length);
        for (int i = 0; i < _intArray.Length; i++)
        {
            int ks = _intArray[i];
            if (!d.ContainsKey(ks))
                d.Add(ks, _intArray[i]);
        }
        return d;
    }

    [Benchmark]
    public Dictionary<int, int> TryAdd()
    {
        var d = new Dictionary<int, int>(_intArray.Length);
        for (int i = 0; i < _intArray.Length; i++)
        {
            d.TryAdd(_intArray[i], _intArray[i]);
        }
        return d;
    }

    [Benchmark]
    public Dictionary<int, int> CollectionsMarshalRef()
    {
        var d = new Dictionary<int, int>(_intArray.Length);
        for (int i = 0; i < _intArray.Length; i++)
        {
            int ks = _intArray[i];
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(d, ks, out bool exists);
            if (!exists)
                val = _intArray[i];
        }
        return d;
    }
}
