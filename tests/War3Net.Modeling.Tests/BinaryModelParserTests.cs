namespace War3Net.Modeling.Tests
{
    [TestClass]
    public sealed class BinaryModelParserTests
    {
        // Minimal MDX with a VERS chunk (format version 1200) and a LITE chunk that includes
        // the shadow intensity field introduced in format version 1200 (Reforged 2.0).
        // Layout follows HiveWE mdx_reader.cpp read_LITE:
        //   LITE size (includes its own 4 bytes)
        //     node size (includes its own 4 bytes) + name(80) + objectId + parentId + flags
        //     type + attenuationStart + attenuationEnd + color(3) + intensity
        //     + ambientColor(3) + ambientIntensity + shadowIntensity(v1200+)
        [TestMethod]
        public void ParseModelWithV1200Light()
        {
            const float shadowIntensity = 0.3f;
            var bytes = CreateModelWithLight(1200, shadowIntensity);

            using var stream = new MemoryStream(bytes);
            var model = BinaryModelParser.Parse(stream, false);

            Assert.IsNotNull(model.Version);
            Assert.AreEqual(FormatVersion.Reforged2, model.Version.Value.FormatVersion);
            Assert.IsNotNull(model.Lights);
            Assert.AreEqual(1, model.Lights.Length);
            Assert.AreEqual("Omni01", model.Lights[0].Name);
            Assert.AreEqual(shadowIntensity, model.Lights[0].ShadowIntensity);
        }

        // Format versions before 1200 don't have the shadow intensity field; parsing must not
        // consume the following optional animation tag as if it were a shadow intensity value.
        [TestMethod]
        public void ParseModelWithV800Light()
        {
            var bytes = CreateModelWithLightAndVisibilityAnimation(800);

            using var stream = new MemoryStream(bytes);
            var model = BinaryModelParser.Parse(stream, false);

            Assert.IsNotNull(model.Lights);
            Assert.AreEqual(1, model.Lights.Length);
            Assert.AreEqual(0f, model.Lights[0].ShadowIntensity);
        }

        private static byte[] CreateModelWithLightAndVisibilityAnimation(int formatVersion)
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteModelHeader(writer, formatVersion);

                // LITE chunk
                writer.Write("LITE".FromRawcode());
                var liteSizePos = stream.Position;
                writer.Write(0); // patched below

                var lightSizePos = stream.Position;
                writer.Write(0); // light inclusive size, patched below
                WriteLightHeader(writer, "Omni01");

                // Optional animation tag after the fixed light header. For format versions < 1200
                // this tag immediately follows ambient intensity; a buggy parser that reads a
                // shadow intensity field here would consume the tag and fail.
                writer.Write("KLAV".FromRawcode());
                writer.Write(1); // key count
                writer.Write(0); // interpolation type
                writer.Write(uint.MaxValue); // global sequence id
                writer.Write(0); // frame
                writer.Write(1f); // value

                var lightEnd = stream.Position;
                PatchInt32(stream, lightSizePos, (int)(lightEnd - lightSizePos));

                var liteEnd = stream.Position;
                PatchInt32(stream, liteSizePos, (int)(liteEnd - liteSizePos - 4));
            }

            return stream.ToArray();
        }

        private static byte[] CreateModelWithLight(int formatVersion, float shadowIntensity)
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteModelHeader(writer, formatVersion);

                // LITE chunk
                writer.Write("LITE".FromRawcode());
                var liteSizePos = stream.Position;
                writer.Write(0); // patched below

                var lightSizePos = stream.Position;
                writer.Write(0); // light inclusive size, patched below
                WriteLightHeader(writer, "Omni01");

                if (formatVersion >= 1200)
                {
                    writer.Write(shadowIntensity);
                }

                var lightEnd = stream.Position;
                PatchInt32(stream, lightSizePos, (int)(lightEnd - lightSizePos));

                var liteEnd = stream.Position;
                PatchInt32(stream, liteSizePos, (int)(liteEnd - liteSizePos - 4));
            }

            return stream.ToArray();
        }

        private static void WriteModelHeader(BinaryWriter writer, int formatVersion)
        {
            writer.Write("MDLX".FromRawcode());

            // VERS chunk
            writer.Write("VERS".FromRawcode());
            writer.Write(4);
            writer.Write(formatVersion);
        }

        private static void WriteLightHeader(BinaryWriter writer, string name)
        {
            // Node
            var nodeSizePos = writer.BaseStream.Position;
            writer.Write(0); // node size, patched below
            WriteFixedString(writer, name, 80);
            writer.Write(0); // objectId
            writer.Write(-1); // parentId
            writer.Write(0x200); // flags: HiveWE Node::Flags::light
            var nodeEnd = writer.BaseStream.Position;
            PatchInt32(writer.BaseStream, nodeSizePos, (int)(nodeEnd - nodeSizePos));

            // Light fixed fields
            writer.Write(0); // type: Omni
            writer.Write(80f); // attenuationStart
            writer.Write(200f); // attenuationEnd
            writer.Write(1f); // color.x
            writer.Write(1f); // color.y
            writer.Write(1f); // color.z
            writer.Write(4f); // intensity
            writer.Write(1f); // ambientColor.x
            writer.Write(1f); // ambientColor.y
            writer.Write(1f); // ambientColor.z
            writer.Write(0f); // ambientIntensity
        }

        private static void WriteFixedString(BinaryWriter writer, string value, int length)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes);
            for (var i = bytes.Length; i < length; i++)
            {
                writer.Write((byte)0);
            }
        }

        private static void PatchInt32(MemoryStream stream, long position, int value)
        {
            var oldPosition = stream.Position;
            stream.Position = position;
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(value);
            }

            stream.Position = oldPosition;
        }
    }
}
