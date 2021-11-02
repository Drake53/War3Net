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
        [DataTestMethod]
        [DataRow(true, true)]
        [DataRow(true, 1)]
        [DataRow(false, false)]
        [DataRow(false, 0)]
        public void ObjectDataModification_ValueAsBool_Correct(bool expected, object value)
        {
            var mod = new ObjectDataModificationMock();
            mod.Value = value;
            Assert.AreEqual(expected, mod.ValueAsBool);
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