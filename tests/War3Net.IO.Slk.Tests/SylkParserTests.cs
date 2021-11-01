using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Slk.Tests
{
    [TestClass]
    public class SylkParserTests
    {
        private const string _unitDataPath = @"..\..\..\unitdata.slk";
        private SylkTable _unitData;

        [TestInitialize]
        public void Initialize()
        {
            using var dataFile = File.OpenRead(_unitDataPath);
            _unitData = new SylkParser().Parse(dataFile);
        }

        [TestMethod]
        public void SylkParser_Parse_CorrectWidth()
        {
            Assert.AreEqual(32, _unitData.Width);
        }

        [TestMethod]
        public void SylkParser_Parse_CorrectHeight()
        {
            Assert.AreEqual(865, _unitData.Height);
        }

        [TestMethod]
        public void SylkParser_Parse_CorrectColumns()
        {
            Assert.AreEqual(30, _unitData.Columns);
        }

        [TestMethod]
        public void SylkParser_Parse_CorrectRows()
        {
            Assert.AreEqual(864, _unitData.Rows);
        }

        [TestMethod]
        public void SylkParser_Parse_FirstRowIsCorrect()
        {
            var archmageName = _unitData[0,1];
            Assert.AreEqual("Hamg", archmageName);
        }

        [TestMethod]
        public void SylkParser_Parse_LastRowIsCorrect()
        {
            var wolfriderName = _unitData[0,_unitData.Rows];
            Assert.AreEqual("owad", wolfriderName);
        }
    }
}