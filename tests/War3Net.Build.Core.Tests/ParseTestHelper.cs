// ------------------------------------------------------------------------------
// <copyright file="ParseTestHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    internal static class ParseTestHelper
    {
        internal static void RunBinaryRWTest(
            string filePath,
            Type type,
            string? readMethodName = null,
            params object[]? additionalReadParameters)
        {
            RunBinaryRWTest(filePath, type, readMethodName, additionalReadParameters, null);
        }

        internal static void RunBinaryRWTest(
            string filePath,
            Type type,
            string? readMethodName = null,
            object[]? additionalReadParameters = null,
            object[]? additionalWriteParameters = null)
        {
            var expectedReadTypes = new[] { typeof(BinaryReader) };
            if (additionalReadParameters is not null)
            {
                expectedReadTypes = expectedReadTypes.Concat(additionalReadParameters.Select(p => p.GetType())).ToArray();
            }

            var readMethod = typeof(BinaryReaderExtensions).GetMethod(readMethodName ?? $"Read{type.Name}", expectedReadTypes);
            Assert.IsNotNull(readMethod, $"Could not find extension method to read {type.Name}.");

            var expectedWriteTypes = new[] { typeof(BinaryWriter), type };
            if (additionalWriteParameters is not null)
            {
                expectedWriteTypes = expectedWriteTypes.Concat(additionalWriteParameters.Select(p => p.GetType())).ToArray();
            }

            var writeMethod = typeof(BinaryWriterExtensions).GetMethod(nameof(BinaryWriter.Write), expectedWriteTypes);
            Assert.IsNotNull(writeMethod, $"Could not find extension method to write {type.Name}.");

            using var expectedStream = MpqFile.OpenRead(filePath);
            using var reader = new BinaryReader(expectedStream);
            var parsedFile = readMethod!.Invoke(null, new object?[] { reader }.Concat(additionalReadParameters ?? Array.Empty<object?[]>()).ToArray());
            Assert.AreEqual(type, parsedFile!.GetType());

            using var actualStream = new MemoryStream();
            using var writer = new BinaryWriter(actualStream);
            writeMethod!.Invoke(null, new object?[] { writer, parsedFile }.Concat(additionalWriteParameters ?? Array.Empty<object?[]>()).ToArray());
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
            using var reader = new StreamReader(expectedStream, new UTF8Encoding(false, true));
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