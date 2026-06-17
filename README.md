# Tedd.DictionaryUtils

A rigorous architectural framework providing advanced operational primitives and extensions for the .NET `Dictionary<TKey, TValue>` infrastructure.

## Architectural Paradigm

This library is engineered to mitigate fundamental data structure initialization anomalies and operational friction when manipulating high-volume collections in .NET applications. It achieves this by extending the standard `IEnumerable<T>` and `Dictionary<TKey, TValue>` types with deterministic, robust execution pathways for data aggregation and dictionary population.

The core mechanisms emphasize safe instantiation (bypassing duplicate key exceptions), transparent collection-based aggregation, and streamlined data retrieval paradigms.

## Operational Capabilities (Established)

### Safe Dictionary Instantiation: `ToDictionarySafe`

The standard `ToDictionary` LINQ method throws an `ArgumentException` when it encounters duplicate keys. `ToDictionarySafe` instead skips subsequent occurrences of an already-seen key, keeping the first value encountered during enumeration.

```csharp
using System.Collections.Generic;
using Tedd;

var dataset = new[]
{
    new { Id = 1, Value = "Primary Data" },
    new { Id = 1, Value = "Anomalous Data" }, // Duplicate Key
    new { Id = 2, Value = "Secondary Data" }
};

// Instantiates securely without throwing ArgumentException
Dictionary<int, string> safeDictionary = dataset.ToDictionarySafe(
    keySelector: x => x.Id,
    elementSelector: x => x.Value
);

// State verification: safeDictionary[1] == "Primary Data"
```

### Collection-Based Aggregation: `ToDictionaryList`

When the architectural mandate requires the preservation of duplicate keys by mapping them to an aggregate collection, `ToDictionaryList` constructs a complex `Dictionary<TKey, List<TValue>>` structure in a single operational pass. This provides a highly optimized paradigm for hierarchical data binding and grouping.

```csharp
using System.Collections.Generic;
using Tedd;

var systemLogs = new[]
{
    new { Severity = "ERROR", Message = "Null reference detected." },
    new { Severity = "INFO", Message = "Service initialized." },
    new { Severity = "ERROR", Message = "Timeout exception." }
};

// Constructs a mapping of Severity to an aggregated List of Messages
Dictionary<string, List<string>> aggregatedLogs = systemLogs.ToDictionaryList(
    keySelector: x => x.Severity,
    elementSelector: x => x.Message
);

// State verification: aggregatedLogs["ERROR"].Count == 2
```

### Deferred Value Materialization: `GetOrAdd`

To optimize resource allocation during complex state retrieval, `GetOrAdd` provides a deterministic pathway to retrieve an existing value from a dictionary or dynamically execute a factory delegate to instantiate and insert a new value if the key is absent.

```csharp
using System.Collections.Generic;
using Tedd;

var executionCache = new Dictionary<string, string>();

// Retrieves existing or dynamically instantiates via the factory function
string configuration = executionCache.GetOrAdd("DbConnectionString", () =>
{
    // High-latency allocation mechanism executed conditionally
    return "Server=primary;Database=core;";
});
```

## Planned Future Enhancements (Hypotheses)

### Object Graph Serialization: `DictionarySerializer` (Pending)

Future iterations of the framework anticipate the integration of comprehensive object graph traversal and flattening capabilities via the `DictionarySerializer`. This infrastructure will recursively unwrap complex generic objects and collections into a strict, unified, scalar-value dictionary (`Dictionary<string, object>`).

*Note: This architectural component is currently gated behind the `SERIALIZER` compilation symbol and remains subject to empirical validation before formal integration into the public API.*
