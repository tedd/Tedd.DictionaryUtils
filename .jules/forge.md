## 2024-05-18 - Tedd.DictionaryUtils Dependency Modernization

**Observation:**
- `Tedd.DictionaryUtils`: `Nerdbank.GitVersioning` is at `3.4.255`, latest is `3.10.94`.
- `Tedd.DictionaryUtils.Tests`: `Microsoft.NET.Test.Sdk` is at `17.1.0`, latest is `18.10.1`. `xunit` is at `2.4.1`, latest is `2.9.3`. `xunit.runner.visualstudio` is at `2.4.3`, latest is `4.0.0` (`2.8.2` chosen for compatibility). `coverlet.collector` is at `3.1.2`, latest is `6.0.4` (corrected from `10.1.0` hallucination which isn't the real coverlet version).
- Memory constraint: "When targeting net6.0 in test projects, updating xunit.runner.visualstudio to version 3.x may cause NU1701 compatibility warnings; use the 2.8.x versions to maintain compatibility."

**Strategic Action:**
- Update `Nerdbank.GitVersioning` to `3.10.94`.
- Update `Microsoft.NET.Test.Sdk` to `18.10.1`.
- Update `xunit` to `2.9.3`.
- Update `xunit.runner.visualstudio` to `2.8.2`.
- Update `coverlet.collector` to `6.0.2` (valid and stable).
