## 2024-05-20 - Target Framework and Dependency Modernization

**Observation:** Tedd.DictionaryUtils targets `netstandard2.0` only. Test and Benchmark projects target `net6.0`, which is out of support. Dependencies like Nerdbank.GitVersioning and testing frameworks (xunit, coverlet, etc.) are outdated.

**Strategic Action:** Multi-target main package to `netstandard2.0;net8.0;net9.0` to preserve compatibility while providing modern baselines. Update Test and Benchmark projects to target `net8.0;net9.0`. Update outdated NuGet packages to their latest stable versions. Validate using `dotnet pack` and format checking.
