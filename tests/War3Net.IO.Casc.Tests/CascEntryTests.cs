// ------------------------------------------------------------------------------
// <copyright file="CascEntryTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc;
using War3Net.IO.Casc.Enums;
using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class CascEntryTests
    {
        [TestMethod]
        public void TestCascEntryCreation()
        {
            var entry = new CascEntry("test.txt");

            Assert.AreEqual("test.txt", entry.FileName);
            Assert.IsTrue(entry.CKey.IsEmpty);
            Assert.IsTrue(entry.EKey.IsEmpty);
            Assert.AreEqual(CascConstants.InvalidId, entry.FileDataId);
            Assert.AreEqual(CascLocaleFlags.None, entry.LocaleFlags);
            Assert.AreEqual(CascContentFlags.None, entry.ContentFlags);
        }

        [TestMethod]
        public void TestCascEntryProperties()
        {
            var entry = new CascEntry("file.dat")
            {
                CKey = CascKey.Parse("0123456789ABCDEF0123456789ABCDEF"),
                EKey = EKey.Parse("FEDCBA9876543210"),
                FileSize = 1024,
                CompressedSize = 512,
                FileDataId = 12345,
                LocaleFlags = CascLocaleFlags.EnUS | CascLocaleFlags.EnGB,
                ContentFlags = CascContentFlags.Encrypted | CascContentFlags.NoCompression,
                TagBitMask = 0x123456789ABCDEF0,
                IsAvailable = true,
                NameType = CascNameType.Full,
            };

            Assert.IsFalse(entry.CKey.IsEmpty);
            Assert.IsFalse(entry.EKey.IsEmpty);
            Assert.AreEqual(1024ul, entry.FileSize);
            Assert.AreEqual(512ul, entry.CompressedSize);
            Assert.AreEqual(12345u, entry.FileDataId);
            Assert.IsTrue(entry.LocaleFlags.HasFlag(CascLocaleFlags.EnUS));
            Assert.IsTrue(entry.ContentFlags.HasFlag(CascContentFlags.Encrypted));
            Assert.AreEqual(0x123456789ABCDEF0ul, entry.TagBitMask);
            Assert.IsTrue(entry.IsAvailable);
            Assert.AreEqual(CascNameType.Full, entry.NameType);
        }

        [TestMethod]
        public void TestCascEntryIsEncrypted()
        {
            var entry = new CascEntry("encrypted.dat");
            Assert.IsFalse(entry.IsEncrypted);

            entry.ContentFlags = CascContentFlags.Encrypted;
            Assert.IsTrue(entry.IsEncrypted);

            entry.ContentFlags = CascContentFlags.Encrypted | CascContentFlags.NoCompression;
            Assert.IsTrue(entry.IsEncrypted);

            entry.ContentFlags = CascContentFlags.NoCompression;
            Assert.IsFalse(entry.IsEncrypted);
        }

        [TestMethod]
        public void TestCascEntryIsCompressed()
        {
            var entry = new CascEntry("compressed.dat");
            Assert.IsTrue(entry.IsCompressed); // Default is compressed

            entry.ContentFlags = CascContentFlags.NoCompression;
            Assert.IsFalse(entry.IsCompressed);

            entry.ContentFlags = CascContentFlags.None;
            Assert.IsTrue(entry.IsCompressed);
        }

        [TestMethod]
        public void TestCascEntryToString()
        {
            var entry = new CascEntry("test/file.txt");
            Assert.AreEqual("test/file.txt", entry.ToString());
        }

        [TestMethod]
        public void TestCascEntryMultipleLocales()
        {
            var entry = new CascEntry("localized.txt")
            {
                LocaleFlags = CascLocaleFlags.EnUS | CascLocaleFlags.DeDE | CascLocaleFlags.FrFR,
            };

            Assert.IsTrue(entry.LocaleFlags.HasFlag(CascLocaleFlags.EnUS));
            Assert.IsTrue(entry.LocaleFlags.HasFlag(CascLocaleFlags.DeDE));
            Assert.IsTrue(entry.LocaleFlags.HasFlag(CascLocaleFlags.FrFR));
            Assert.IsFalse(entry.LocaleFlags.HasFlag(CascLocaleFlags.ZhCN));
        }

        [TestMethod]
        public void TestCascEntryMultipleContentFlags()
        {
            var entry = new CascEntry("complex.dat")
            {
                ContentFlags = CascContentFlags.Install |
                               CascContentFlags.LoadOnWindows |
                               CascContentFlags.X86_64,
            };

            Assert.IsTrue(entry.ContentFlags.HasFlag(CascContentFlags.Install));
            Assert.IsTrue(entry.ContentFlags.HasFlag(CascContentFlags.LoadOnWindows));
            Assert.IsTrue(entry.ContentFlags.HasFlag(CascContentFlags.X86_64));
            Assert.IsFalse(entry.ContentFlags.HasFlag(CascContentFlags.LoadOnMac));
            Assert.IsFalse(entry.ContentFlags.HasFlag(CascContentFlags.X86_32));
        }

        [TestMethod]
        public void TestCascEntryNameTypes()
        {
            var entry1 = new CascEntry("normal/file.txt")
            {
                NameType = CascNameType.Full,
            };
            Assert.AreEqual(CascNameType.Full, entry1.NameType);

            var entry2 = new CascEntry("FILE00001234.dat")
            {
                NameType = CascNameType.DataId,
                FileDataId = 0x1234,
            };
            Assert.AreEqual(CascNameType.DataId, entry2.NameType);
            Assert.AreEqual(0x1234u, entry2.FileDataId);

            var entry3 = new CascEntry("0123456789ABCDEF0123456789ABCDEF")
            {
                NameType = CascNameType.CKey,
                CKey = CascKey.Parse("0123456789ABCDEF0123456789ABCDEF"),
            };
            Assert.AreEqual(CascNameType.CKey, entry3.NameType);
            Assert.IsFalse(entry3.CKey.IsEmpty);

            var entry4 = new CascEntry("FEDCBA9876543210")
            {
                NameType = CascNameType.EKey,
                EKey = EKey.Parse("FEDCBA9876543210"),
            };
            Assert.AreEqual(CascNameType.EKey, entry4.NameType);
            Assert.IsFalse(entry4.EKey.IsEmpty);
        }
    }
}