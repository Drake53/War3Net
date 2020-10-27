// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class ObjectDataModification
    {
        private int _id;
        private object _value;
        private ObjectDataType _type;

        private int? _level; // For doodads, this number indicates the variation.
        private int _pointer;

        private int _sanityCheck;

        public ObjectDataModification(int id, int value)
            : this(id, value, ObjectDataType.Int, null)
        {
        }

        /// <param name="isUnreal">
        /// Set to <see langword="true"/> for <see cref="ObjectDataType.Unreal"/>, and <see langword="false"/> for <see cref="ObjectDataType.Real"/>.
        /// Which one should be used depends on <paramref name="id"/>'s type (found in metadata.slk's 'type' column).
        /// </param>
        public ObjectDataModification(int id, float value, bool isUnreal)
            : this(id, value, isUnreal ? ObjectDataType.Unreal : ObjectDataType.Real, null)
        {
        }

        public ObjectDataModification(int id, string value)
            : this(id, value, ObjectDataType.String, null)
        {
        }

        public ObjectDataModification(int id, bool value)
            : this(id, value, ObjectDataType.Bool, null)
        {
        }

        public ObjectDataModification(int id, char value)
            : this(id, value, ObjectDataType.Char, null)
        {
        }

        public ObjectDataModification(int id, int level, int value)
            : this(id, value, ObjectDataType.Int, level)
        {
        }

        /// <param name="isUnreal">
        /// Set to <see langword="true"/> for <see cref="ObjectDataType.Unreal"/>, and <see langword="false"/> for <see cref="ObjectDataType.Real"/>.
        /// Which one should be used depends on <paramref name="id"/>'s type (found in metadata.slk's 'type' column).
        /// </param>
        public ObjectDataModification(int id, int level, float value, bool isUnreal)
            : this(id, value, isUnreal ? ObjectDataType.Unreal : ObjectDataType.Real, level)
        {
        }

        public ObjectDataModification(int id, int level, string value)
            : this(id, value, ObjectDataType.String, level)
        {
        }

        public ObjectDataModification(int id, int level, bool value)
            : this(id, value, ObjectDataType.Bool, level)
        {
        }

        public ObjectDataModification(int id, int level, char value)
            : this(id, value, ObjectDataType.Char, level)
        {
        }

        public ObjectDataModification(string rawcode, int value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Int, null)
        {
        }

        /// <param name="isUnreal">
        /// Set to <see langword="true"/> for <see cref="ObjectDataType.Unreal"/>, and <see langword="false"/> for <see cref="ObjectDataType.Real"/>.
        /// Which one should be used depends on <paramref name="rawcode"/>'s type (found in metadata.slk's 'type' column).
        /// </param>
        public ObjectDataModification(string rawcode, float value, bool isUnreal)
            : this(rawcode.FromRawcode(), value, isUnreal ? ObjectDataType.Unreal : ObjectDataType.Real, null)
        {
        }

        public ObjectDataModification(string rawcode, string value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.String, null)
        {
        }

        public ObjectDataModification(string rawcode, bool value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Bool, null)
        {
        }

        public ObjectDataModification(string rawcode, char value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Char, null)
        {
        }

        public ObjectDataModification(string rawcode, int level, int value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Int, level)
        {
        }

        /// <param name="isUnreal">
        /// Set to <see langword="true"/> for <see cref="ObjectDataType.Unreal"/>, and <see langword="false"/> for <see cref="ObjectDataType.Real"/>.
        /// Which one should be used depends on <paramref name="rawcode"/>'s type (found in metadata.slk's 'type' column).
        /// </param>
        public ObjectDataModification(string rawcode, int level, float value, bool isUnreal)
            : this(rawcode.FromRawcode(), value, isUnreal ? ObjectDataType.Unreal : ObjectDataType.Real, level)
        {
        }

        public ObjectDataModification(string rawcode, int level, string value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.String, level)
        {
        }

        public ObjectDataModification(string rawcode, int level, bool value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Bool, level)
        {
        }

        public ObjectDataModification(string rawcode, int level, char value)
            : this(rawcode.FromRawcode(), value, ObjectDataType.Char, level)
        {
        }

        private ObjectDataModification(int id, object value, ObjectDataType type, int? level, int pointer = 0)
        {
            _id = id;
            _value = value;
            _type = type;

            _level = level;
            _pointer = pointer;
            _sanityCheck = 0;
        }

        private ObjectDataModification()
        {
        }

        public int Id => _id;

        public object Value => _value;

        public ObjectDataType Type => _type;

        public int? Level => _level;

        public int ValueAsInt => _type == ObjectDataType.Int ? (int)_value : throw new InvalidOperationException($"Modification is of type {_type}, so cannot retrieve it as {ObjectDataType.Int}.");

        public float ValueAsFloat => _type == ObjectDataType.Real || _type == ObjectDataType.Unreal ? (float)_value : throw new InvalidOperationException($"Modification is of type {_type}, so cannot retrieve it as {ObjectDataType.Real} or {ObjectDataType.Unreal}.");

        public string ValueAsString => _type == ObjectDataType.String ? (string)_value : throw new InvalidOperationException($"Modification is of type {_type}, so cannot retrieve it as {ObjectDataType.String}.");

        public bool ValueAsBool => _type == ObjectDataType.Bool ? (bool)_value : throw new InvalidOperationException($"Modification is of type {_type}, so cannot retrieve it as {ObjectDataType.Bool}.");

        public char ValueAsChar => _type == ObjectDataType.Char ? (char)_value : throw new InvalidOperationException($"Modification is of type {_type}, so cannot retrieve it as {ObjectDataType.Char}.");

        internal int SanityCheck
        {
            get => _sanityCheck;
            set => _sanityCheck = value;
        }

        public static ObjectDataModification Parse(Stream stream, int oldId, int newId, bool readLevelData, bool leaveOpen = false)
        {
            var data = new ObjectDataModification();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._id = reader.ReadInt32();
                data._type = reader.ReadInt32<ObjectDataType>();

                if (readLevelData)
                {
                    data._level = reader.ReadInt32();
                    data._pointer = reader.ReadInt32();
                }

                switch (data._type)
                {
                    case ObjectDataType.Int:
                        data._value = reader.ReadInt32();
                        break;

                    case ObjectDataType.Real:
                    case ObjectDataType.Unreal:
                        data._value = reader.ReadSingle();
                        break;

                    case ObjectDataType.Bool:
                        data._value = reader.ReadBoolean();
                        break;

                    case ObjectDataType.Char:
                        data._value = reader.ReadChar();
                        break;

                    case ObjectDataType.String:
                        data._value = reader.ReadChars();
                        break;
                }

                data._sanityCheck = reader.ReadInt32();

                // var sanityCheck = reader.ReadInt32();
                // if (sanityCheck != 0 && sanityCheck != oldId && sanityCheck != newId)
                // {
                //     var expectedMessage = newId == 0 ? $"0 or '{oldId.ToRawcode()}'" : $"0, '{oldId.ToRawcode()}', or '{newId.ToRawcode()}'";
                //     throw new InvalidDataException($"Sanity check failed. Expected {expectedMessage}, but got '{sanityCheck.ToRawcode()}'.");
                // }

                // data._sanityCheck = sanityCheck;
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_id);
            writer.Write((int)_type);

            if (_level.HasValue)
            {
                writer.Write(_level.Value);
                writer.Write(_pointer);
            }

            switch (_type)
            {
                case ObjectDataType.Int:
                    writer.Write((int)_value); break;

                case ObjectDataType.Real:
                case ObjectDataType.Unreal:
                    writer.Write((float)_value); break;

                case ObjectDataType.Bool:
                    writer.Write((bool)_value); break;

                case ObjectDataType.Char:
                    writer.Write((char)_value); break;

                case ObjectDataType.String:
                    writer.WriteString((string)_value); break;
            }

            writer.Write(_sanityCheck);
        }
    }
}