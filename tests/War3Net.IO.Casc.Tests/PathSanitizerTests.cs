// ------------------------------------------------------------------------------
// <copyright file="PathSanitizerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.IO.Casc.Utilities;

namespace War3Net.IO.Casc.Tests
{
    [TestClass]
    public class PathSanitizerTests
    {
        [TestMethod]
        public void TestSanitizeFilePath_ValidPaths()
        {
            // Simple valid paths
            Assert.AreEqual("test.txt", PathSanitizer.SanitizeFilePath("test.txt"));
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeFilePath("folder/file.txt"));
            Assert.AreEqual("folder/subfolder/file.txt", PathSanitizer.SanitizeFilePath("folder/subfolder/file.txt"));

            // Windows-style paths should be normalized
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeFilePath("folder\\file.txt"));
            Assert.AreEqual("a/b/c/file.txt", PathSanitizer.SanitizeFilePath("a\\b\\c\\file.txt"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_EmptyPath()
        {
            PathSanitizer.SanitizeFilePath(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_NullPath()
        {
            PathSanitizer.SanitizeFilePath(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_WhitespacePath()
        {
            PathSanitizer.SanitizeFilePath("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_DirectoryTraversal()
        {
            PathSanitizer.SanitizeFilePath("../file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_DirectoryTraversalWindows()
        {
            PathSanitizer.SanitizeFilePath("..\\file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_DirectoryTraversalEmbedded()
        {
            PathSanitizer.SanitizeFilePath("folder/../file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_DirectoryTraversalDouble()
        {
            PathSanitizer.SanitizeFilePath("../../file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_NullByte()
        {
            PathSanitizer.SanitizeFilePath("file\0.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_AbsolutePathWindows()
        {
            PathSanitizer.SanitizeFilePath("C:\\file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_AbsolutePathUnix()
        {
            PathSanitizer.SanitizeFilePath("/etc/passwd");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNameCON()
        {
            PathSanitizer.SanitizeFilePath("CON");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNamePRN()
        {
            PathSanitizer.SanitizeFilePath("folder/PRN.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNameCOM1()
        {
            PathSanitizer.SanitizeFilePath("COM1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNameLPT1()
        {
            PathSanitizer.SanitizeFilePath("LPT1.log");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNameNUL()
        {
            PathSanitizer.SanitizeFilePath("NUL.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ReservedNameAUX()
        {
            PathSanitizer.SanitizeFilePath("aux");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_EncodedTraversal()
        {
            PathSanitizer.SanitizeFilePath("%2e%2e/file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_DoubleEncodedTraversal()
        {
            PathSanitizer.SanitizeFilePath("%252e%252e/file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_HomePath()
        {
            PathSanitizer.SanitizeFilePath("~/file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_EnvironmentVariable()
        {
            PathSanitizer.SanitizeFilePath("$HOME/file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_WindowsEnvironmentVariable()
        {
            PathSanitizer.SanitizeFilePath("%USERPROFILE%/file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ColonInPath()
        {
            PathSanitizer.SanitizeFilePath("file:stream.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ControlCharacter()
        {
            PathSanitizer.SanitizeFilePath("file\x01.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ZeroWidthJoiner()
        {
            PathSanitizer.SanitizeFilePath("file\u200Dname.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_RightToLeftOverride()
        {
            PathSanitizer.SanitizeFilePath("file\u202Ename.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_ByteOrderMark()
        {
            PathSanitizer.SanitizeFilePath("file\uFEFFname.txt");
        }

        [TestMethod]
        public void TestSanitizeCdnPath_ValidPaths()
        {
            Assert.AreEqual("test.txt", PathSanitizer.SanitizeCdnPath("test.txt"));
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeCdnPath("folder/file.txt"));
            Assert.AreEqual("data/001/abc123", PathSanitizer.SanitizeCdnPath("data/001/abc123"));

            // Should normalize slashes
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeCdnPath("folder\\file.txt"));

            // Should remove leading slashes
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeCdnPath("/folder/file.txt"));

            // Should normalize double slashes
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeCdnPath("folder//file.txt"));

            // Should trim whitespace
            Assert.AreEqual("folder/file.txt", PathSanitizer.SanitizeCdnPath("  folder/file.txt  "));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_Empty()
        {
            PathSanitizer.SanitizeCdnPath(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_Null()
        {
            PathSanitizer.SanitizeCdnPath(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_DirectoryTraversal()
        {
            PathSanitizer.SanitizeCdnPath("../file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_DirectoryTraversalEmbedded()
        {
            PathSanitizer.SanitizeCdnPath("folder/../file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_DirectoryTraversalWindows()
        {
            PathSanitizer.SanitizeCdnPath("..\\file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_EncodedTraversal()
        {
            PathSanitizer.SanitizeCdnPath("%2e%2e%2ffile.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_NullByte()
        {
            PathSanitizer.SanitizeCdnPath("file\0.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_AbsolutePath()
        {
            PathSanitizer.SanitizeCdnPath("C:\\file.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeCdnPath_ColonInPath()
        {
            PathSanitizer.SanitizeCdnPath("file:stream.txt");
        }

        [TestMethod]
        public void TestIsPathSafe_ValidPath()
        {
            Assert.IsTrue(PathSanitizer.IsPathSafe("folder/file.txt"));
            Assert.IsTrue(PathSanitizer.IsPathSafe("test.txt"));
            Assert.IsTrue(PathSanitizer.IsPathSafe("a/b/c/d/e/file.txt"));
        }

        [TestMethod]
        [DataRow("../file.txt")]
        [DataRow("CON")]
        [DataRow("file\0.txt")]
        [DataRow("/etc/passwd")]
        [DataRow("C:\\Windows\\System32")]
        [DataRow("~/file.txt")]
        [DataRow("")]
        [DataRow(null)]
        public void TestIsPathSafe_InvalidPaths(string path)
        {
            Assert.IsFalse(PathSanitizer.IsPathSafe(path));
        }

        [TestMethod]
        public void TestSanitizeFilePath_UnicodeNormalization()
        {
            // Test that different Unicode representations are normalized
            // These represent the same character but with different Unicode compositions
            var nfc = "é"; // NFC: single character U+00E9
            var nfd = "é"; // NFD: e + combining acute accent

            // Both should normalize to the same result
            var result1 = PathSanitizer.SanitizeFilePath($"file_{nfc}.txt");
            var result2 = PathSanitizer.SanitizeFilePath($"file_{nfd}.txt");

            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void TestSanitizeFilePath_ComplexValidPath()
        {
            // Test a complex but valid path
            var path = "Assets/Models/Characters/Player_Model_v2.1.fbx";
            var result = PathSanitizer.SanitizeFilePath(path);
            Assert.AreEqual(path, result);
        }

        [TestMethod]
        public void TestSanitizeFilePath_RemovesEmptySegments()
        {
            // Multiple slashes should be normalized
            var result = PathSanitizer.SanitizeFilePath("folder//subfolder///file.txt");
            Assert.AreEqual("folder/subfolder/file.txt", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_InvalidFileNameCharacters()
        {
            // Test with invalid filename characters
            PathSanitizer.SanitizeFilePath("file<name>.txt");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSanitizeFilePath_MalformedUrlEncoding()
        {
            // Test with malformed URL encoding
            PathSanitizer.SanitizeFilePath("file%ZZname.txt");
        }
    }
}