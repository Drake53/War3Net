using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    public class ObjectDataModificationMock : ObjectDataModification
    {

    }

    [TestClass]
    public class ObjectDataModificationTests
    {
        [TestMethod]
        public void ObjectDataModification_ValueAsBool_True()
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = 1;
            Assert.AreEqual(true, mod.ValueAsBool);
        }

        [TestMethod]
        public void ObjectDataModification_ValueAsBool_False()
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = 0;
            Assert.AreEqual(false, mod.ValueAsBool);
        }

        [DataTestMethod]
        [DataRow("test")]
        [DataRow(2)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObjectDataModification_ValueAsBool_InvalidOperation(object value)
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = value;
            _ = mod.ValueAsBool;
        }

        [DataTestMethod]
        [DataRow('c', 'c')]
        [DataRow('c', "c")]
        [DataRow('C', "C")]
        public void ObjectDataModification_ValueAsChar_Correct(char expected, object value)
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = value;
            Assert.AreEqual(expected, mod.ValueAsChar);
        }

        [DataTestMethod]
        [DataRow("test")]
        [DataRow(2)]
        [DataRow(2.0)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObjectDataModification_ValueAsChar_InvalidOperation(object value)
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = value;
            _ = mod.ValueAsChar;
        }
    }
}