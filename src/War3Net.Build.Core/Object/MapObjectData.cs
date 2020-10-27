// ------------------------------------------------------------------------------
// <copyright file="MapObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class MapObjectData
    {
        public const string FileName = "war3map.w3o";
        public const ObjectDataFormatVersion LatestVersion = ObjectDataFormatVersion.Normal;

        private ObjectDataFormatVersion _fileFormatVersion;

        private MapUnitObjectData? _unitData;
        private MapItemObjectData? _itemData;
        private MapDestructableObjectData? _destructableData;
        private MapDoodadObjectData? _doodadData;
        private MapAbilityObjectData? _abilityData;
        private MapBuffObjectData? _buffData;
        private MapUpgradeObjectData? _upgradeData;

        public MapObjectData(
            MapUnitObjectData? unitData = null,
            MapItemObjectData? itemData = null,
            MapDestructableObjectData? destructableData = null,
            MapDoodadObjectData? doodadData = null,
            MapAbilityObjectData? abilityData = null,
            MapBuffObjectData? buffData = null,
            MapUpgradeObjectData? upgradeData = null)
        {
            _unitData = unitData;
            _itemData = itemData;
            _destructableData = destructableData;
            _doodadData = doodadData;
            _abilityData = abilityData;
            _buffData = buffData;
            _upgradeData = upgradeData;

            _fileFormatVersion = LatestVersion;
        }

        internal MapObjectData()
        {
        }

        public MapUnitObjectData? UnitData
        {
            get => _unitData;
            set => _unitData = value;
        }

        public MapItemObjectData? ItemData
        {
            get => _itemData;
            set => _itemData = value;
        }

        public MapDestructableObjectData? DestructableData
        {
            get => _destructableData;
            set => _destructableData = value;
        }

        public MapDoodadObjectData? DoodadData
        {
            get => _doodadData;
            set => _doodadData = value;
        }

        public MapAbilityObjectData? AbilityData
        {
            get => _abilityData;
            set => _abilityData = value;
        }

        public MapBuffObjectData? BuffData
        {
            get => _buffData;
            set => _buffData = value;
        }

        public MapUpgradeObjectData? UpgradeData
        {
            get => _upgradeData;
            set => _upgradeData = value;
        }

        public ObjectDataFormatVersion FormatVersion
        {
            get => _fileFormatVersion;
            set => _fileFormatVersion = value;
        }

        public static MapObjectData Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapObjectData();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._fileFormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

                    data._unitData = reader.ReadBool() ? MapUnitObjectData.Parse(stream, true) : null;
                    data._itemData = reader.ReadBool() ? MapItemObjectData.Parse(stream, true) : null;
                    data._destructableData = reader.ReadBool() ? MapDestructableObjectData.Parse(stream, true) : null;
                    data._doodadData = reader.ReadBool() ? MapDoodadObjectData.Parse(stream, true) : null;
                    data._abilityData = reader.ReadBool() ? MapAbilityObjectData.Parse(stream, true) : null;
                    data._buffData = reader.ReadBool() ? MapBuffObjectData.Parse(stream, true) : null;
                    data._upgradeData = reader.ReadBool() ? MapUpgradeObjectData.Parse(stream, true) : null;
                }

                return data;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The '{FileName}' file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The '{FileName}' file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
        }

        public static void Serialize(MapObjectData objectData, Stream stream, bool leaveOpen = false)
        {
            objectData.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_fileFormatVersion);

                writer.WriteBool(!(_unitData is null));
                _unitData?.SerializeTo(stream, true);

                writer.WriteBool(!(_itemData is null));
                _itemData?.SerializeTo(stream, true);

                writer.WriteBool(!(_destructableData is null));
                _destructableData?.SerializeTo(stream, true);

                writer.WriteBool(!(_doodadData is null));
                _doodadData?.SerializeTo(stream, true);

                writer.WriteBool(!(_abilityData is null));
                _abilityData?.SerializeTo(stream, true);

                writer.WriteBool(!(_buffData is null));
                _buffData?.SerializeTo(stream, true);

                writer.WriteBool(!(_upgradeData is null));
                _upgradeData?.SerializeTo(stream, true);
            }
        }
    }
}