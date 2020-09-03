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

        [TestMethod]
        public void TestLoad()
        {
            var mapScript = string.Join("\r\n", CoreSystemProvider.GetCoreSystemFiles().Select(file => $"do\r\n{File.ReadAllText(file)}\r\nend"));
            _vm.State.DoString(mapScript, "coresystem");

            const string identifier = "System";
            var systemTable = _vm.State.DoString($"return {identifier}").Single();
            Assert.IsTrue(systemTable != null);
        }

        [TestMethod]
        public void TestVersion()
        {
            Assert.AreEqual("Lua 5.3", _vm.State["_VERSION"]);
        }
    }
}