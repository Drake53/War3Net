// ------------------------------------------------------------------------------
// <copyright file="SerializationTestHelper.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Extensions;
using War3Net.Build.Serialization.Json;
using War3Net.IO.Mpq;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    internal static class SerializationTestHelper<TFile>
    {
        private static readonly MethodInfo _binaryReadMethod = typeof(BinaryReaderExtensions).GetMethod($"Read{typeof(TFile).Name}", new[] { typeof(BinaryReader) })!;
        private static readonly MethodInfo _binaryWriteMethod = typeof(BinaryWriterExtensions).GetMethod(nameof(BinaryWriter.Write), new[] { typeof(BinaryWriter), typeof(TFile) })!;

        internal static void RunBinaryRWTest(string filePath)
        {
            using var expectedStream = MpqFile.OpenRead(filePath);
            using var reader = new BinaryReader(expectedStream);
            var parsedFile = _binaryReadMethod.Invoke(null, new object?[] { reader });

            using var actualStream = new MemoryStream();
            using var writer = new BinaryWriter(actualStream);
            _binaryWriteMethod.Invoke(null, new object?[] { writer, parsedFile });
            writer.Flush();

            StreamAssert.AreEqual(expectedStream, actualStream, true);
        }

        internal static void RunJsonRWTest(string filePath, bool useStringEnumConverter)
        {
            using var expectedStream = MpqFile.OpenRead(filePath);
            using var reader = new BinaryReader(expectedStream);
            var parsedFile = _binaryReadMethod.Invoke(null, new object?[] { reader });

            var options = GetJsonSerializerOptions(useStringEnumConverter);
            var json = JsonSerializer.Serialize(parsedFile, options);
            var deserializedFile = JsonSerializer.Deserialize<TFile>(json, options);

            using var actualStream = new MemoryStream();
            using var writer = new BinaryWriter(actualStream);
            _binaryWriteMethod.Invoke(null, new object?[] { writer, deserializedFile });
            writer.Flush();

            StreamAssert.AreEqual(expectedStream, actualStream, true, false);
        }

        private static JsonSerializerOptions GetJsonSerializerOptions(bool useStringEnumConverter)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            options.Converters.Add(new JsonStringVersionConverter());
            if (useStringEnumConverter)
            {
                options.Converters.Add(new JsonStringEnumConverter());
            }

            return options;
        }
    }
}