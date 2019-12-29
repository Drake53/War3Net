// ------------------------------------------------------------------------------
// <copyright file="MapUnitData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class MapUnitData
    {
        private readonly List<InventoryItemData> _inventory;
        private readonly List<ModifiedAbilityData> _modifiedAbilities;
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

        private byte _flags; // refer to documentation of war3map.doo
        private int _owner;
        private byte _UNK0;
        private byte _UNK1;

        private int _hp; // -1 == default
        private int _mp; // -1 == default

        private int _mapItemTablePointer; // -1 == no table

        private int _goldAmount; // default: 12500
        private float _targetAcquisition; // -1 == normal, -2 == camp

        private int _heroLevel; // 1 if not hero
        private int _heroStrength; // 0 == default
        private int _heroAgility;
        private int _heroIntelligence;

        private RandomUnitData _randomData;

        private int _customPlayerColour; // 0-indexed, -1 == none
        private int _waygateDestination; // -1 == deactivated, otherwise refers to rect creation number in war3map.w3r
        private int _creationNumber;

        public int Owner => _owner;

        public string TypeId => new string(_typeId);

        public float PositionX => _positionX;

        public float PositionY => _positionY;

        public float Facing => _rotation;

        public int GoldAmount => _goldAmount;

        public MapUnitData()
        {
            _inventory = new List<InventoryItemData>();
            _modifiedAbilities = new List<ModifiedAbilityData>();
            _mapItemTableDropData = new List<DroppedItemSetData>();
        }

        public static MapUnitData Parse(Stream stream, bool leaveOpen = false)
        {
            return Parse(stream, MapWidgetsVersion.RoC, leaveOpen);
        }

        public static MapUnitData ParseTft(Stream stream, bool leaveOpen = true)
        {
            return Parse(stream, MapWidgetsVersion.TFT, leaveOpen);
        }

        private static MapUnitData Parse(Stream stream, MapWidgetsVersion version, bool leaveOpen)
        {
            var unitData = new MapUnitData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                unitData._typeId = reader.ReadChars(4);
                unitData._variation = reader.ReadInt32();

                unitData._positionX = reader.ReadSingle();
                unitData._positionY = reader.ReadSingle();
                unitData._positionZ = reader.ReadSingle();
                unitData._rotation = reader.ReadSingle();
                unitData._scaleX = reader.ReadSingle();
                unitData._scaleY = reader.ReadSingle();
                unitData._scaleZ = reader.ReadSingle();

                unitData._flags = reader.ReadByte();
                unitData._owner = reader.ReadInt32();
                unitData._UNK0 = reader.ReadByte();
                unitData._UNK1 = reader.ReadByte();

                unitData._hp = reader.ReadInt32();
                unitData._mp = reader.ReadInt32();

                unitData._mapItemTablePointer = version >= MapWidgetsVersion.TFT ? reader.ReadInt32() : -1;

                var droppedItemDataCount = reader.ReadInt32();
                for (var i = 0; i < droppedItemDataCount; i++)
                {
                    unitData._mapItemTableDropData.Add(DroppedItemSetData.Parse(stream, true));
                }

                unitData._goldAmount = reader.ReadInt32();
                unitData._targetAcquisition = reader.ReadSingle();

                unitData._heroLevel = reader.ReadInt32();
                if (version >= MapWidgetsVersion.TFT)
                {
                    unitData._heroStrength = reader.ReadInt32();
                    unitData._heroAgility = reader.ReadInt32();
                    unitData._heroIntelligence = reader.ReadInt32();
                }

                var inventoryItemCount = reader.ReadInt32();
                for (var i = 0; i < inventoryItemCount; i++)
                {
                    unitData._inventory.Add(InventoryItemData.Parse(stream, true));
                }

                var modifiedAbilityDataCount = reader.ReadInt32();
                for (var i = 0; i < modifiedAbilityDataCount; i++)
                {
                    unitData._modifiedAbilities.Add(ModifiedAbilityData.Parse(stream, true));
                }

                unitData._randomData = RandomUnitData.Parse(stream, true);

                unitData._customPlayerColour = reader.ReadInt32();
                unitData._waygateDestination = reader.ReadInt32();
                unitData._creationNumber = reader.ReadInt32();
            }

            return unitData;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                WriteTo(writer);
            }
        }

        public void WriteTo(BinaryWriter writer)
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

            writer.Write(_flags);
            writer.Write(_owner);
            writer.Write(_UNK0);
            writer.Write(_UNK1);

            writer.Write(_hp);
            writer.Write(_mp);

            writer.Write(_mapItemTablePointer);

            writer.Write(_mapItemTableDropData.Count);
            foreach (var droppedItemDataSet in _mapItemTableDropData)
            {
                droppedItemDataSet.WriteTo(writer);
            }

            writer.Write(_goldAmount);
            writer.Write(_targetAcquisition);

            writer.Write(_heroLevel);
            writer.Write(_heroStrength);
            writer.Write(_heroAgility);
            writer.Write(_heroIntelligence);

            writer.Write(_inventory.Count);
            foreach (var inventoryItemData in _inventory)
            {
                inventoryItemData.WriteTo(writer);
            }

            writer.Write(_modifiedAbilities.Count);
            foreach (var modifiedAbilityData in _modifiedAbilities)
            {
                modifiedAbilityData.WriteTo(writer);
            }

            _randomData.WriteTo(writer);

            writer.Write(_customPlayerColour);
            writer.Write(_waygateDestination);
            writer.Write(_creationNumber);
        }
    }
}