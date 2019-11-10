// ------------------------------------------------------------------------------
// <copyright file="MapInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using War3Net.Build.Common;
using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed class MapInfo
    {
        public const string FileName = "war3map.w3i";

        private readonly List<PlayerData> _playerData;
        private readonly List<ForceData> _forceData;
        private readonly List<UpgradeData> _upgradeData;
        private readonly List<TechData> _techData;
        private readonly List<RandomUnitTable> _unitTables;
        private readonly List<RandomItemTable> _itemTables;

        private MapInfoFormatVersion _fileFormatVersion;
        private int _mapVersion;
        private int _editorVersion;
        private Version _gameVersion;

        private string _mapName;
        private string _mapAuthor;
        private string _mapDescription;
        private string _recommendedPlayers;

        private Quadrilateral _cameraBounds;
        private RectangleMargins _cameraBoundsComplements;
        private int _playableMapAreaWidth;
        private int _playableMapAreaHeight;

        private MapFlags _mapFlags;
        private Tileset _tileset;

        private int _campaignBackgroundNumber; // RoC
        private int _loadingScreenBackgroundNumber;
        private string _loadingScreenPath;
        private string _loadingScreenText;
        private string _loadingScreenTitle;
        private string _loadingScreenSubtitle;
        private int _loadingScreenNumber; // RoC

        private GameDataSet _gameDataSet;

        private string _prologueScreenPath;
        private string _prologueScreenText;
        private string _prologueScreenTitle;
        private string _prologueScreenSubtitle;

        private FogStyle _fogStyle;
        private float _fogStartZ;
        private float _fogEndZ;
        private float _fogDensity;
        private Color _fogColor;

        private WeatherType _globalWeather;
        private string _soundEnvironment;
        private Tileset _lightEnvironment;
        private Color _waterTintingColor;

        private ScriptLanguage _scriptLanguage;

        public MapInfo()
        {
            _playerData = new List<PlayerData>();
            _forceData = new List<ForceData>();
            _upgradeData = new List<UpgradeData>();
            _techData = new List<TechData>();
            _unitTables = new List<RandomUnitTable>();
            _itemTables = new List<RandomItemTable>();
        }

        public MapInfoFormatVersion FormatVersion
        {
            get => _fileFormatVersion;
            set => _fileFormatVersion = value;
        }

        public int MapVersion
        {
            get => _mapVersion;
            set => _mapVersion = value;
        }

        public int EditorVersion
        {
            get => _editorVersion;
            set => _editorVersion = value;
        }

        public Version GameVersion
        {
            get => _gameVersion;
            set => _gameVersion = value;
        }

        public string MapName
        {
            get => _mapName;
            set => _mapName = value;
        }

        public string MapAuthor
        {
            get => _mapAuthor;
            set => _mapAuthor = value;
        }

        public string MapDescription
        {
            get => _mapDescription;
            set => _mapDescription = value;
        }

        public string RecommendedPlayers
        {
            get => _recommendedPlayers;
            set => _recommendedPlayers = value;
        }

        public Quadrilateral CameraBounds
        {
            get => _cameraBounds;
            set => _cameraBounds = value;
        }

        public RectangleMargins CameraBoundsComplements
        {
            get => _cameraBoundsComplements;
            set => _cameraBoundsComplements = value;
        }

        // Equal to entire map width minus cameraBoundsComplement[0] and [1]
        public int PlayableMapAreaWidth
        {
            get => _playableMapAreaWidth;
            set => _playableMapAreaWidth = value;
        }

        // Equal to entire map minus height cameraBoundsComplement[2] and [3]
        public int PlayableMapAreaHeight
        {
            get => _playableMapAreaHeight;
            set => _playableMapAreaHeight = value;
        }

        public MapFlags MapFlags
        {
            get => _mapFlags;
            set => _mapFlags = value;
        }

        public Tileset Tileset
        {
            get => _tileset;
            set => _tileset = value;
        }

        [Obsolete("This field is only used in the RoC file format version.")]
        public int CampaignBackgroundNumber
        {
            get => _campaignBackgroundNumber;
            set => _campaignBackgroundNumber = value;
        }

        public int LoadingScreenBackgroundNumber
        {
            get => _loadingScreenBackgroundNumber;
            set => _loadingScreenBackgroundNumber = value;
        }

        public string LoadingScreenPath
        {
            get => _loadingScreenPath;
            set => _loadingScreenPath = value;
        }

        public string LoadingScreenText
        {
            get => _loadingScreenText;
            set => _loadingScreenText = value;
        }

        public string LoadingScreenTitle
        {
            get => _loadingScreenTitle;
            set => _loadingScreenTitle = value;
        }

        public string LoadingScreenSubtitle
        {
            get => _loadingScreenSubtitle;
            set => _loadingScreenSubtitle = value;
        }

        [Obsolete("This field is only used in the RoC file format version.")]
        public int LoadingScreenNumber
        {
            get => _loadingScreenNumber;
            set => _loadingScreenNumber = value;
        }

        public GameDataSet GameDataSet
        {
            get => _gameDataSet;
            set => _gameDataSet = value;
        }

        public string PrologueScreenPath
        {
            get => _prologueScreenPath;
            set => _prologueScreenPath = value;
        }

        public string PrologueScreenText
        {
            get => _prologueScreenText;
            set => _prologueScreenText = value;
        }

        public string PrologueScreenTitle
        {
            get => _prologueScreenTitle;
            set => _prologueScreenTitle = value;
        }

        public string PrologueScreenSubtitle
        {
            get => _prologueScreenSubtitle;
            set => _prologueScreenSubtitle = value;
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

        public WeatherType GlobalWeather
        {
            get => _globalWeather;
            set => _globalWeather = value;
        }

        public string SoundEnvironment
        {
            get => string.IsNullOrEmpty(_soundEnvironment) ? "Default" : _soundEnvironment;
        }

        public Tileset LightEnvironment
        {
            get => _lightEnvironment == Tileset.Unspecified ? _tileset : _lightEnvironment;
            set => _lightEnvironment = value;
        }

        public Color WaterTintingColor
        {
            get => _waterTintingColor;
            set => _waterTintingColor = value;
        }

        public ScriptLanguage ScriptLanguage
        {
            get => _scriptLanguage;
            set => _scriptLanguage = value;
        }

        public int PlayerDataCount => _playerData.Count;

        public int ForceDataCount => _forceData.Count;

        public int UpgradeDataCount => _upgradeData.Count;

        public int TechDataCount => _techData.Count;

        public int RandomUnitTableCount => _unitTables.Count;

        public int RandomItemTableCount => _itemTables.Count;

        public static MapInfo Default
        {
            get
            {
                var info = new MapInfo();

                info._fileFormatVersion = MapInfoFormatVersion.Lua;
                info._mapVersion = 1;
                info._editorVersion = 0x304E3357; // [W]ar[3][N]et.Build v[0].2.1
                info._gameVersion = new Version(1, 31, 1, 12164);

                info._mapName = "Just another Warcraft III map";
                info._mapAuthor = "Unknown";
                info._mapDescription = "Nondescript";
                info._recommendedPlayers = "Any";

                info._cameraBounds = new Quadrilateral(-2816f, 2816f, 2816f, -3328f);
                info._cameraBoundsComplements = new RectangleMargins(6, 6, 4, 8);
                info._playableMapAreaWidth = 52;
                info._playableMapAreaHeight = 52;

                info._mapFlags
                    = MapFlags.UseItemClassificationSystem
                    | MapFlags.ShowWaterWavesOnRollingShores
                    | MapFlags.ShowWaterWavesOnCliffShores
                    | MapFlags.MeleeMap
                    | MapFlags.MaskedAreasArePartiallyVisible
                    | MapFlags.HasMapPropertiesMenuBeenOpened;
                info._tileset = Tileset.LordaeronSummer;

                info._loadingScreenBackgroundNumber = -1;
                info._loadingScreenPath = null;
                info._loadingScreenText = null;
                info._loadingScreenTitle = null;
                info._loadingScreenSubtitle = null;

                info._gameDataSet = GameDataSet.Unset;

                info._prologueScreenPath = null;
                info._prologueScreenText = null;
                info._prologueScreenTitle = null;
                info._prologueScreenSubtitle = null;

                info._fogStyle = FogStyle.Linear;
                info._fogStartZ = 3000f;
                info._fogEndZ = 5000f;
                info._fogDensity = 0.5f;
                info._fogColor = Color.Black;

                info._globalWeather = WeatherType.None;
                info._soundEnvironment = null;
                info._lightEnvironment = 0;
                info._waterTintingColor = Color.White;

                info._scriptLanguage = ScriptLanguage.Lua;

                var player0 = new PlayerData()
                {
                    PlayerNumber = 0,
                    PlayerName = "Player 1",
                    PlayerController = PlayerController.User,
                    PlayerRace = PlayerRace.Human,
                    IsRaceSelectable = false,
                    StartPosition = new PointF(0f, 0f),
                    FixedStartPosition = false,
                };
                info.SetPlayerData(player0);

                var team0 = new ForceData()
                {
                    ForceName = "Team 1",
                    ForceFlags = 0,
                };
                team0.SetPlayers(player0);
                info.SetForceData(team0);

                return info;
            }
        }

        public static MapInfo Parse(Stream stream, bool leaveOpen = false)
        {
            var info = new MapInfo();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                info._fileFormatVersion = (MapInfoFormatVersion)reader.ReadInt32();
                info._mapVersion = reader.ReadInt32();
                info._editorVersion = reader.ReadInt32();

                if (info._fileFormatVersion >= MapInfoFormatVersion.Lua)
                {
                    info._gameVersion = new Version(
                        reader.ReadInt32(),
                        reader.ReadInt32(),
                        reader.ReadInt32(),
                        reader.ReadInt32());
                }

                info._mapName = reader.ReadChars();
                info._mapAuthor = reader.ReadChars();
                info._mapDescription = reader.ReadChars();
                info._recommendedPlayers = reader.ReadChars();

                info._cameraBounds = Quadrilateral.Parse(stream, true);
                info._cameraBoundsComplements = RectangleMargins.Parse(stream, true);
                info._playableMapAreaWidth = reader.ReadInt32();
                info._playableMapAreaHeight = reader.ReadInt32();

                info._mapFlags = (MapFlags)reader.ReadInt32();
                info._tileset = (Tileset)reader.ReadChar();

                if (info._fileFormatVersion == MapInfoFormatVersion.RoC)
                {
                    info._campaignBackgroundNumber = reader.ReadInt32();
                }
                else
                {
                    info._loadingScreenBackgroundNumber = reader.ReadInt32();
                    info._loadingScreenPath = reader.ReadChars();
                }

                info._loadingScreenText = reader.ReadChars();
                info._loadingScreenTitle = reader.ReadChars();
                info._loadingScreenSubtitle = reader.ReadChars();

                if (info._fileFormatVersion == MapInfoFormatVersion.RoC)
                {
                    info._loadingScreenNumber = reader.ReadInt32();
                }
                else
                {
                    info._gameDataSet = (GameDataSet)reader.ReadInt32();
                    info._prologueScreenPath = reader.ReadChars();
                }

                info._prologueScreenText = reader.ReadChars();
                info._prologueScreenTitle = reader.ReadChars();
                info._prologueScreenSubtitle = reader.ReadChars();

                if (info._fileFormatVersion >= MapInfoFormatVersion.Tft)
                {
                    info._fogStyle = (FogStyle)reader.ReadInt32();
                    info._fogStartZ = reader.ReadSingle();
                    info._fogEndZ = reader.ReadSingle();
                    info._fogDensity = reader.ReadSingle();
                    info._fogColor = reader.ReadColorRgba();

                    info._globalWeather = (WeatherType)reader.ReadInt32();
                    info._soundEnvironment = reader.ReadChars();
                    info._lightEnvironment = (Tileset)reader.ReadChar();
                    info._waterTintingColor = reader.ReadColorRgba();
                }

                if (info._fileFormatVersion >= MapInfoFormatVersion.Lua)
                {
                    info._scriptLanguage = (ScriptLanguage)reader.ReadInt32();
                }

                var playerDataCount = reader.ReadInt32();
                for (var i = 0; i < playerDataCount; i++)
                {
                    info._playerData.Add(PlayerData.Parse(stream, true));
                }

                var forceDataCount = reader.ReadInt32();
                for (var i = 0; i < forceDataCount; i++)
                {
                    info._forceData.Add(ForceData.Parse(stream, true));
                }

                var upgradeDataCount = reader.ReadInt32();
                for (var i = 0; i < upgradeDataCount; i++)
                {
                    info._upgradeData.Add(UpgradeData.Parse(stream, true));
                }

                var techDataCount = reader.ReadInt32();
                for (var i = 0; i < techDataCount; i++)
                {
                    info._techData.Add(TechData.Parse(stream, true));
                }

                var randomUnitTableCount = reader.ReadInt32();
                for (var i = 0; i < randomUnitTableCount; i++)
                {
                    info._unitTables.Add(RandomUnitTable.Parse(stream, true));
                }

                if (info._fileFormatVersion >= MapInfoFormatVersion.Tft)
                {
                    var randomItemTableCount = reader.ReadInt32();
                    for (var i = 0; i < randomItemTableCount; i++)
                    {
                        info._itemTables.Add(RandomItemTable.Parse(stream, true));
                    }
                }
            }

            return info;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_fileFormatVersion);
                writer.Write(_mapVersion);
                writer.Write(_editorVersion);

                if (_fileFormatVersion >= MapInfoFormatVersion.Lua)
                {
                    writer.Write(_gameVersion.Major);
                    writer.Write(_gameVersion.Minor);
                    writer.Write(_gameVersion.Build);
                    writer.Write(_gameVersion.Revision);
                }

                writer.WriteString(_mapName);
                writer.WriteString(_mapAuthor);
                writer.WriteString(_mapDescription);
                writer.WriteString(_recommendedPlayers);

                _cameraBounds.WriteTo(writer);
                _cameraBoundsComplements.WriteTo(writer);
                writer.Write(_playableMapAreaWidth);
                writer.Write(_playableMapAreaHeight);

                writer.Write((int)_mapFlags);
                writer.Write((char)_tileset);

                if (_fileFormatVersion == MapInfoFormatVersion.RoC)
                {
                    writer.Write(_campaignBackgroundNumber);
                }
                else
                {
                    writer.Write(_loadingScreenBackgroundNumber);
                    writer.WriteString(_loadingScreenPath);
                }

                writer.WriteString(_loadingScreenText);
                writer.WriteString(_loadingScreenTitle);
                writer.WriteString(_loadingScreenSubtitle);

                if (_fileFormatVersion == MapInfoFormatVersion.RoC)
                {
                    writer.Write(_loadingScreenNumber);
                }
                else
                {
                    writer.Write((int)_gameDataSet);
                    writer.WriteString(_prologueScreenPath);
                }

                writer.WriteString(_prologueScreenText);
                writer.WriteString(_prologueScreenTitle);
                writer.WriteString(_prologueScreenSubtitle);

                if (_fileFormatVersion >= MapInfoFormatVersion.Tft)
                {
                    writer.Write((int)_fogStyle);
                    writer.Write(_fogStartZ);
                    writer.Write(_fogEndZ);
                    writer.Write(_fogDensity);
                    writer.Write(_fogColor.R);
                    writer.Write(_fogColor.G);
                    writer.Write(_fogColor.B);
                    writer.Write(_fogColor.A);

                    writer.Write((int)_globalWeather);
                    writer.WriteString(_soundEnvironment);
                    writer.Write((char)_lightEnvironment);

                    writer.Write(_waterTintingColor.R);
                    writer.Write(_waterTintingColor.G);
                    writer.Write(_waterTintingColor.B);
                    writer.Write(_waterTintingColor.A);
                }

                if (_fileFormatVersion >= MapInfoFormatVersion.Lua)
                {
                    writer.Write((int)_scriptLanguage);
                }

                writer.Write(_playerData.Count);
                foreach (var data in _playerData)
                {
                    data.WriteTo(writer);
                }

                writer.Write(_forceData.Count);
                foreach (var data in _forceData)
                {
                    data.WriteTo(writer);
                }

                writer.Write(_upgradeData.Count);
                foreach (var data in _upgradeData)
                {
                    data.WriteTo(writer);
                }

                writer.Write(_techData.Count);
                foreach (var data in _techData)
                {
                    data.WriteTo(writer);
                }

                writer.Write(_unitTables.Count);
                foreach (var table in _unitTables)
                {
                    table.WriteTo(writer);
                }

                if (_fileFormatVersion >= MapInfoFormatVersion.Tft)
                {
                    writer.Write(_itemTables.Count);
                    foreach (var table in _itemTables)
                    {
                        table.WriteTo(writer);
                    }
                }
            }
        }

        public void SetSoundEnvironment(SoundEnvironment soundEnvironment)
        {
            _soundEnvironment = soundEnvironment.ToString();
        }

        public PlayerData GetPlayerData(int index)
        {
            return _playerData[index];
        }

        public void SetPlayerData(params PlayerData[] data)
        {
            _playerData.Clear();
            _playerData.AddRange(data);
        }

        public bool PlayerExists(int playerIndex)
        {
            foreach (var playerData in _playerData)
            {
                if (playerData.PlayerNumber == playerIndex)
                {
                    return true;
                }
            }

            return false;
        }

        public ForceData GetForceData(int index)
        {
            return _forceData[index];
        }

        public void SetForceData(params ForceData[] data)
        {
            _forceData.Clear();
            _forceData.AddRange(data);
        }

        public ForceData GetForceData(PlayerData player, out int teamIndex)
        {
            teamIndex = 0;
            foreach (var force in _forceData)
            {
                if (force.ContainsPlayer(player.PlayerNumber))
                {
                    return force;
                }

                teamIndex++;
            }

            teamIndex = -1;
            return null;
        }
    }
}