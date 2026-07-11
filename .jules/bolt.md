## 2026-06-11 - Dictionary Double-Hashing Elimination
**Observation:** Operations like `ToDictionarySafe` and `ToDictionaryList` checked `ContainsKey` or `TryGetValue` prior to an `Add` for missing keys, resulting in redundant O(1) hash lookups per key.
**Strategic Action:** Introduced `.NET 8.0` specific optimization utilizing `System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault()`. This retrieves a memory reference directly in a single pass, eliminating the double hash lookup latency.

## 2024-07-11 - DictionaryListUtilsExtensions & DictionaryUtilsExtensions Refactoring

**Observation:** The previous refactoring of `ToDictionarySafe` and `ToDictionaryList` (using `CollectionsMarshal.GetValueRefOrAddDefault` instead of `TryGetValue` + `Add`) is verified as successful. The performance delta is a measurable reduction in Mean duration (~0.79x ratio for Size=100 and ~0.85x ratio for Size=10000) while memory allocation remained identical. This micro-optimization of dictionary populations inside `Tedd.DictionaryUtils` is positive. Also fixed exception types thrown on null arguments from `ArgumentException` to `ArgumentNullException`.

**Strategic Action:** I will format the code and prepare the submission.
