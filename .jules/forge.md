## 2026-07-02 - NuGet Dependency and Framework Modernization

**Observation:**
- The repository was missing modern target frameworks (`net9.0`, `net10.0`) in both `Tedd.DictionaryUtils.csproj` and `Tedd.DictionaryUtils.Tests.csproj`.
- Dependencies were outdated: `Nerdbank.GitVersioning` (3.4.255), `Microsoft.NET.Test.Sdk` (17.1.0), `xunit` (2.4.1), `xunit.runner.visualstudio` (2.4.3), and `coverlet.collector` (3.1.2).
- An old test dependency transitively referenced `Newtonsoft.Json 9.0.1` which had a high-severity vulnerability (NU1903).
- Updating `xunit.runner.visualstudio` to version `3.1.5` introduced a `NU1701` compatibility warning for the preserved `net6.0` test target, necessitating a downgrade to version `2.8.2`.

**Strategic Action:**
- Updated target frameworks for `Tedd.DictionaryUtils.csproj` to `netstandard2.0;net8.0;net9.0;net10.0` and `Tedd.DictionaryUtils.Tests.csproj` to `net6.0;net8.0;net9.0;net10.0`. Preserved older consumer compatibility.
- Modernized dependencies in `Tedd.DictionaryUtils.Tests.csproj` to `Microsoft.NET.Test.Sdk` (18.7.0), `xunit` (2.9.3), `xunit.runner.visualstudio` (2.8.2), and `coverlet.collector` (10.0.1).
- Updated `Nerdbank.GitVersioning` in `Tedd.DictionaryUtils.csproj` to `3.10.85`.
