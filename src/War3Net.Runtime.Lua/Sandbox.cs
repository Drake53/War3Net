// ------------------------------------------------------------------------------
// <copyright file="Sandbox.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NLua;

namespace War3Net.Runtime.Lua
{
    public static class Sandbox
    {
        public static void RemoveNonWhitelistedGlobals(this NLua.Lua luaState)
        {
            var globalsTable = (LuaTable)luaState["_G"];

            var whitelist = GetWhitelist().ToHashSet();
            var whitelistTables = GetWhitelistTables().ToDictionary(table => table.First(), table => table.Skip(1).ToHashSet());

            foreach (string key in globalsTable.Keys)
            {
                if (whitelist.Contains(key))
                {
                    whitelist.Remove(key);
                }
                else if (whitelistTables.ContainsKey(key))
                {
                    foreach (string tableKey in ((LuaTable)luaState[key]).Keys)
                    {
                        if (whitelistTables[key].Contains(tableKey))
                        {
                            whitelistTables[key].Remove(tableKey);
                        }
                        else
                        {
                            luaState[$"{key}.{tableKey}"] = null;
                        }
                    }

                    if (whitelistTables[key].Any())
                    {
                        throw new Exception("Table whitelist contains invalid members.");
                    }

                    whitelistTables.Remove(key);
                }
                else
                {
                    luaState[key] = null;
                }
            }

            if (whitelist.Any() || whitelistTables.Any())
            {
                throw new Exception("Whitelist contains invalid members");
            }
        }

        [Obsolete]
        public static void SetSandboxEnvironment(this NLua.Lua luaState)
        {
            var sb = $@"{{
{string.Join("\r\n", GetWhitelist().Select(identifier => $"  {identifier} = {identifier},"))}
{string.Join("\r\n", GetWhitelistTables().Select(table => $"  {table.First()} = {{ {string.Join(", ", table.Skip(1).Select(identifier => $"{identifier} = {table.First()}.{identifier}"))} }},"))}
}}";
        }

        internal static IEnumerable<string> GetWhitelist()
        {
            yield return "_G";
            yield return "_VERSION";
            yield return "assert";
            yield return "collectgarbage";
            yield return "error";
            yield return "getmetatable";
            yield return "ipairs";
            yield return "load";
            yield return "next";
            yield return "pairs";
            yield return "pcall";
            yield return "print";
            yield return "rawequal";
            yield return "rawget";
            yield return "rawlen";
            yield return "rawset";
            yield return "select";
            yield return "setmetatable";
            yield return "tonumber";
            yield return "tostring";
            yield return "type";
            yield return "xpcall";
        }

        internal static IEnumerable<string[]> GetWhitelistTables()
        {
            yield return new[]
            {
                "coroutine",

                "create",
                "isyieldable",
                "resume",
                "running",
                "status",
                "wrap",
                "yield",
            };

            yield return new[]
            {
                "math",

                "abs",
                "acos",
                "asin",
                "atan",
                "ceil",
                "cos",
                "deg",
                "exp",
                "floor",
                "fmod",
                "huge",
                "log",
                "max",
                "maxinteger",
                "min",
                "mininteger",
                "modf",
                "pi",
                "rad",
                "random",
                "randomseed",
                "sin",
                "sqrt",
                "tan",
                "tointeger",
                "type",
                "ult",
            };

            yield return new[]
            {
                "os",

                "clock",
                "date",
                "difftime",
                "time",
            };

            yield return new[]
            {
                "string",

                "byte",
                "char",
                "find",
                "format",
                "gmatch",
                "gsub",
                "len",
                "lower",
                "match",
                "pack",
                "packsize",
                "rep",
                "reverse",
                "sub",
                "unpack",
                "upper",
            };

            yield return new[]
            {
                "table",

                "concat",
                "insert",
                "move",
                "pack",
                "remove",
                "sort",
                "unpack",
            };

            yield return new[]
            {
                "utf8",

                "char",
                "charpattern",
                "codepoint",
                "codes",
                "len",
                "offset",
            };
        }
    }
}