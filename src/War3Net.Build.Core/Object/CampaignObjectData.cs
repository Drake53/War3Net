// ------------------------------------------------------------------------------
// <copyright file="CampaignObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class CampaignObjectData
    {
        public const string FileName = "war3campaign.w3o";
        public const ObjectDataFormatVersion LatestVersion = ObjectDataFormatVersion.Normal;

        private ObjectDataFormatVersion _fileFormatVersion;

        private CampaignUnitObjectData? _unitData;
        private CampaignItemObjectData? _itemData;
        private CampaignDestructableObjectData? _destructableData;
        private CampaignDoodadObjectData? _doodadData;
        private CampaignAbilityObjectData? _abilityData;
        private CampaignBuffObjectData? _buffData;
        private CampaignUpgradeObjectData? _upgradeData;

        public CampaignObjectData(
            CampaignUnitObjectData? unitData = null,
            CampaignItemObjectData? itemData = null,
            CampaignDestructableObjectData? destructableData = null,
            CampaignDoodadObjectData? doodadData = null,
            CampaignAbilityObjectData? abilityData = null,
            CampaignBuffObjectData? buffData = null,
            CampaignUpgradeObjectData? upgradeData = null)
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

        internal CampaignObjectData()
        {
        }

        public CampaignUnitObjectData UnitData
        {
            get => _unitData;
            set => _unitData = value;
        }

        public CampaignItemObjectData ItemData
        {
            get => _itemData;
            set => _itemData = value;
        }

        public CampaignDestructableObjectData DestructableData
        {
            get => _destructableData;
            set => _destructableData = value;
        }

        public CampaignDoodadObjectData DoodadData
        {
            get => _doodadData;
            set => _doodadData = value;
        }

        public CampaignAbilityObjectData AbilityData
        {
            get => _abilityData;
            set => _abilityData = value;
        }

        public CampaignBuffObjectData BuffData
        {
            get => _buffData;
            set => _buffData = value;
        }

        public CampaignUpgradeObjectData UpgradeData
        {
            get => _upgradeData;
            set => _upgradeData = value;
        }

        public ObjectDataFormatVersion FormatVersion
        {
            get => _fileFormatVersion;
            set => _fileFormatVersion = value;
        }

        public static CampaignObjectData Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var data = new CampaignObjectData();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    data._fileFormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

                    data._unitData = reader.ReadBool() ? CampaignUnitObjectData.Parse(stream, true) : null;
                    data._itemData = reader.ReadBool() ? CampaignItemObjectData.Parse(stream, true) : null;
                    data._destructableData = reader.ReadBool() ? CampaignDestructableObjectData.Parse(stream, true) : null;
                    data._doodadData = reader.ReadBool() ? CampaignDoodadObjectData.Parse(stream, true) : null;
                    data._abilityData = reader.ReadBool() ? CampaignAbilityObjectData.Parse(stream, true) : null;
                    data._buffData = reader.ReadBool() ? CampaignBuffObjectData.Parse(stream, true) : null;
                    data._upgradeData = reader.ReadBool() ? CampaignUpgradeObjectData.Parse(stream, true) : null;
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

        public static void Serialize(CampaignObjectData objectData, Stream stream, bool leaveOpen = false)
        {
            objectData.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_fileFormatVersion);

                writer.WriteBool(_unitData is not null);
                _unitData?.SerializeTo(stream, true);

                writer.WriteBool(_itemData is not null);
                _itemData?.SerializeTo(stream, true);

                writer.WriteBool(_destructableData is not null);
                _destructableData?.SerializeTo(stream, true);

                writer.WriteBool(_doodadData is not null);
                _doodadData?.SerializeTo(stream, true);

                writer.WriteBool(_abilityData is not null);
                _abilityData?.SerializeTo(stream, true);

                writer.WriteBool(_buffData is not null);
                _buffData?.SerializeTo(stream, true);

                writer.WriteBool(_upgradeData is not null);
                _upgradeData?.SerializeTo(stream, true);
            }
        }
    }
}