// ------------------------------------------------------------------------------
// <copyright file="ParseTestHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Common.Providers;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    internal static class ParseTestHelper
    {
        internal static void RunBinaryRWTest(string filePath, Type type)
        {
            var expectedReadTypes = new[] { typeof(BinaryReader) };
            var readMethod = typeof(BinaryReaderExtensions).GetMethod($"Read{type.Name}", expectedReadTypes);
            Assert.IsNotNull(readMethod, $"Could not find extension method to read {type.Name}.");

            var expectedWriteTypes = new[] { typeof(BinaryWriter), type };
            var writeMethod = typeof(BinaryWriterExtensions).GetMethod(nameof(BinaryWriter.Write), expectedWriteTypes);
            Assert.IsNotNull(writeMethod, $"Could not find extension method to write {type.Name}.");

            using var expectedStream = MpqFile.OpenRead(filePath);
            using var reader = new BinaryReader(expectedStream);
            var parsedFile = readMethod!.Invoke(null, new object?[] { reader });
            Assert.AreEqual(type, parsedFile!.GetType());

            using var actualStream = new MemoryStream();
            using var writer = new BinaryWriter(actualStream);
            writeMethod!.Invoke(null, new object?[] { writer, parsedFile });
            writer.Flush();

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }

        internal static void RunStreamRWTest(
            string filePath,
            Type type,
            string writeMethodName)
        {
            var readMethod = typeof(StreamReaderExtensions).GetMethod($"Read{type.Name}");
            Assert.IsNotNull(readMethod, $"Could not find extension method to read {type.Name}.");

            var writeMethod = typeof(StreamWriterExtensions).GetMethod(writeMethodName, new[] { typeof(StreamWriter), type });
            Assert.IsNotNull(writeMethod, $"Could not find extension method to write {type.Name}.");

            using var expectedStream = MpqFile.OpenRead(filePath);
            using var reader = new StreamReader(expectedStream, UTF8EncodingProvider.StrictUTF8);
            var parsedFile = readMethod!.Invoke(null, new[] { reader });
            Assert.AreEqual(type, parsedFile!.GetType());

            using var actualStream = new MemoryStream();
            using var writer = new StreamWriter(actualStream, reader.CurrentEncoding);
            writeMethod!.Invoke(null, new[] { writer, parsedFile });
            writer.Flush();

            StreamAssert.AreEqualText(expectedStream, actualStream, true);
        }
    }
}