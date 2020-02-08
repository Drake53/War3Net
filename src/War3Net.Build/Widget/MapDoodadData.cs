// ------------------------------------------------------------------------------
// <copyright file="MapDoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapDoodadData
    {
        private readonly List<DroppedItemSetData> _mapItemTableDropData;

        private char[] _typeId;
        private int _variation;

        private float _positionX;
        private float _positionY;
        private float _positionZ;
        private float _rotation;
        private float _scaleX;
        private float _scaleY;
        private float _scaleZ;

        private char[] _skin;

        private DoodadState _state;
        private byte _life; // in %, where 0x64 = 100%

        private int _mapItemTablePointer; // -1 == no table

        private int _creationNumber;

        public MapDoodadData()
        {
            _mapItemTableDropData = new List<DroppedItemSetData>();
        }

        public MapDoodadData(char[] typeId, float x, float y, float rotation, float scale, int creationNumber)
            : this(typeId, 0, x, y, rotation, scale, creationNumber)
        {
        }

        public MapDoodadData(char[] typeId, int variation, float x, float y, float rotation, float scale, int creationNumber)
            : this()
        {
            _typeId = typeId;
            _variation = variation;
            _positionX = x;
            _positionY = y;
            _positionZ = 0;
            _rotation = rotation;
            _scaleX = scale;
            _scaleY = scale;
            _scaleZ = scale;

            _state = DoodadState.Normal;
            _life = 100;
            _mapItemTablePointer = -1;
            _creationNumber = creationNumber;
        }

        public string TypeId => new string(_typeId);

        public int Variation => _variation;

        public float PositionX => _positionX;

        public float PositionY => _positionY;

        public float PositionZ => _positionZ;

        public float Facing => _rotation;

        public float FacingDeg => _rotation * (180 / MathF.PI);

        public float ScaleX => _scaleX;

        public float ScaleY => _scaleY;

        public float ScaleZ => _scaleZ;

        public string? Skin
        {
            get => new string(_skin);
            set
            {
                if ((value?.Length ?? 4) != 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Skin id string must be exactly 4 characters long, or it must be set to null.");
                }

                _skin = value?.ToCharArray();
            }
        }

        public DoodadState State => _state;

        public byte Life => _life;

        public int MapItemTablePointer => _mapItemTablePointer;

        public int CreationNumber => _creationNumber;

        public IEnumerable<DroppedItemSetData> DroppedItemData => _mapItemTableDropData;

        public IEnumerable<(int chance, string id)>[] DroppedItemSets
        {
            get
            {
                var itemSetCount = _mapItemTableDropData.Count;
                var itemSets = new IEnumerable<(int chance, string id)>[itemSetCount];
                for (var i = 0; i < itemSetCount; i++)
                {
                    itemSets[i] = _mapItemTableDropData[i].ItemSet.Select(itemSet => (itemSet.Item1, new string(itemSet.Item2)));
                }

                return itemSets;
            }
        }

        public static MapDoodadData Parse(Stream stream, bool leaveOpen = false)
        {
            return Parse(stream, MapWidgetsVersion.RoC, leaveOpen);
        }

        public static MapDoodadData ParseTft(Stream stream, bool leaveOpen = true)
        {
            return Parse(stream, MapWidgetsVersion.TFT, leaveOpen);
        }

        private static MapDoodadData Parse(Stream stream, MapWidgetsVersion version, bool leaveOpen)
        {
            var doodadData = new MapDoodadData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                doodadData._typeId = reader.ReadChars(4);
                doodadData._variation = reader.ReadInt32();

                doodadData._positionX = reader.ReadSingle();
                doodadData._positionY = reader.ReadSingle();
                doodadData._positionZ = reader.ReadSingle();
                doodadData._rotation = reader.ReadSingle();
                doodadData._scaleX = reader.ReadSingle();
                doodadData._scaleY = reader.ReadSingle();
                doodadData._scaleZ = reader.ReadSingle();

                // Check if next byte is 'printable'.
                if (reader.PeekChar() >= 0x20)
                {
                    // Read reforged skin data (it's possible that the file contains this, but NOT tft data).
                    doodadData._skin = reader.ReadChars(4);
                }

                doodadData._state = (DoodadState)reader.ReadByte();
                doodadData._life = reader.ReadByte();

                if (version >= MapWidgetsVersion.TFT)
                {
                    doodadData._mapItemTablePointer = reader.ReadInt32();

                    var droppedItemDataCount = reader.ReadInt32();
                    for (var i = 0; i < droppedItemDataCount; i++)
                    {
                        doodadData._mapItemTableDropData.Add(DroppedItemSetData.Parse(stream, true));
                    }
                }

                doodadData._creationNumber = reader.ReadInt32();
            }

            return doodadData;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer, bool tft = true)
        {
            writer.Write(_typeId);
            writer.Write(_variation);

            writer.Write(_positionX);
            writer.Write(_positionY);
            writer.Write(_positionZ);
            writer.Write(_rotation);
            writer.Write(_scaleX);
            writer.Write(_scaleY);
            writer.Write(_scaleZ);

            if (_skin != null)
            {
                writer.Write(_skin);
            }

            writer.Write((byte)_state);
            writer.Write(_life);

            if (tft)
            {
                writer.Write(_mapItemTablePointer);

                writer.Write(_mapItemTableDropData.Count);
                foreach (var droppedItemDataSet in _mapItemTableDropData)
                {
                    droppedItemDataSet.WriteTo(writer);
                }
            }

            writer.Write(_creationNumber);
        }
    }
}