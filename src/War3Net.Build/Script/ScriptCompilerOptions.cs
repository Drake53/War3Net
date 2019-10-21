// ------------------------------------------------------------------------------
// <copyright file="ScriptCompilerOptions.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.IO.Mpq;

namespace War3Net.Build.Script
{
    public sealed class ScriptCompilerOptions
    {
        public string SourceDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public bool Debug { get; set; }

        public bool Optimize { get; set; }

        public bool Obfuscate { get; set; }

        public Dictionary<string, MpqFileFlags> FileFlags { get; private set; }

        public MpqFileFlags DefaultFileFlags { get; set; }

        [System.Obsolete]
        public ScriptBuilderOptions BuilderOptions { get; set; }

        public MapInfo MapInfo { get; set; }

        public string LobbyMusic { get; set; }

        internal List<string> Libraries { get; private set; }

        public ScriptCompilerOptions(params string[] libraries)
        {
            FileFlags = new Dictionary<string, MpqFileFlags>();
            DefaultFileFlags = MpqFileFlags.Exists;
            Libraries = new List<string>(libraries);
        }

        public ScriptCompilerOptions(IEnumerable<string> libraries)
        {
            FileFlags = new Dictionary<string, MpqFileFlags>();
            DefaultFileFlags = MpqFileFlags.Exists;
            Libraries = libraries.ToList();
        }
    }
}