## 2024-05-24 - Dependency and Framework Modernization

**Observation:**
- `Tedd.DictionaryUtils`: `Nerdbank.GitVersioning` is at version 3.4.255 (latest is 3.10.94). `LangVersion` is 10.0. Missing `net9.0` and `net10.0` target frameworks.
- `Tedd.DictionaryUtils.Tests`: Dependencies (`Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `coverlet.collector`) are outdated. `xunit.runner.visualstudio` 3.x+ is known to cause `NU1701` on `net6.0`. Transitive vulnerability in tests (NU1903) due to outdated `Newtonsoft.Json` (indirect via old xunit). Missing `net9.0` target framework.
- `Tedd.DictionaryUtils.Benchmark` and `Tedd.DictionaryUtils.Archive`: `LangVersion` is 10.0. Missing `net9.0` and `net10.0` target frameworks.

**Strategic Action:**
- Updated target frameworks across projects to include `net9.0` and `net10.0` to ensure proper multi-targeting compatibility and testing.
- Updated `Nerdbank.GitVersioning` to 3.10.94 in `Tedd.DictionaryUtils`.
- Updated `LangVersion` to `latest` in all projects.
- Updated `Microsoft.NET.Test.Sdk` to 17.13.0.
- Updated `xunit` to 2.9.3.
- Updated `xunit.runner.visualstudio` to 2.8.2.
- Updated `coverlet.collector` to 6.0.4.
