// ------------------------------------------------------------------------------
// <copyright file="LuaScriptCompiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CSharpLua;

namespace War3Net.Build.Script
{
    internal sealed class LuaScriptCompiler : ScriptCompiler
    {
        public LuaScriptCompiler(ScriptCompilerOptions options)
            : base(options)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override ScriptBuilder GetScriptBuilder()
        {
            return new LuaScriptBuilder();
        }

        public override void BuildMainAndConfig(out string mainFunctionFilePath, out string configFunctionFilePath)
        {
            throw new NotImplementedException();
        }

        public override bool Compile(IEnumerable<ContentReference> references, out string scriptFilePath, params string[] additionalSourceFiles)
        {
            throw new NotImplementedException();
        }
    }
}