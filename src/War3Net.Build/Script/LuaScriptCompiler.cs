// ------------------------------------------------------------------------------
// <copyright file="LuaScriptCompiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Script
{
    public sealed class LuaScriptCompiler : ScriptCompiler
    {
        public LuaScriptCompiler(ScriptCompilerOptions options)
            : base(options)
        {
            throw new NotImplementedException();
        }

        public override ScriptBuilder GetScriptBuilder()
        {
            return new LuaScriptBuilder();
        }

        public override bool Compile(params string[] additionalSourceFiles)
        {
            throw new NotImplementedException();
        }
    }
}