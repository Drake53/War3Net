// ------------------------------------------------------------------------------
// <copyright file="CampaignInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class CampaignInfo
    {
        public const string FileName = "war3campaign.w3f";

        private readonly List<CampaignMapButton> _mapButtons;
        private readonly List<CampaignMap> _maps;

        private CampaignInfoFormatVersion _fileFormatVersion;
        private int _campaignVersion;
        private int _editorVersion;

        private string _campaignName;
        private string _campaignDifficulty;
        private string _campaignAuthor;
        private string _campaignDescription;

        private CampaignFlags _campaignFlags;

        private int _campaignBackgroundNumber;
        private string _backgroundScreenPath;
        private string _minimapPath;

        private int _ambientSoundNumber;
        private string _ambientSoundPath;

        private FogStyle _fogStyle;
        private float _fogStartZ;
        private float _fogEndZ;
        private float _fogDensity;
        private Color _fogColor;

        private CampaignRace _race;

        internal CampaignInfo()
        {
            _mapButtons = new List<CampaignMapButton>();
            _maps = new List<CampaignMap>();
        }

        public static CampaignInfo Default
        {
            get
            {
                var info = new CampaignInfo();

                info._fileFormatVersion = CampaignInfoFormatVersion.Normal;
                info._campaignVersion = 1;
                info._editorVersion = 0x314E3357; // [W]ar[3][N]et.Build v[1].x

                info._campaignName = "Just another Warcraft III campaign";
                info._campaignDifficulty = "Normal";
                info._campaignAuthor = "Unknown";
                info._campaignDescription = "Nondescript";

                info._campaignFlags = 0;

                info._campaignBackgroundNumber = -1;
                info._backgroundScreenPath = string.Empty;
                info._minimapPath = string.Empty;
                info._ambientSoundNumber = -1;
                info._ambientSoundPath = string.Empty;

                info._fogStyle = (FogStyle)(-1);
                info._fogStartZ = 0f;
                info._fogEndZ = 0f;
                info._fogDensity = 0f;
                info._fogColor = Color.Black;

                info._race = CampaignRace.Human;

                return info;
            }
        }

        public static bool IsRequired => true;

        public CampaignInfoFormatVersion FormatVersion
        {
            get => _fileFormatVersion;
            set => _fileFormatVersion = value;
        }

        public int CampaignVersion
        {
            get => _campaignVersion;
            set => _campaignVersion = value;
        }

        public int EditorVersion
        {
            get => _editorVersion;
            set => _editorVersion = value;
        }

        public string CampaignName
        {
            get => _campaignName;
            set => _campaignName = value;
        }

        public string CampaignDifficulty
        {
            get => _campaignDifficulty;
            set => _campaignDifficulty = value;
        }

        public string CampaignAuthor
        {
            get => _campaignAuthor;
            set => _campaignAuthor = value;
        }

        public string CampaignDescription
        {
            get => _campaignDescription;
            set => _campaignDescription = value;
        }

        public CampaignFlags CampaignFlags
        {
            get => _campaignFlags;
            set => _campaignFlags = value;
        }

        public int CampaignBackgroundNumber
        {
            get => _campaignBackgroundNumber;
            set => _campaignBackgroundNumber = value;
        }

        public string BackgroundScreenPath
        {
            get => _backgroundScreenPath;
            set => _backgroundScreenPath = value;
        }

        public string MinimapPath
        {
            get => _minimapPath;
            set => _minimapPath = value;
        }

        public int AmbientSoundNumber
        {
            get => _ambientSoundNumber;
            set => _ambientSoundNumber = value;
        }

        public string AmbientSoundPath
        {
            get => _ambientSoundPath;
            set => _ambientSoundPath = value;
        }

        public FogStyle FogStyle
        {
            get => _fogStyle;
            set => _fogStyle = value;
        }

        public float FogStartZ
        {
            get => _fogStartZ;
            set => _fogStartZ = value;
        }

        public float FogEndZ
        {
            get => _fogEndZ;
            set => _fogEndZ = value;
        }

        public float FogDensity
        {
            get => _fogDensity;
            set => _fogDensity = value;
        }

        public Color FogColor
        {
            get => _fogColor;
            set => _fogColor = value;
        }

        public CampaignRace Race
        {
            get => _race;
            set => _race = value;
        }

        public int MapButtonCount => _mapButtons.Count;

        public int MapCount => _maps.Count;

        public static CampaignInfo Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var info = new CampaignInfo();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    info._fileFormatVersion = (CampaignInfoFormatVersion)reader.ReadInt32();
                    if (!Enum.IsDefined(typeof(CampaignInfoFormatVersion), info._fileFormatVersion))
                    {
                        throw new NotSupportedException($"Unknown version of '{FileName}': {info._fileFormatVersion}");
                    }

                    info._campaignVersion = reader.ReadInt32();
                    info._editorVersion = reader.ReadInt32();

                    info._campaignName = reader.ReadChars();
                    info._campaignDifficulty = reader.ReadChars();
                    info._campaignAuthor = reader.ReadChars();
                    info._campaignDescription = reader.ReadChars();

                    info._campaignFlags = (CampaignFlags)reader.ReadInt32();

                    info._campaignBackgroundNumber = reader.ReadInt32();
                    info._backgroundScreenPath = reader.ReadChars();
                    info._minimapPath = reader.ReadChars();

                    info._ambientSoundNumber = reader.ReadInt32();
                    info._ambientSoundPath = reader.ReadChars();

                    info._fogStyle = (FogStyle)reader.ReadInt32();
                    info._fogStartZ = reader.ReadSingle();
                    info._fogEndZ = reader.ReadSingle();
                    info._fogDensity = reader.ReadSingle();
                    info._fogColor = reader.ReadColorRgba();

                    info._race = (CampaignRace)reader.ReadInt32();

                    var mapCount = reader.ReadInt32();
                    for (var i = 0; i < mapCount; i++)
                    {
                        info._mapButtons.Add(CampaignMapButton.Parse(stream, true));
                    }

                    var mapOrderCount = reader.ReadInt32();
                    for (var i = 0; i < mapOrderCount; i++)
                    {
                        info._maps.Add(CampaignMap.Parse(stream, true));
                    }
                }

                return info;
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

        public static void Serialize(CampaignInfo campaignInfo, Stream stream, bool leaveOpen = false)
        {
            campaignInfo.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_fileFormatVersion);
                writer.Write(_campaignVersion);
                writer.Write(_editorVersion);

                writer.WriteString(_campaignName);
                writer.WriteString(_campaignDifficulty);
                writer.WriteString(_campaignAuthor);
                writer.WriteString(_campaignDescription);

                writer.Write((int)_campaignFlags);

                writer.Write(_campaignBackgroundNumber);
                writer.WriteString(_backgroundScreenPath);
                writer.WriteString(_minimapPath);

                writer.Write(_ambientSoundNumber);
                writer.WriteString(_ambientSoundPath);

                writer.Write((int)_fogStyle);
                writer.Write(_fogStartZ);
                writer.Write(_fogEndZ);
                writer.Write(_fogDensity);
                writer.Write(_fogColor.R);
                writer.Write(_fogColor.G);
                writer.Write(_fogColor.B);
                writer.Write(_fogColor.A);

                writer.Write((int)_race);

                writer.Write(_mapButtons.Count);
                foreach (var map in _mapButtons)
                {
                    map.WriteTo(writer);
                }

                writer.Write(_maps.Count);
                foreach (var mapOrder in _maps)
                {
                    mapOrder.WriteTo(writer);
                }
            }
        }

        public CampaignMapButton GetMapButton(int index)
        {
            return _mapButtons[index];
        }

        public void SetMapButtons(params CampaignMapButton[] buttons)
        {
            _mapButtons.Clear();
            _mapButtons.AddRange(buttons);
        }

        public CampaignMap GetMap(int index)
        {
            return _maps[index];
        }

        public void SetMaps(params CampaignMap[] maps)
        {
            _maps.Clear();
            _maps.AddRange(maps);
        }
    }
}