// ------------------------------------------------------------------------------
// <copyright file="SecurityTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Runtime.Lua.Tests
{
    [TestClass]
    public class SecurityTests
    {
        private static NLuaVirtualMachine _vm;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _vm = new NLuaVirtualMachine(null, false);
        }

        [ClassCleanup]
        public static void ClassClean()
        {
            _vm.Dispose();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetLuaStdLibGlobals), DynamicDataSourceType.Method)]
        public void TestStdLibGlobals(string identifier, bool accessible)
        {
            var result = _vm.State.DoString($"return pcall(function() return {identifier} end)");
            if ((bool)result[0])
            {
                Assert.AreEqual(accessible, result[1] != null);
            }
            else
            {
                Assert.IsFalse(accessible);
            }
        }

        private static IEnumerable<object[]> GetLuaStdLibGlobals()
        {
            foreach (var identifier in Sandbox.GetWhitelist())
            {
                yield return new object[] { identifier, true };
            }

            foreach (var table in Sandbox.GetWhitelistTables())
            {
                var tableIdentifier = table.First();
                yield return new object[] { tableIdentifier, true };
                foreach (var identifier in table.Skip(1))
                {
                    yield return new object[] { $"{tableIdentifier}.{identifier}", true };
                }
            }

            yield return new object[] { "dofile", false };
            yield return new object[] { "loadfile", false };
            yield return new object[] { "require", false };

            yield return new object[] { "debug", false };
            yield return new object[] { "debug.debug", false };
            yield return new object[] { "debug.gethook", false };
            yield return new object[] { "debug.getinfo", false };
            yield return new object[] { "debug.getlocal", false };
            yield return new object[] { "debug.getmetatable", false };
            yield return new object[] { "debug.getregistry", false };
            yield return new object[] { "debug.getupvalue", false };
            yield return new object[] { "debug.getuservalue", false };
            yield return new object[] { "debug.sethook", false };
            yield return new object[] { "debug.setlocal", false };
            yield return new object[] { "debug.setmetatable", false };
            yield return new object[] { "debug.setupvalue", false };
            yield return new object[] { "debug.setuservalue", false };
            yield return new object[] { "debug.traceback", false };
            yield return new object[] { "debug.upvalueid", false };
            yield return new object[] { "debug.upvaluejoin", false };

            yield return new object[] { "io", false };
            yield return new object[] { "io.close", false };
            yield return new object[] { "io.flush", false };
            yield return new object[] { "io.input", false };
            yield return new object[] { "io.lines", false };
            yield return new object[] { "io.open", false };
            yield return new object[] { "io.output", false };
            yield return new object[] { "io.popen", false };
            yield return new object[] { "io.read", false };
            yield return new object[] { "io.stderr", false };
            yield return new object[] { "io.stdin", false };
            yield return new object[] { "io.stdout", false };
            yield return new object[] { "io.tmpfile", false };
            yield return new object[] { "io.type", false };
            yield return new object[] { "io.write", false };

            // todo: file
            // file:close
            // file:flush
            // file:lines
            // file:read
            // file:seek
            // file:setvbuf
            // file:write

            yield return new object[] { "os.execute", false };
            yield return new object[] { "os.exit", false };
            yield return new object[] { "os.getenv", false };
            yield return new object[] { "os.remove", false };
            yield return new object[] { "os.rename", false };
            yield return new object[] { "os.setlocale", false };
            yield return new object[] { "os.tmpname", false };

            yield return new object[] { "package", false };
            yield return new object[] { "package.config", false };
            yield return new object[] { "package.cpath", false };
            yield return new object[] { "package.loaded", false };
            yield return new object[] { "package.loadlib", false };
            yield return new object[] { "package.path", false };
            yield return new object[] { "package.preload", false };
            yield return new object[] { "package.searchers", false };
            yield return new object[] { "package.searchpath", false };

            yield return new object[] { "string.dump", false };

            yield return new object[] { "tostringcs", false };
        }
    }
}