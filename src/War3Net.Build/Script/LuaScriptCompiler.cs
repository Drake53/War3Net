// ------------------------------------------------------------------------------
// <copyright file="LuaScriptCompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
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

        [Obsolete(null, true)]
        public override ScriptBuilder GetScriptBuilder()
        {
            return new LuaScriptBuilder();
        }

        public override void BuildMainAndConfig(out string mainFunctionFilePath, out string configFunctionFilePath)
        {
            throw new NotImplementedException();
        }

        public override bool Compile(out string scriptFilePath, params string[] additionalSourceFiles)
        {
            throw new NotImplementedException();
        }
    }
}