// ------------------------------------------------------------------------------
// <copyright file="MapObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.IO;
using System.Text;

namespace War3Net.Build.Object
{
    public sealed class MapObjectData
    {
        public const string FileName = "war3map.w3o";

        private ObjectDataFormatVersion _fileFormatVersion;

        private MapUnitObjectData? _unitData;
        private MapItemObjectData? _itemData;
        private MapDestructableObjectData? _destructableData;
        private MapDoodadObjectData? _doodadData;
        private MapAbilityObjectData? _abilityData;
        private MapBuffObjectData? _buffData;
        private MapUpgradeObjectData? _upgradeData;

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

        public static MapObjectData Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new MapObjectData();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._fileFormatVersion = (ObjectDataFormatVersion)reader.ReadInt32();
                    if (!Enum.IsDefined(typeof(ObjectDataFormatVersion), data._fileFormatVersion))
                    {
                        throw new NotSupportedException($"Unknown version of '{FileName}': {data._fileFormatVersion}");
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._unitData = MapUnitObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._itemData = MapItemObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._destructableData = MapDestructableObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._doodadData = MapDoodadObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._abilityData = MapAbilityObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._buffData = MapBuffObjectData.Parse(stream, true);
                    }

                    if (reader.ReadInt32() != 0)
                    {
                        data._upgradeData = MapUpgradeObjectData.Parse(stream, true);
                    }
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

                writer.Write(_unitData is null ? 0 : 1);
                _unitData?.SerializeTo(stream, true);

                writer.Write(_itemData is null ? 0 : 1);
                _itemData?.SerializeTo(stream, true);

                writer.Write(_destructableData is null ? 0 : 1);
                _destructableData?.SerializeTo(stream, true);

                writer.Write(_doodadData is null ? 0 : 1);
                _doodadData?.SerializeTo(stream, true);

                writer.Write(_abilityData is null ? 0 : 1);
                _abilityData?.SerializeTo(stream, true);

                writer.Write(_buffData is null ? 0 : 1);
                _buffData?.SerializeTo(stream, true);

                writer.Write(_upgradeData is null ? 0 : 1);
                _upgradeData?.SerializeTo(stream, true);
            }
        }
    }
}