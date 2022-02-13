// ------------------------------------------------------------------------------
// <copyright file="MapBuilderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using CSharpLua;

namespace War3Net.Build.Extensions
{
    public static class MapBuilderExtensions
    {
        public static void Compile(this MapBuilder mapBuilder, Compiler compiler, IEnumerable<string> luaSystemLibs)
        {
            mapBuilder.Map.CompileScript(compiler, luaSystemLibs);
        }
    }
}