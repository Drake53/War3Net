# War3Net Modding Library
## The complete .NET toolkit for Warcraft III modding

[![GitHub stars](https://img.shields.io/github/stars/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/network/members)
[![GitHub issues](https://img.shields.io/github/issues/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/pulls)
[![GitHub license](https://img.shields.io/github/license/Drake53/War3Net.svg)](https://github.com/Drake53/War3Net/blob/master/LICENSE)
[![Latest release](https://img.shields.io/github/v/release/Drake53/War3Net)](https://github.com/Drake53/War3Net/releases)

---

<p align="center">
  <img src="assets/images/war3net-banner.png" alt="War3Net Banner">
</p>

<p align="center">
  •
  <b>
  <a href="#what-is-war3net">What is War3Net?</a> •
  <a href="#vision--principles">Vision</a> •
  <a href="#components">Components</a> •
  <a href="#packages">Packages</a> •
  <a href="#command-line-tool">Command-Line Tool</a> •
  <a href="#vscode-extension">VS Code Extension</a> •
  <a href="#built-with-war3net">Built with War3Net</a> •
  <a href="#roadmap">Roadmap</a> •
  <a href="#contributing">Contributing</a> •
  <a href="#license">License</a>
  </b>
  •
</p>

---

### What is War3Net?

Your one-stop .NET toolkit for all things Warcraft III. From low-level file formats like MPQ and BLP to high-level map building in C# and working with JASS scripts—War3Net has you covered.

Stop reinventing the wheel. Start building.

### Vision & Principles

War3Net aims to be the definitive .NET foundation for Warcraft III tooling—built on three core principles:

- **Completeness** — Every file format. Every game version. RoC through Reforged.
- **Correctness** — Byte-for-byte accuracy with Blizzard's implementations. No guesswork.
- **Compatibility** — .NET 6+ for broad OS support, including Windows 7/8.

### Components

Three major components are included in War3Net:

- **[NuGet packages](#packages)** — A collection of NuGet packages, each covering a Warcraft III file format or a related concern (MPQ, BLP, JASS, map building, and more). These are the foundation everything else is built on.
- **[Command-line tool](#command-line-tool)** — `w3n`, a single CLI that exposes the modules' functionality for scripting, CI, or terminal use.
- **[VS Code extension](#vscode-extension)** — War3Net Toolkit, which brings those same file formats into the editor, built on top of the CLI.

### Command-Line Tool

*Coming soon™*

### VS Code Extension

*Coming soon™*

### Packages

Main packages:

| Package | Description | Downloads |
| ------- | ----------- | --------- |
| **[War3Net.Build]** | Build complete Warcraft III maps programmatically, including script generation and asset packaging. | [![War3Net.Build downloads](https://img.shields.io/nuget/dt/War3Net.Build.svg)](https://www.nuget.org/packages/War3Net.Build) |
| **[War3Net.CodeAnalysis.Jass]** | Work with JASS code: parse existing scripts, transpile to C#/Lua, or generate new code. | [![War3Net.CodeAnalysis.Jass downloads](https://img.shields.io/nuget/dt/War3Net.CodeAnalysis.Jass.svg)](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) |

Full list of current and planned packages:

| Package                            | Summary                                                          | NuGet                         |
| ---------------------------------- | ---------------------------------------------------------------- | ----------------------------- |
| [War3Net.Build]                    | Generate map scripts and MPQ archives from C#/vJass source code. | [![VBuild]][PBuild]           |
| [War3Net.Build.Core]               | Parsers and serializers for war3map files.                       | [![VBuildCore]][PBuildCore]   |
| [War3Net.CodeAnalysis]             | Helper methods for Pidgin parsers.                               | [![VCode]][PCode]             |
| [War3Net.CodeAnalysis.Decompilers] | Regenerate war3map files from a Warcraft III map script.         | [![VCodeDecomp]][PCodeDecomp] |
| [War3Net.CodeAnalysis.Jass]        | Parse and render JASS source files.                              | [![VCodeJass]][PCodeJass]     |
| [War3Net.CodeAnalysis.Transpilers] | Transpile JASS to C# or Lua.                                     | [![VCodeTrans]][PCodeTrans]   |
| [War3Net.Common]                   | Shared utilities for War3Net packages.                           | [![VCommon]][PCommon]         |
| [War3Net.Drawing.Blp]              | Read and write BLP texture files.                                | [![VBlp]][PBlp]               |
| [War3Net.IO.Casc]                  | Read CASC archives.                                              | *Coming soon™*                |
| [War3Net.IO.Compression]           | Compression algorithms for MPQ archives.                         | [![VCompress]][PCompress]     |
| [War3Net.IO.Mpq]                   | Read and write MPQ archives.                                     | [![VMpq]][PMpq]               |
| [War3Net.IO.Slk]                   | Read and write SLK data files.                                   | [![VSlk]][PSlk]               |
| [War3Net.Modeling]                 | Read and write .mdl and .mdx files.                              | *Coming soon™*                |
| [War3Net.Rendering]                | Render Warcraft III models using [Veldrid].                      | *Coming soon™*                |
| [War3Net.Replay]                   | Parse replay (.w3g) files.                                       | *Coming soon™*                |
| [War3Net.Runtime]                  | Execute JASS and Lua map scripts using [NLua].                   | *Coming soon™*                |
| [War3Net.Runtime.Api.Blizzard]     | Blizzard.j API implemented in C#.                                | *Coming soon™*                |
| [War3Net.Runtime.Api.Common]       | API for [War3Net.Runtime.Core], similar to [War3Api.Common].     | *Coming soon™*                |
| [War3Net.Runtime.Core]             | C# implementation of Warcraft III's backend code.                | *Coming soon™*                |

Some of the above packages are based on code from other repositories:
- *[War3Net.Drawing.Blp]:* [SereniaBLPLib](https://github.com/WoW-Tools/SereniaBLPLib)
- *[War3Net.IO.Compression] and [War3Net.IO.Mpq]:* [MpqTool](https://github.com/hazzik/MpqTool)

### Built with War3Net

> **Disclaimer:** Projects listed here are independent and not affiliated with, endorsed by, or maintained by the creator of War3Net. Use them at your own discretion.

| Project | Description |
| ------- | ----------- |
| [Maya WarTex](https://github.com/wiselencave/maya-wartex) | Autodesk Maya plug-in for BLP texture loading via [War3Net.Drawing.Blp](https://github.com/Drake53/War3Net/tree/master/src/War3Net.Drawing.Blp). |

*Using War3Net in your project? Open a PR to add it here!*

### Roadmap

Three major initiatives are in progress:

**War3Net CLI** — Exposing War3Net's functionality to users outside of the .NET ecosystem.
- `w3n` CLI exposing commands for MPQ, BLP, MDX, JASS, and more
- End goals: Full War3Net functionality covered by the CLI • VSCode extension

**JASS Language Suite** — Bringing modern IDE tooling to JASS.
- Fault-tolerant parsing, semantic analysis, LSP support, and a debugger
- End goals: Intelligent script adaptation across game versions • Integration in the CLI and extension

**Warcraft III Runtime** — Run maps without the game.
- Native JASS VM, C# natives, headless rendering
- End goals: Automated C# map testing • Full game emulation

### Contributing

Please open an issue before creating a pull request.

Pull requests should be small and atomic to make them easier to review.
Avoid doing multiple things in a single pull request like fixing bugs, adding new features, and refactoring code.

### License

War3Net is licensed under the [MIT](LICENSE) license.
Projects from NuGet packages and submodules may have a different license.





[CSharpLua]: https://github.com/Drake53/CSharp.lua
[NLua]: https://github.com/NLua/NLua
[Veldrid]: https://github.com/mellinoe/veldrid
[War3Api.Blizzard]: https://github.com/Drake53/War3Api/tree/master/src/War3Api.Blizzard
[War3Api.Common]: https://github.com/Drake53/War3Api/tree/master/src/War3Api.Common

[War3Net.Build]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Build
[War3Net.Build.Core]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.Build.Core
[War3Net.CodeAnalysis]: https://github.com/Drake53/War3Net/tree/master/src/War3Net.CodeAnalysis
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
