// ------------------------------------------------------------------------------
// <copyright file="ScriptCompilerOptions.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed class ScriptCompilerOptions
    {
        public string SourceDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public bool Debug { get; set; }

        public bool Optimize { get; set; }

        public bool Obfuscate { get; set; }

        public ScriptBuilderOptions BuilderOptions { get; set; }
    }
}