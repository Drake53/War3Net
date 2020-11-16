// ------------------------------------------------------------------------------
// <copyright file="ParseTestHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Extensions;
using War3Net.Common.Testing;
using War3Net.IO.Mpq;

namespace War3Net.Build.Core.Tests
{
    internal static class ParseTestHelper
    {
        internal static void RunBinaryRWTest(string filePath, Type type, string? readMethodName = null, params object[] additionalReadParameters)
        {
            using var expectedStream = FileProvider.GetFile(filePath);
            using var reader = new BinaryReader(expectedStream);

            using var actualStream = new MemoryStream();
            using var writer = new BinaryWriter(actualStream);

            var readMethod = typeof(BinaryReaderExtensions).GetMethod(readMethodName ?? $"Read{type.Name}");
            Assert.IsNotNull(readMethod, $"Could not find extension method to read {type.Name}.");

            var writeMethod = typeof(BinaryWriterExtensions).GetMethod("Write", new[] { typeof(BinaryWriter), type });
            Assert.IsNotNull(writeMethod, $"Could not find extension method to write {type.Name}.");

            var parsedFile = readMethod!.Invoke(null, new[] { reader }.Concat(additionalReadParameters).ToArray());
            var recreatedFile = writeMethod!.Invoke(null, new[] { writer, parsedFile });

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }

        internal static void RunStreamRWTest(string filePath, Type type, string? readMethodName = null, params object[] additionalReadParameters)
        {
            using var expectedStream = FileProvider.GetFile(filePath);
            using var reader = new StreamReader(expectedStream);

            using var actualStream = new MemoryStream();
            using var writer = new StreamWriter(actualStream);

            var readMethod = typeof(StreamReaderExtensions).GetMethod(readMethodName ?? $"Read{type.Name}");
            Assert.IsNotNull(readMethod, $"Could not find extension method to read {type.Name}.");

            var writeMethod = typeof(StreamWriterExtensions).GetMethod("Write", new[] { typeof(StreamWriter), type });
            Assert.IsNotNull(writeMethod, $"Could not find extension method to write {type.Name}.");

            var parsedFile = readMethod!.Invoke(null, new[] { reader }.Concat(additionalReadParameters).ToArray());
            var recreatedFile = writeMethod!.Invoke(null, new[] { writer, parsedFile });

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }
    }
}