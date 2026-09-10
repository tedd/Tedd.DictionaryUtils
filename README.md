# Tedd.DictionaryUtils

A rigorous architectural framework providing advanced operational primitives and extensions for the .NET `Dictionary<TKey, TValue>` infrastructure.

## Architectural Paradigm

This library is engineered to mitigate fundamental data structure initialization anomalies and operational friction when manipulating high-volume collections in .NET applications. It achieves this by extending the standard `IEnumerable<T>` and `Dictionary<TKey, TValue>` types with deterministic, robust execution pathways for data aggregation and dictionary population.

The core mechanisms emphasize safe instantiation (bypassing duplicate key exceptions), transparent collection-based aggregation, and streamlined data retrieval paradigms.

Crucially, the framework facilitates complex **hierarchical data binding and routed event infrastructure**. By utilizing operations like `ToDictionaryList`, developers can rapidly project flat data streams into optimized, heavily-nested topologies (`Dictionary<TKey, List<TValue>>`). This specific architectural topology underpins efficient hierarchical data binding in UI frameworks and enables the construction of high-performance routed event dispatchers where multiple delegates or event payloads are mapped to specific, deterministic keys. This structural mapping specifically mitigates the need for speculative assumptions regarding event flow and data grouping.

## Operational Capabilities (Established)

### Safe Dictionary Instantiation: `ToDictionarySafe`

The standard `ToDictionary` LINQ method throws an `ArgumentException` when it encounters duplicate keys. `ToDictionarySafe` instead skips subsequent occurrences of an already-seen key, keeping the first value encountered during enumeration.

```csharp
using System.Collections.Generic;
using Tedd;

(int Id, string Value)[] dataset =
[
    (1, "Primary Data"),
    (1, "Anomalous Data"), // Duplicate Key
    (2, "Secondary Data")
];

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

(string Severity, string Message)[] systemLogs =
[
    ("ERROR", "Null reference detected."),
    ("INFO", "Service initialized."),
    ("ERROR", "Timeout exception.")
];

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

Dictionary<string, string> executionCache = new();

// Retrieves existing or dynamically instantiates via the factory function
string configuration = executionCache.GetOrAdd("DbConnectionString", () =>
{
    // High-latency allocation mechanism executed conditionally
    return "Server=primary;Database=core;";
});
```

## Planned Future Enhancements (Hypotheses)

### Object Graph Serialization: `DictionarySerializer` (Pending)

An experimental `DictionarySerializer` implementation (compiled only when the `SERIALIZER` symbol is defined) traverses an object graph and flattens it into a `Dictionary<string, object>`.

*Note: This component is not included in the default build and remains subject to validation before being treated as a stable public API.*
