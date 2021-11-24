using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.IO.Slk.Tests
{
    [TestClass]
    public class SylkTableTests
    {
        private const string _unitDataPath = @"..\..\..\unitdata.slk";
        private const string _unitMetadataPath = @"..\..\..\unitui.slk";
        private SylkTable _unitData;
        private SylkTable _unitMetaData;
        private SylkTable _combinedUnitData;
        private SylkTable _shrunkCombinedUnitData;

        [TestInitialize]
        public void Initialize()
        {
            using var dataFile = File.OpenRead(_unitDataPath);
            _unitData = new SylkParser().Parse(dataFile);

            using var metaDataFile = File.OpenRead(_unitMetadataPath);
            _unitMetaData = new SylkParser().Parse(metaDataFile);

            _combinedUnitData = _unitData.Combine(_unitMetaData, "unitID", "unitUIID");

            _shrunkCombinedUnitData = _combinedUnitData.Shrink();
        }

        [TestMethod]
        public void SylkTable_Combine_CorrectWidth()
        {
            Assert.AreEqual(_unitData.Width + _unitMetaData.Width - 1, _combinedUnitData.Width);
        }

        [TestMethod]
        public void SylkTable_Combine_CorrectHeight()
        {
            Assert.AreEqual(_unitData.Height, _combinedUnitData.Height);
        }

        [TestMethod]
        public void SylkTable_Combine_CorrectColumns()
        {
            Assert.AreEqual(_unitData.Columns + _unitMetaData.Columns + 1, _combinedUnitData.Columns);
        }

        [TestMethod]
        public void SylkTable_Combine_CorrectRows()
        {
            Assert.AreEqual(_unitData.Rows, _combinedUnitData.Rows);
        }

        [TestMethod]
        public void SylkTable_Combine_FirstRowIsCorrect()
        {
            var archmageName = _combinedUnitData[0,1];
            Assert.AreEqual("Hamg", archmageName);
        }

        [TestMethod]
        public void SylkTable_Combine_LastRowIsCorrect()
        {
            var wolfriderName = _combinedUnitData[0,_combinedUnitData.Rows];
            Assert.AreEqual("owad", wolfriderName);
        }

        [TestMethod]
        public void SylkTable_Shrink_CorrectWidth()
        {
            Assert.AreEqual(_combinedUnitData.Width, _shrunkCombinedUnitData.Width);
        }

        [TestMethod]
        public void SylkTable_Shrink_CorrectHeight()
        {
            Assert.AreEqual(_unitData.Height, _shrunkCombinedUnitData.Height);
        }

        [TestMethod]
        public void SylkTable_Shrink_CorrectColumns()
        {
            Assert.AreEqual(_combinedUnitData.Columns, _shrunkCombinedUnitData.Columns);
        }

        [TestMethod]
        public void SylkTable_Shrink_CorrectRows()
        {
            Assert.AreEqual(_unitData.Rows, _shrunkCombinedUnitData.Rows);
        }

        [TestMethod]
        public void SylkTable_Shrink_FirstRowIsCorrect()
        {
            var archmageName = _shrunkCombinedUnitData[0,1];
            Assert.AreEqual("Hamg", archmageName);
        }

        [TestMethod]
        public void SylkTable_Shrink_LastRowIsCorrect()
        {
            var wolfriderName = _shrunkCombinedUnitData[0,_shrunkCombinedUnitData.Rows];
            Assert.AreEqual("owad", wolfriderName);
        }
    }
}