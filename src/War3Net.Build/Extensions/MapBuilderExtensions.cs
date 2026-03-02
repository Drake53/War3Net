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