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