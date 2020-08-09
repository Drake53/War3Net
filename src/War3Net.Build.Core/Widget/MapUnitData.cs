// ------------------------------------------------------------------------------
// <copyright file="MapUnitData.cs" company="Drake53">
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

        private char[] _skin;

        private byte _flags;
        private int _owner;
        private byte _UNK0;
        private byte _UNK1;

        private int _hp;
        private int _mp;

        private int _mapItemTablePointer;

        private int _goldAmount;
        private float _targetAcquisition;

        private int _heroLevel;
        private int _heroStrength;
        private int _heroAgility;
        private int _heroIntelligence;

        private RandomUnitData _randomData;

        private int _customPlayerColour;
        private int _waygateDestination;
        private int _creationNumber;

        public MapUnitData()
        {
            _inventory = new List<InventoryItemData>();
            _modifiedAbilities = new List<ModifiedAbilityData>();
            _mapItemTableDropData = new List<DroppedItemSetData>();
        }

        /// <param name="rotation">The unit's facing angle (in radians).</param>
        public MapUnitData(char[] typeId, float x, float y, float rotation, float scale, int owner, int creationNumber)
            : this()
        {
            _typeId = typeId;
            _variation = 0;
            _positionX = x;
            _positionY = y;
            _positionZ = 0;
            _rotation = rotation;
            _scaleX = scale;
            _scaleY = scale;
            _scaleZ = scale;

            _flags = 0;
            _owner = owner;
            // unk0/1

            _hp = -1;
            _mp = -1;

            _mapItemTablePointer = -1;

            _goldAmount = 12500;
            _targetAcquisition = -1;

            _heroLevel = 1;

            _randomData = new RandomUnitData();

            _customPlayerColour = -1;
            _waygateDestination = -1;
            _creationNumber = creationNumber;
        }

        public string TypeId => new string(_typeId);

        public bool IsUnit => _heroLevel > 0;

        public bool IsItem => _heroLevel == 0;

        public bool IsRandomUnit => TypeId == "uDNR" || IsRandomBuilding;

        public bool IsRandomBuilding => TypeId == "bDNR";

        public bool IsRandomItem => TypeId == "iDNR";

        public int Variation => _variation;

        public float PositionX
        {
            get => _positionX;
            set => _positionX = value;
        }

        public float PositionY
        {
            get => _positionY;
            set => _positionY = value;
        }

        public float PositionZ
        {
            get => _positionZ;
            set => _positionZ = value;
        }

        public float Facing
        {
            get => _rotation;
            set => _rotation = value;
        }

        public float FacingDeg
        {
            get => _rotation * (180 / MathF.PI);
            set => _rotation = value * (MathF.PI / 180);
        }

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

        public bool HasSkin => (_skin?.Length ?? 0) == 4 && new string(_skin) != new string(_typeId);

        public byte Flags => _flags;

        public int Owner => _owner;

        public byte Unk0 => _UNK0;

        public byte Unk1 => _UNK1;

        /// <summary>
        /// Use -1 for default hp.
        /// </summary>
        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        /// <summary>
        /// Use -1 for default mp.
        /// </summary>
        public int Mp
        {
            get => _mp;
            set => _mp = value;
        }

        /// <summary>
        /// Use -1 for no table.
        /// </summary>
        public int MapItemTablePointer => _mapItemTablePointer;

        public int GoldAmount
        {
            get => _goldAmount;
            set => _goldAmount = value;
        }

        /// <summary>
        /// Use -1 for default, and -2 for camp.
        /// </summary>
        public float TargetAcquisition
        {
            get => _targetAcquisition;
            set => _targetAcquisition = value;
        }

        /// <summary>
        /// Non-hero units are level 1, items are level 0.
        /// </summary>
        public int HeroLevel
        {
            get => _heroLevel;
            set => _heroLevel = value;
        }

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroStrength
        {
            get => _heroStrength;
            set => _heroStrength = value;
        }

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroAgility
        {
            get => _heroAgility;
            set => _heroAgility = value;
        }

        /// <summary>
        /// Use 0 for default.
        /// </summary>
        public int HeroIntelligence
        {
            get => _heroIntelligence;
            set => _heroIntelligence = value;
        }

        public RandomUnitData RandomData => _randomData;

        /// <summary>
        /// Use -1 for non-custom color.
        /// </summary>
        public int CustomPlayerColor
        {
            get => _customPlayerColour;
            set => _customPlayerColour = value;
        }

        /// <summary>
        /// Use -1 to deactivate. Index refers to <see cref="Environment.Region.CreationNumber"/>.
        /// </summary>
        public int WaygateDestination => _waygateDestination;

        public int CreationNumber => _creationNumber;

        public IEnumerable<InventoryItemData> Inventory => _inventory;

        public IEnumerable<ModifiedAbilityData> AbilityData => _modifiedAbilities;

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

        public static MapUnitData Parse(Stream stream, bool leaveOpen = false)
        {
            return Parse(stream, false, leaveOpen);
        }

        public static MapUnitData ParseTft(Stream stream, bool leaveOpen = false)
        {
            return Parse(stream, true, leaveOpen);
        }

        private static MapUnitData Parse(Stream stream, bool tft, bool leaveOpen)
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

                // Check if next byte is 'printable' (this also assumes that _flags byte is a low number).
                if (reader.PeekChar() >= 0x20)
                {
                    // Read reforged skin data (it's possible that the file contains this, but NOT tft data).
                    unitData._skin = reader.ReadChars(4);
                }

                unitData._flags = reader.ReadByte();
                unitData._owner = reader.ReadInt32();
                unitData._UNK0 = reader.ReadByte();
                unitData._UNK1 = reader.ReadByte();

                unitData._hp = reader.ReadInt32();
                unitData._mp = reader.ReadInt32();

                unitData._mapItemTablePointer = tft ? reader.ReadInt32() : -1;

                var droppedItemDataCount = reader.ReadInt32();
                for (var i = 0; i < droppedItemDataCount; i++)
                {
                    unitData._mapItemTableDropData.Add(DroppedItemSetData.Parse(stream, true));
                }

                unitData._goldAmount = reader.ReadInt32();
                unitData._targetAcquisition = reader.ReadSingle();

                unitData._heroLevel = reader.ReadInt32();
                if (tft)
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

            writer.Write(_flags);
            writer.Write(_owner);
            writer.Write(_UNK0);
            writer.Write(_UNK1);

            writer.Write(_hp);
            writer.Write(_mp);

            if (tft)
            {
                writer.Write(_mapItemTablePointer);
            }

            writer.Write(_mapItemTableDropData.Count);
            foreach (var droppedItemDataSet in _mapItemTableDropData)
            {
                droppedItemDataSet.WriteTo(writer);
            }

            writer.Write(_goldAmount);
            writer.Write(_targetAcquisition);

            writer.Write(_heroLevel);
            if (tft)
            {
                writer.Write(_heroStrength);
                writer.Write(_heroAgility);
                writer.Write(_heroIntelligence);
            }

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

        public override string ToString()
        {
            return $"{TypeId}{(HasSkin ? $" ({Skin})" : string.Empty)}";
        }
    }
}