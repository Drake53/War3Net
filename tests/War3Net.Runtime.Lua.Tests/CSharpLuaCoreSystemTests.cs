// ------------------------------------------------------------------------------
// <copyright file="CSharpLuaCoreSystemTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;

using CSharpLua.CoreSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.Runtime.Lua.Tests
{
    [TestClass]
    public class CSharpLuaCoreSystemTests
    {
        private static NLua.Lua _luaState;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _luaState = new NLua.Lua();
            _luaState.RemoveNonWhitelistedGlobals();
        }

        [ClassCleanup]
        public static void ClassClean()
        {
            _luaState.Dispose();
        }

        [TestMethod]
        public void TestLoad()
        {
            var mapScript = string.Join("\r\n", CoreSystemProvider.GetCoreSystemFiles().Select(file => $"do\r\n{File.ReadAllText(file)}\r\nend"));
            _luaState.DoString(mapScript, "coresystem");

            const string identifier = "System";
            var systemTable = _luaState.DoString($"return {identifier}").Single();
            Assert.IsTrue(systemTable != null);
        }

        [TestMethod]
        public void TestVersion()
        {
            Assert.AreEqual("Lua 5.3", _luaState["_VERSION"]);
        }
    }
}