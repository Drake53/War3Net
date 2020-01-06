﻿// ------------------------------------------------------------------------------
// <copyright file="ScriptCompilerOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build.Script
{
    public sealed class ScriptCompilerOptions
    {
        public string SourceDirectory { get; set; }

        public string OutputDirectory { get; set; }

        /// <summary>
        /// If true, script files added through assets will be overwritten, even if <see cref="SourceDirectory"/> is null.
        /// </summary>
        public bool ForceCompile { get; set; }

        public bool Debug { get; set; }

        public bool Optimize { get; set; }

        public bool Obfuscate { get; set; }

        public Dictionary<string, MpqFileFlags> FileFlags { get; private set; }

        public MpqFileFlags DefaultFileFlags { get; set; }

        public MapInfo? MapInfo { get; set; }

        public MapEnvironment? MapEnvironment { get; set; }

        public MapDoodads? MapDoodads { get; set; }

        public MapUnits? MapUnits { get; set; }

        public MapRegions? MapRegions { get; set; }

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