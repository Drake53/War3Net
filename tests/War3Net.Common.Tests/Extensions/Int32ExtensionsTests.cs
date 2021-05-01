// ------------------------------------------------------------------------------
// <copyright file="Int32ExtensionsTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Common.Extensions;

namespace War3Net.Common.Tests.Extensions
{
    [TestClass]
    public sealed class Int32ExtensionsTests
    {
        [TestMethod]
        public void TestReadRgba()
        {
            const int R = 200;
            const int G = 50;
            const int B = 150;
            const int A = 255;

            var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            using var reader = new BinaryReader(stream);

            writer.Write((A << 24) | (B << 16) | (G << 8) | R);

            stream.Position = 0;
            Assert.AreEqual(R, reader.ReadByte());
            Assert.AreEqual(G, reader.ReadByte());
            Assert.AreEqual(B, reader.ReadByte());
            Assert.AreEqual(A, reader.ReadByte());

            stream.Position = 0;
            var color = reader.ReadInt32().ToRgbaColor();
            Assert.AreEqual(R, color.R);
            Assert.AreEqual(G, color.G);
            Assert.AreEqual(B, color.B);
            Assert.AreEqual(A, color.A);
        }
    }
}