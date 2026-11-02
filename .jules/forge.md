## 2024-05-18 - Tedd.DictionaryUtils NuGet Modernization

**Observation:** Discovered outdated dependencies in `Tedd.DictionaryUtils` (`Nerdbank.GitVersioning` from 3.4.255 to 3.10.94) and `Tedd.DictionaryUtils.Tests` (transitive vulnerability in `Newtonsoft.Json`, outdated testing dependencies `xunit`, `xunit.runner.visualstudio`, `coverlet.collector`). Also found an incompatible version of `Microsoft.NET.Test.Sdk` (18.11.0) which drops support for `net6.0`, requiring retention of 17.1.0 to preserve testing capabilities across all defined test targets (`net6.0`, `net8.0`, `net10.0`). The test project target frameworks remain untouched.

**Strategic Action:**
1. Updated `Nerdbank.GitVersioning` in `Tedd.DictionaryUtils.csproj` to latest stable 3.10.94.
2. Manually downgraded `Microsoft.NET.Test.Sdk` back to 17.1.0 in `Tedd.DictionaryUtils.Tests.csproj` because version 18.x drops support for `net6.0`, which is one of the test targets. Retaining `net6.0` testing is essential since the main package targets `netstandard2.0` and may run on .NET 6 in consumer applications.
3. Updated `xunit` to 2.9.3, `xunit.runner.visualstudio` to 2.8.2, and `coverlet.collector` to 10.1.0 in `Tedd.DictionaryUtils.Tests.csproj`.
4. Added direct reference to `Newtonsoft.Json` 13.0.3 in `Tedd.DictionaryUtils.Tests.csproj` to resolve transitive vulnerabilities.
5. Successfully restored, built, tested, packed, and formatted the solution with these changes without altering the target frameworks.
