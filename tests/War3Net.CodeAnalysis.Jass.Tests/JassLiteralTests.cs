// ------------------------------------------------------------------------------
// <copyright file="JassLiteralTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Jass.Tests
{
    [TestClass]
    public class JassLiteralTests
    {
        [TestMethod]
        [DataRow("'t'", 't')]
        [DataRow("'\\r'", '\r')]
        [DataRow("'\\n'", '\n')]
        [DataRow("'\\t'", '\t')]
        [DataRow("'\\b'", '\b')]
        [DataRow("'\\f'", '\f')]
        [DataRow("'\\\\'", '\\')]
        [DataRow("'\\\"'", '"')]
        [DataRow("'\\''", '\'')]
        public void TestParseChar(string characterLiteral, char expectedValue)
        {
            Assert.AreEqual(expectedValue, JassLiteral.ParseChar(characterLiteral));
        }
    }
}