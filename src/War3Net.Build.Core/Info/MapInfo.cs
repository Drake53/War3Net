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

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Providers;
using War3Net.Common.Extensions;

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

        // Because why waste 15 bytes on some zeroes...
        private bool _skipData;

        private MapInfoFormatVersion _fileFormatVersion;
        private int _mapVersion;
        private int _editorVersion;
        private Version _gameVersion;

        private string _mapName;
        private string _mapAuthor;
        private string _mapDescription;
        private string _recommendedPlayers;

        private Quadrilateral _cameraBounds;
        private RectangleMargins? _cameraBoundsComplements;
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

        private ScriptLanguage _scriptLanguage; // Lua (1.31)

        private SupportedModes _supportedModes; // Reforged (1.32)
        private GameDataVersion _gameDataVersion; // Reforged (1.32)

        // v8 values
        private float _unk1;
        private int _unk2;
        private float _unk3;
        private float _unk4;
        private float _unk5;
        private int _unk6;
        private int _unk7;

        internal MapInfo()
        {
            _playerData = new List<PlayerData>();
            _forceData = new List<ForceData>();
            _upgradeData = new List<UpgradeData>();
            _techData = new List<TechData>();
            _unitTables = new List<RandomUnitTable>();
            _itemTables = new List<RandomItemTable>();
        }

        public static MapInfo Default
        {
            get
            {
                var info = new MapInfo();

                info._fileFormatVersion = MapInfoFormatVersion.Lua;
                info._mapVersion = 1;
                info._editorVersion = "W3N1".FromRawcode(); // War3Net.Build v1.x
                info._gameVersion = GamePatchVersionProvider.GetGameVersion(GamePatch.v1_31_1);

                info._mapName = "Just another Warcraft III map";
                info._mapAuthor = "Unknown";
                info._mapDescription = "Nondescript";
                info._recommendedPlayers = "Any";

                const int DefaultSize = 32;
                const int DefaultLeftComplement = 6;
                const int DefaultRightComplement = 6;
                const int DefaultTopComplement = 8;
                const int DefaultBottomComplement = 4;
                const int DefaultHorizontalBoundsOffset = 4;
                const int DefaultVerticalBoundsOffset = 2;
                const float TileSize = 128f;
                info._cameraBounds = new Quadrilateral(
                    -TileSize * (DefaultSize - DefaultLeftComplement - DefaultHorizontalBoundsOffset),
                    TileSize * (DefaultSize - DefaultRightComplement - DefaultHorizontalBoundsOffset),
                    TileSize * (DefaultSize - DefaultTopComplement - DefaultVerticalBoundsOffset),
                    -TileSize * (DefaultSize - DefaultBottomComplement - DefaultVerticalBoundsOffset));
                info._cameraBoundsComplements = new RectangleMargins(DefaultLeftComplement, DefaultRightComplement, DefaultBottomComplement, DefaultTopComplement);
                info._playableMapAreaWidth = (2 * DefaultSize) - DefaultLeftComplement - DefaultRightComplement;
                info._playableMapAreaHeight = (2 * DefaultSize) - DefaultBottomComplement - DefaultTopComplement;

                info._mapFlags
                    = MapFlags.UseItemClassificationSystem
                    | MapFlags.ShowWaterWavesOnRollingShores
                    | MapFlags.ShowWaterWavesOnCliffShores
                    | MapFlags.MeleeMap
                    | MapFlags.MaskedAreasArePartiallyVisible
                    | MapFlags.HasMapPropertiesMenuBeenOpened;
                info._tileset = Tileset.LordaeronSummer;

                info._loadingScreenBackgroundNumber = -1;
                info._loadingScreenPath = string.Empty;
                info._loadingScreenText = string.Empty;
                info._loadingScreenTitle = string.Empty;
                info._loadingScreenSubtitle = string.Empty;

                info._gameDataSet = GameDataSet.Unset;

                info._prologueScreenPath = string.Empty;
                info._prologueScreenText = string.Empty;
                info._prologueScreenTitle = string.Empty;
                info._prologueScreenSubtitle = string.Empty;

                info._fogStyle = FogStyle.Linear;
                info._fogStartZ = 3000f;
                info._fogEndZ = 5000f;
                info._fogDensity = 0.5f;
                info._fogColor = Color.Black;

                info._globalWeather = WeatherType.None;
                info._soundEnvironment = string.Empty;
                info._lightEnvironment = 0;
                info._waterTintingColor = Color.White;

                info._scriptLanguage = ScriptLanguage.Lua;

                info._supportedModes = SupportedModes.SD | SupportedModes.HD;
                info._gameDataVersion = GameDataVersion.TFT;

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

        public static bool IsRequired => true;

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
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Lua)
                {
                    throw new NotSupportedException($"Property {nameof(GameVersion)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Lua}' or higher.");
                }

                _gameVersion = value;
            }
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

        public RectangleMargins? CameraBoundsComplements
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

        public int CampaignBackgroundNumber
        {
            get => _campaignBackgroundNumber;
            set
            {
                if (_fileFormatVersion > MapInfoFormatVersion.RoC)
                {
                    throw new NotSupportedException($"Property {nameof(CampaignBackgroundNumber)} can only be set when {nameof(FormatVersion)} is version '{MapInfoFormatVersion.RoC}'.");
                }

                _campaignBackgroundNumber = value;
            }
        }

        public int LoadingScreenBackgroundNumber
        {
            get => _loadingScreenBackgroundNumber;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(LoadingScreenBackgroundNumber)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _loadingScreenBackgroundNumber = value;
            }
        }

        public string LoadingScreenPath
        {
            get => _loadingScreenPath;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(LoadingScreenPath)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _loadingScreenPath = value;
            }
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

        public int LoadingScreenNumber
        {
            get => _loadingScreenNumber;
            set
            {
                if (_fileFormatVersion > MapInfoFormatVersion.RoC)
                {
                    throw new NotSupportedException($"Property {nameof(LoadingScreenNumber)} can only be set when {nameof(FormatVersion)} is version '{MapInfoFormatVersion.RoC}'.");
                }

                _loadingScreenNumber = value;
            }
        }

        public GameDataSet GameDataSet
        {
            get => _gameDataSet;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(GameDataSet)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _gameDataSet = value;
            }
        }

        public string PrologueScreenPath
        {
            get => _prologueScreenPath;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(PrologueScreenPath)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _prologueScreenPath = value;
            }
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
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(FogStyle)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _fogStyle = value;
            }
        }

        public float FogStartZ
        {
            get => _fogStartZ;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(FogStartZ)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _fogStartZ = value;
            }
        }

        public float FogEndZ
        {
            get => _fogEndZ;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(FogEndZ)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _fogEndZ = value;
            }
        }

        public float FogDensity
        {
            get => _fogDensity;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(FogDensity)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _fogDensity = value;
            }
        }

        public Color FogColor
        {
            get => _fogColor;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(FogColor)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _fogColor = value;
            }
        }

        public WeatherType GlobalWeather
        {
            get => _globalWeather;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(GlobalWeather)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _globalWeather = value;
            }
        }

        public string SoundEnvironment
        {
            get => string.IsNullOrEmpty(_soundEnvironment) ? "Default" : _soundEnvironment;
            set => _soundEnvironment = value;
        }

        public Tileset LightEnvironment
        {
            get => _lightEnvironment == Tileset.Unspecified ? _tileset : _lightEnvironment;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(LightEnvironment)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _lightEnvironment = value;
            }
        }

        public Color WaterTintingColor
        {
            get => _waterTintingColor;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Tft)
                {
                    throw new NotSupportedException($"Property {nameof(WaterTintingColor)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Tft}' or higher.");
                }

                _waterTintingColor = value;
            }
        }

        public ScriptLanguage ScriptLanguage
        {
            get => _scriptLanguage;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Lua)
                {
                    throw new NotSupportedException($"Property {nameof(ScriptLanguage)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Lua}' or higher.");
                }

                _scriptLanguage = value;
            }
        }

        public SupportedModes SupportedModes
        {
            get => _supportedModes;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Reforged)
                {
                    throw new NotSupportedException($"Property {nameof(SupportedModes)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Reforged}' or higher.");
                }

                _supportedModes = value;
            }
        }

        public GameDataVersion GameDataVersion
        {
            get => _gameDataVersion;
            set
            {
                if (_fileFormatVersion < MapInfoFormatVersion.Reforged)
                {
                    throw new NotSupportedException($"Property {nameof(GameDataVersion)} cannot be set when {nameof(FormatVersion)} is not version '{MapInfoFormatVersion.Reforged}' or higher.");
                }

                _gameDataVersion = value;
            }
        }

        public int PlayerDataCount => _playerData.Count;

        public int ForceDataCount => _forceData.Count;

        public int UpgradeDataCount => _upgradeData.Count;

        public int TechDataCount => _techData.Count;

        public int RandomUnitTableCount => _unitTables.Count;

        public int RandomItemTableCount => _itemTables.Count;

        public static MapInfo Parse(Stream stream, bool leaveOpen = false)
        {
            try
            {
                var info = new MapInfo();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    info._fileFormatVersion = reader.ReadInt32<MapInfoFormatVersion>();
                    if (info._fileFormatVersion >= MapInfoFormatVersion.RoC)
                    {
                        info._mapVersion = reader.ReadInt32();
                        info._editorVersion = reader.ReadInt32();

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v27)
                        {
                            info._gameVersion = new Version(
                                reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32());
                        }
                    }

                    info._mapName = reader.ReadChars();
                    info._mapAuthor = reader.ReadChars();
                    info._mapDescription = reader.ReadChars();
                    info._recommendedPlayers = reader.ReadChars();

                    if (info._fileFormatVersion == MapInfoFormatVersion.v8)
                    {
                        info._unk1 = reader.ReadSingle();
                        info._unk2 = reader.ReadInt32();
                        info._unk3 = reader.ReadSingle();
                        info._unk4 = reader.ReadSingle();
                        info._unk5 = reader.ReadSingle();
                        info._unk6 = reader.ReadInt32();
                    }

                    info._cameraBounds = Quadrilateral.Parse(stream, true);
                    if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                    {
                        info._cameraBoundsComplements = RectangleMargins.Parse(stream, true);
                    }

                    info._playableMapAreaWidth = reader.ReadInt32();
                    info._playableMapAreaHeight = reader.ReadInt32();

                    if (info._fileFormatVersion == MapInfoFormatVersion.v8)
                    {
                        info._unk7 = reader.ReadInt32();
                    }

                    info._mapFlags = reader.ReadInt32<MapFlags>();
                    info._tileset = reader.ReadChar<Tileset>();

                    if (info._fileFormatVersion >= MapInfoFormatVersion.v23)
                    {
                        info._loadingScreenBackgroundNumber = reader.ReadInt32();
                        info._loadingScreenPath = reader.ReadChars();
                    }
                    else if (info._fileFormatVersion >= MapInfoFormatVersion.RoC)
                    {
                        info._campaignBackgroundNumber = reader.ReadInt32();
                    }
                    else if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                    {
                        info._loadingScreenPath = reader.ReadChars();
                    }

                    if (info._fileFormatVersion >= MapInfoFormatVersion.v10)
                    {
                        info._loadingScreenText = reader.ReadChars();
                        info._loadingScreenTitle = reader.ReadChars();
                        if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                        {
                            info._loadingScreenSubtitle = reader.ReadChars();
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v23)
                        {
                            info._gameDataSet = reader.ReadInt32<GameDataSet>();
                            info._prologueScreenPath = reader.ReadChars();
                        }
                        else if (info._fileFormatVersion >= MapInfoFormatVersion.RoC)
                        {
                            info._loadingScreenNumber = reader.ReadInt32();
                        }
                        else if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                        {
                            info._prologueScreenPath = reader.ReadChars();
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v11)
                        {
                            info._prologueScreenText = reader.ReadChars();
                            info._prologueScreenTitle = reader.ReadChars();
                            if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                            {
                                info._prologueScreenSubtitle = reader.ReadChars();
                            }
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v23)
                        {
                            info._fogStyle = reader.ReadInt32<FogStyle>();
                            info._fogStartZ = reader.ReadSingle();
                            info._fogEndZ = reader.ReadSingle();
                            info._fogDensity = reader.ReadSingle();
                            info._fogColor = reader.ReadColorRgba();

                            if (info._fileFormatVersion >= MapInfoFormatVersion.Tft)
                            {
                                info._globalWeather = reader.ReadInt32<WeatherType>();
                            }

                            info._soundEnvironment = reader.ReadChars();
                            info._lightEnvironment = reader.ReadChar<Tileset>();
                            info._waterTintingColor = reader.ReadColorRgba();
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.Lua)
                        {
                            info._scriptLanguage = reader.ReadInt32<ScriptLanguage>();
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.Reforged)
                        {
                            info._supportedModes = reader.ReadInt32<SupportedModes>();
                            info._gameDataVersion = reader.ReadInt32<GameDataVersion>();
                        }
                    }

                    var playerDataCount = reader.ReadInt32();
                    for (var i = 0; i < playerDataCount; i++)
                    {
                        info._playerData.Add(info._fileFormatVersion >= MapInfoFormatVersion.Reforged
                            ? ReforgedPlayerData.Parse(stream, true)
                            : PlayerData.Parse(stream, true));
                    }

                    var forceDataCount = reader.ReadInt32();
                    for (var i = 0; i < forceDataCount; i++)
                    {
                        info._forceData.Add(ForceData.Parse(stream, true));
                    }

                    if (reader.ReadByte() == 255)
                    {
                        info._skipData = true;
                    }
                    else
                    {
                        stream.Seek(-1, SeekOrigin.Current);

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

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v15)
                        {
                            var randomUnitTableCount = reader.ReadInt32();
                            for (var i = 0; i < randomUnitTableCount; i++)
                            {
                                info._unitTables.Add(RandomUnitTable.Parse(stream, true));
                            }
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v24)
                        {
                            var randomItemTableCount = reader.ReadInt32();
                            for (var i = 0; i < randomItemTableCount; i++)
                            {
                                info._itemTables.Add(RandomItemTable.Parse(stream, true));
                            }
                        }

                        if (info._fileFormatVersion >= MapInfoFormatVersion.v26 && info._fileFormatVersion < MapInfoFormatVersion.Lua)
                        {
                            if (reader.ReadInt32() != 0)
                            {
                                throw new InvalidDataException();
                            }
                        }
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

        public static void Serialize(MapInfo mapInfo, Stream stream, bool leaveOpen = false)
        {
            mapInfo.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            if (_fileFormatVersion >= MapInfoFormatVersion.v27 && _gameVersion is null)
            {
                throw new InvalidOperationException($"Cannot serialize {nameof(MapInfo)}, because {nameof(GameVersion)} is null.");
            }

            if (_cameraBounds is null)
            {
                throw new InvalidOperationException($"Cannot serialize {nameof(MapInfo)}, because {nameof(CameraBounds)} is null.");
            }

            if (_fileFormatVersion >= MapInfoFormatVersion.v15 && _cameraBoundsComplements is null)
            {
                throw new InvalidOperationException($"Cannot serialize {nameof(MapInfo)}, because {nameof(CameraBoundsComplements)} is null.");
            }

            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write((int)_fileFormatVersion);

                if (_fileFormatVersion >= MapInfoFormatVersion.RoC)
                {
                    writer.Write(_mapVersion);
                    writer.Write(_editorVersion);

                    if (_fileFormatVersion >= MapInfoFormatVersion.v27)
                    {
                        writer.Write(_gameVersion.Major);
                        writer.Write(_gameVersion.Minor);
                        writer.Write(_gameVersion.Build);
                        writer.Write(_gameVersion.Revision);
                    }
                }

                writer.WriteString(_mapName);
                writer.WriteString(_mapAuthor);
                writer.WriteString(_mapDescription);
                writer.WriteString(_recommendedPlayers);

                if (_fileFormatVersion == MapInfoFormatVersion.v8)
                {
                    writer.Write(_unk1);
                    writer.Write(_unk2);
                    writer.Write(_unk3);
                    writer.Write(_unk4);
                    writer.Write(_unk5);
                    writer.Write(_unk6);
                }

                _cameraBounds.WriteTo(writer);
                if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                {
                    _cameraBoundsComplements!.WriteTo(writer);
                }

                writer.Write(_playableMapAreaWidth);
                writer.Write(_playableMapAreaHeight);

                if (_fileFormatVersion == MapInfoFormatVersion.v8)
                {
                    writer.Write(_unk7);
                }

                writer.Write((int)_mapFlags);
                writer.Write((char)_tileset);

                if (_fileFormatVersion >= MapInfoFormatVersion.v23)
                {
                    writer.Write(_loadingScreenBackgroundNumber);
                    writer.WriteString(_loadingScreenPath);
                }
                else if (_fileFormatVersion >= MapInfoFormatVersion.RoC)
                {
                    writer.Write(_campaignBackgroundNumber);
                }
                else if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                {
                    writer.WriteString(_loadingScreenPath);
                }

                if (_fileFormatVersion >= MapInfoFormatVersion.v10)
                {
                    writer.WriteString(_loadingScreenText);
                    writer.WriteString(_loadingScreenTitle);
                    if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                    {
                        writer.WriteString(_loadingScreenSubtitle);
                    }

                    if (_fileFormatVersion >= MapInfoFormatVersion.v23)
                    {
                        writer.Write((int)_gameDataSet);
                        writer.WriteString(_prologueScreenPath);
                    }
                    else if (_fileFormatVersion >= MapInfoFormatVersion.RoC)
                    {
                        writer.Write(_loadingScreenNumber);
                    }
                    else if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                    {
                        writer.WriteString(_prologueScreenPath);
                    }

                    if (_fileFormatVersion >= MapInfoFormatVersion.v11)
                    {
                        writer.WriteString(_prologueScreenText);
                        writer.WriteString(_prologueScreenTitle);
                        if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                        {
                            writer.WriteString(_prologueScreenSubtitle);
                        }
                    }

                    if (_fileFormatVersion >= MapInfoFormatVersion.v23)
                    {
                        writer.Write((int)_fogStyle);
                        writer.Write(_fogStartZ);
                        writer.Write(_fogEndZ);
                        writer.Write(_fogDensity);
                        writer.Write(_fogColor.R);
                        writer.Write(_fogColor.G);
                        writer.Write(_fogColor.B);
                        writer.Write(_fogColor.A);

                        if (_fileFormatVersion >= MapInfoFormatVersion.Tft)
                        {
                            writer.Write((int)_globalWeather);
                        }

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

                    if (_fileFormatVersion >= MapInfoFormatVersion.Reforged)
                    {
                        writer.Write((int)_supportedModes);
                        writer.Write((int)_gameDataVersion);
                    }
                }

                writer.Write(_playerData.Count);
                foreach (var data in _playerData)
                {
                    if ((_fileFormatVersion >= MapInfoFormatVersion.Reforged) != (data is ReforgedPlayerData))
                    {
                        throw new InvalidDataException($"The '{FileName}' file has a {(_fileFormatVersion >= MapInfoFormatVersion.Reforged ? string.Empty : "non-")}Reforged file format version, but contains {(data is ReforgedPlayerData ? string.Empty : "non-")}Reforged PlayerData.");
                    }

                    data.WriteTo(writer);
                }

                writer.Write(_forceData.Count);
                foreach (var data in _forceData)
                {
                    data.WriteTo(writer);
                }

                if (_skipData)
                {
                    writer.Write((byte)255);
                    return;
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

                if (_fileFormatVersion >= MapInfoFormatVersion.v15)
                {
                    writer.Write(_unitTables.Count);
                    foreach (var table in _unitTables)
                    {
                        table.WriteTo(writer);
                    }
                }

                if (_fileFormatVersion >= MapInfoFormatVersion.v24)
                {
                    writer.Write(_itemTables.Count);
                    foreach (var table in _itemTables)
                    {
                        table.WriteTo(writer);
                    }
                }

                if (_fileFormatVersion >= MapInfoFormatVersion.v26 && _fileFormatVersion < MapInfoFormatVersion.Lua)
                {
                    writer.Write(0);
                }
            }
        }

        public void SetGameVersion(GamePatch gamePatch)
        {
            _gameVersion = GamePatchVersionProvider.GetGameVersion(gamePatch);
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

        public UpgradeData GetUpgradeData(int index)
        {
            return _upgradeData[index];
        }

        public void SetUpgradeData(params UpgradeData[] data)
        {
            _upgradeData.Clear();
            _upgradeData.AddRange(data);

            _skipData = false;
        }

        public TechData GetTechData(int index)
        {
            return _techData[index];
        }

        public void SetTechData(params TechData[] data)
        {
            _techData.Clear();
            _techData.AddRange(data);

            _skipData = false;
        }

        public RandomUnitTable GetUnitTable(int tableIndex)
        {
            return _unitTables[tableIndex];
        }

        public void SetUnitTables(params RandomUnitTable[] tables)
        {
            _unitTables.Clear();
            _unitTables.AddRange(tables);

            _skipData = false;
        }

        public RandomItemTable GetItemTable(int tableIndex)
        {
            return _itemTables[tableIndex];
        }

        public void SetItemTables(params RandomItemTable[] tables)
        {
            _itemTables.Clear();
            _itemTables.AddRange(tables);

            _skipData = false;
        }
    }
}