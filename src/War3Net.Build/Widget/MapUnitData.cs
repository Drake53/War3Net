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

        private int _hp;
        private int _mp;

        private int _mapItemTablePointer; // -1 == no table

        private int _goldAmount;
        private float _targetAcquisition;

        private int _heroLevel;
        private int _heroStrength;
        private int _heroAgility;
        private int _heroIntelligence;

        private RandomUnitData _randomData;

        private int _customPlayerColour; // 0-indexed, -1 == none
        private int _waygateDestination; // -1 == deactivated, otherwise refers to rect creation number in war3map.w3r
        private int _creationNumber;

        public string TypeId => new string(_typeId);

        public bool IsUnit => _heroLevel > 0;

        public bool IsItem => _heroLevel == 0;

        public bool IsRandomUnit => TypeId == "uDNR" || IsRandomBuilding;

        public bool IsRandomBuilding => TypeId == "bDNR";

        public bool IsRandomItem => TypeId == "iDNR";

        public int Variation => _variation;

        public float PositionX => _positionX;

        public float PositionY => _positionY;

        public float PositionZ => _positionZ;

        public float Facing => _rotation;

        public float ScaleX => _scaleX;

        public float ScaleY => _scaleY;

        public float ScaleZ => _scaleZ;

        public byte Flags => _flags;

        public int Owner => _owner;

        public byte Unk0 => _UNK0;

        public byte Unk1 => _UNK1;

        /// <summary>
        /// Use -1 for default hp.
        /// </summary>
        public int Hp => _hp;

        /// <summary>
        /// Use -1 for default mp.
        /// </summary>
        public int Mp => _mp;

        public int MapItemTablePointer => _mapItemTablePointer;

        public int GoldAmount => _goldAmount;

        /// <summary>
        /// Use -1 for default, and -2 for camp.
        /// </summary>
        public float TargetAcquisition => _targetAcquisition;

        /// <summary>
        /// Non-hero units are level 1.
        /// </summary>
        public int HeroLevel => _heroLevel;

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroStrength => _heroStrength;

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroAgility => _heroAgility;

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroIntelligence => _heroIntelligence;

        public RandomUnitData RandomData => _randomData;

        public int CustomPlayerColor => _customPlayerColour;

        public int WaygateDestination => _waygateDestination;

        public int CreationNumber => _creationNumber;

        public IEnumerable<InventoryItemData> Inventory => _inventory;

        public IEnumerable<ModifiedAbilityData> AbilityData => _modifiedAbilities;

        public IEnumerable<DroppedItemSetData> DroppedItemData => _mapItemTableDropData;

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

                var temp = new string(reader.ReadChars(4));
                if (temp.Equals(unitData.TypeId, StringComparison.Ordinal))
                {
                    // 4 + 1 + 42 + 4 + 4 + 4 + 4 = 63 bytes
                    var unk0 = reader.ReadByte(); // 2

                    stream.Seek(42, SeekOrigin.Current); // all zeroes
                    var unk1 = reader.ReadInt32(); // 1
                    var unk2 = reader.ReadInt32(); // -1
                    var unk3 = reader.ReadInt32(); // -1
                    stream.Seek(4, SeekOrigin.Current); // all zeroes

                    unitData._randomData = new RandomUnitData();

                    return unitData;
                }
                else
                {
                    // Assuming lists are empty:
                    // RoC: 1 + 4 + 1 + 1 + 4 + 4     + 4 + 4 + 4 + 4             + 4 + 4 + randomData + 4 + 4 + 4 = 51 + randomData
                    // TFT: 1 + 4 + 1 + 1 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + randomData + 4 + 4 + 4 = 67 + randomData
                    stream.Seek(-4, SeekOrigin.Current);
                }

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