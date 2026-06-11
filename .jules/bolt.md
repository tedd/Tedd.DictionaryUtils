## 2026-06-11 - Dictionary Double-Hashing Elimination
**Observation:** Operations like `ToDictionarySafe` and `ToDictionaryList` checked `ContainsKey` or `TryGetValue` prior to an `Add` for missing keys, resulting in redundant O(1) hash lookups per key.
**Strategic Action:** Introduced `.NET 8.0` specific optimization utilizing `System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault()`. This retrieves a memory reference directly in a single pass, eliminating the double hash lookup latency.
