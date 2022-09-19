# War3Net
## A .NET implementation of Warcraft III related libraries

[![GitHub stars](https://img.shields.io/github/stars/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/network/members)

---

<p align="center">
  •
  <b>
  <a href="#what-is-war3net">What is War3Net?</a> •
  <a href="#projects">Projects</a> •
  <a href="#contributing">Contributing</a> •
  <a href="#license">License</a>
  </b>
  •
</p>

---

### What is War3Net?

War3Net is a collection of libraries for Warcraft III modding.

### Projects

| Project                           | Summary                                                                                                   | NuGet                         |
| --------------------------------- | --------------------------------------------------------------------------------------------------------- | ----------------------------- |
| [War3Net.Build]                   | Generate Wacraft III map script and MPQ archive, by reading from C#/vJass source code and war3map files.  | [![VBuild]][PBuild]           |
| [War3Net.Build.Core]              | Parsers and serializers for war3map files.                                                                | [![VBuildCore]][PBuildCore]   |
| [War3Net.CodeAnalysis]            | Helper methods for Pidgin parsers.                                                                        | [![VCode]][PCode]             |
| [War3Net.CodeAnalysis.CSharp]     | *deprecated*                                                                                              | [![VCodeCSharp]][PCodeCSharp] |
| [War3Net.CodeAnalysis.Decompilers]| Regenerate war3map files from a Warcraft III map script.                                                  | [![VCodeDecomp]][PCodeDecomp] |
| [War3Net.CodeAnalysis.Jass]       | War3Net.CodeAnalysis.Jass is a library for parsing and rendering JASS source files.                       | [![VCodeJass]][PCodeJass]     |
| [War3Net.CodeAnalysis.Transpilers]| Transpiles JASS source code to C# or lua.                                                                 | [![VCodeTrans]][PCodeTrans]   |
| [War3Net.Common]                  | Contains some methods used by several other War3Net projects.                                             | [![VCommon]][PCommon]         |
| [War3Net.Drawing.Blp]             | War3Net.Drawing.Blp is a library for reading files with the ".blp" extension.                             | [![VBlp]][PBlp]               |
| [War3Net.IO.Casc]                 | Class library for opening CASC archives.                                                                  | *Coming soon*                 |
| [War3Net.IO.Compression]          | Decompression and compression algorithms for compression methods commonly used in MPQ archives.           | [![VCompress]][PCompress]     |
| [War3Net.IO.Mpq]                  | Class library for opening and creating MPQ files.                                                         | [![VMpq]][PMpq]               |
| [War3Net.IO.Slk]                  | Library for opening and creating files in SLK format.                                                     | [![VSlk]][PSlk]               |
| [War3Net.Modeling]                | Read and write .mdl and .mdx files.                                                                       | *Coming soon*                 |
| [War3Net.Rendering]               | Renders Warcraft III models using [Veldrid].                                                              | *Coming soon*                 |
| [War3Net.Replay]                  | Parse replay (.w3g) files.                                                                                | *Coming soon*                 |
| [War3Net.Runtime]                 | Uses [NLua] to run JASS and lua map scripts.                                                              | *Coming soon*                 |
| [War3Net.Runtime.Core]            | C# implementation of Warcraft III's backend code.                                                         | *Coming soon*                 |
| [War3Net.Runtime.Api.Blizzard]    | The Blizzard.j API implemented in C#.                                                                     | *Coming soon*                 |
| [War3Net.Runtime.Api.Common]      | API for [War3Net.Runtime.Core], similar to [War3Api.Common].                                              | *Coming soon*                 |

Some of the above projects are based on code from other repositories:
- *[War3Net.Drawing.Blp]:* [SereniaBLPLib](https://github.com/WoW-Tools/SereniaBLPLib)
- *[War3Net.IO.Compression] and [War3Net.IO.Mpq]:* [MpqTool](https://github.com/hazzik/MpqTool)

### Contributing

[![GitHub issues](https://img.shields.io/github/issues/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/pulls)

### License

[![GitHub license](https://img.shields.io/github/license/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/blob/master/LICENSE)

War3Net is licenced under the [MIT](LICENSE) license.
Projects from NuGet packages and submodules may have a different license.





[CSharpLua]: https://github.com/Drake53/CSharp.lua
[NLua]: https://github.com/NLua/NLua
[Veldrid]: https://github.com/mellinoe/veldrid
[War3Api.Blizzard]: https://github.com/Drake53/War3Api/tree/master/src/War3Api.Blizzard
[War3Api.Common]: https://github.com/Drake53/War3Api/tree/master/src/War3Api.Common

[War3Net.Build]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Build
[War3Net.Build.Core]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Build.Core
[War3Net.CodeAnalysis]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis
[War3Net.CodeAnalysis.CSharp]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis.CSharp
[War3Net.CodeAnalysis.Decompilers]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis.Decompilers
[War3Net.CodeAnalysis.Jass]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis.Jass
[War3Net.CodeAnalysis.Transpilers]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis.Transpilers
[War3Net.Common]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Common
[War3Net.Drawing.Blp]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Drawing.Blp
[War3Net.IO.Casc]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.IO.Casc
[War3Net.IO.Compression]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.IO.Compression
[War3Net.IO.Mpq]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.IO.Mpq
[War3Net.IO.Slk]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.IO.Slk
[War3Net.Modeling]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Modeling
[War3Net.Rendering]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Rendering
[War3Net.Replay]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Replay
[War3Net.Runtime]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Runtime
[War3Net.Runtime.Core]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Runtime.Core
[War3Net.Runtime.Api.Blizzard]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Runtime.Api.Blizzard
[War3Net.Runtime.Api.Common]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Runtime.Api.Common

[PBuild]: https://www.nuget.org/packages/War3Net.Build
[PBuildCore]: https://www.nuget.org/packages/War3Net.Build.Core
[PCode]: https://www.nuget.org/packages/War3Net.CodeAnalysis
[PCodeCSharp]: https://www.nuget.org/packages/War3Net.CodeAnalysis.CSharp 
[PCodeDecomp]: https://www.nuget.org/packages/War3Net.CodeAnalysis.Decompilers
[PCodeJass]: https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass
[PCodeTrans]: https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers
[PCommon]: https://www.nuget.org/packages/War3Net.Common
[PBlp]: https://www.nuget.org/packages/War3Net.Drawing.Blp
[PCasc]: https://www.nuget.org/packages/War3Net.IO.Casc
[PCompress]: https://www.nuget.org/packages/War3Net.IO.Compression
[PMpq]: https://www.nuget.org/packages/War3Net.IO.Mpq
[PSlk]: https://www.nuget.org/packages/War3Net.IO.Slk
[PModel]: https://www.nuget.org/packages/War3Net.Modeling
[PRender]: https://www.nuget.org/packages/War3Net.Rendering
[PReplay]: https://www.nuget.org/packages/War3Net.Replay
[PRuntime]: https://www.nuget.org/packages/War3Net.Runtime
[PRuntimeCore]: https://www.nuget.org/packages/War3Net.Runtime.Core
[PBlizzardApi]: https://www.nuget.org/packages/War3Net.Runtime.Api.Blizzard
[PCommonApi]: https://www.nuget.org/packages/War3Net.Runtime.Api.Common

[VBuild]: https://img.shields.io/nuget/v/War3Net.Build.svg
[VBuildCore]: https://img.shields.io/nuget/v/War3Net.Build.Core.svg
[VCode]: https://img.shields.io/nuget/v/War3Net.CodeAnalysis.svg 
[VCodeCSharp]: https://img.shields.io/nuget/v/War3Net.CodeAnalysis.CSharp.svg 
[VCodeDecomp]: https://img.shields.io/nuget/v/War3Net.CodeAnalysis.Decompilers.svg
[VCodeJass]: https://img.shields.io/nuget/v/War3Net.CodeAnalysis.Jass.svg
[VCodeTrans]: https://img.shields.io/nuget/v/War3Net.CodeAnalysis.Transpilers.svg
[VCommon]: https://img.shields.io/nuget/v/War3Net.Common.svg
[VBlp]: https://img.shields.io/nuget/v/War3Net.Drawing.Blp.svg
[VCasc]: https://img.shields.io/nuget/v/War3Net.IO.Casc.svg
[VCompress]: https://img.shields.io/nuget/v/War3Net.IO.Compression.svg
[VMpq]: https://img.shields.io/nuget/v/War3Net.IO.Mpq.svg
[VSlk]: https://img.shields.io/nuget/v/War3Net.IO.Slk.svg
[VModel]: https://img.shields.io/nuget/v/War3Net.Modeling.svg
[VRender]: https://img.shields.io/nuget/v/War3Net.Rendering.svg
[VReplay]: https://img.shields.io/nuget/v/War3Net.Replay.svg
[VRuntime]: https://img.shields.io/nuget/v/War3Net.Runtime.svg
[VRuntimeCore]: https://img.shields.io/nuget/v/War3Net.Runtime.Core.svg
[VBlizzardApi]: https://img.shields.io/nuget/v/War3Net.Runtime.Api.Blizzard.svg
[VCommonApi]: https://img.shields.io/nuget/v/War3Net.Runtime.Api.Common.svg
