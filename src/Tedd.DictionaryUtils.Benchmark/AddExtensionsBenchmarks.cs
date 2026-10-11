using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using Tedd;
using Tedd.Archive;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class AddExtensionsBenchmarks
{
    private Dictionary<int, int>? _dictArchiveExisting;
    private Dictionary<int, int>? _dictNewExisting;
    private Dictionary<int, int>? _dictArchiveAdd;
    private Dictionary<int, int>? _dictNewAdd;
    private int[]? _keysToGet;
    private int[]? _keysToAdd;

    [Params(100, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _dictArchiveExisting = new Dictionary<int, int>();
        _dictNewExisting = new Dictionary<int, int>();

        _keysToGet = Enumerable.Range(0, Size).ToArray();
        _keysToAdd = Enumerable.Range(Size, Size).ToArray();

        // Populate dictionaries with initial data for 'Existing' tests
        foreach (var key in _keysToGet)
        {
            _dictArchiveExisting.Add(key, key);
            _dictNewExisting.Add(key, key);
        }
    }

    [IterationSetup(Targets = new[] { nameof(GetOrAddArchive_NewKeys), nameof(GetOrAddNew_NewKeys) })]
    public void IterationSetupAdd()
    {
        // Reset the dictionaries before each benchmark iteration for adding new keys
        // to avoid key collisions and accurately measure Add performance.
        _dictArchiveAdd = new Dictionary<int, int>();
        _dictNewAdd = new Dictionary<int, int>();
    }

    [Benchmark(Baseline = true)]
    public int GetOrAddArchive_ExistingKeys()
    {
        int sum = 0;
        for (int i = 0; i < _keysToGet!.Length; i++)
        {
            sum += Tedd.Archive.AddExtensions.GetOrAdd(_dictArchiveExisting!, _keysToGet[i], () => _keysToGet[i]);
        }
        return sum;
    }

    [Benchmark]
    public int GetOrAddNew_ExistingKeys()
    {
        int sum = 0;
        for (int i = 0; i < _keysToGet!.Length; i++)
        {
            sum += Tedd.AddExtensions.GetOrAdd(_dictNewExisting!, _keysToGet[i], () => _keysToGet[i]);
        }
        return sum;
    }

    [Benchmark]
    public int GetOrAddArchive_NewKeys()
    {
        int sum = 0;
        for (int i = 0; i < _keysToAdd!.Length; i++)
        {
            sum += Tedd.Archive.AddExtensions.GetOrAdd(_dictArchiveAdd!, _keysToAdd[i], () => _keysToAdd[i]);
        }
        return sum;
    }

    [Benchmark]
    public int GetOrAddNew_NewKeys()
    {
        int sum = 0;
        for (int i = 0; i < _keysToAdd!.Length; i++)
        {
            sum += Tedd.AddExtensions.GetOrAdd(_dictNewAdd!, _keysToAdd[i], () => _keysToAdd[i]);
        }
        return sum;
    }
}
