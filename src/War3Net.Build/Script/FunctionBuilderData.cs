// ------------------------------------------------------------------------------
// <copyright file="FunctionBuilderData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Script
{
    internal sealed class FunctionBuilderData
    {
        private readonly MapInfo _mapInfo;
        private readonly MapDoodads _mapDoodads;
        private readonly MapUnits _mapUnits;
        private readonly MapRegions _mapRegions;
        private readonly MapSounds _mapSounds;
        private readonly string? _lobbyMusic;
        private readonly bool _csharp;

        public FunctionBuilderData(MapInfo mapInfo, MapDoodads mapDoodads, MapUnits mapUnits, MapRegions mapRegions, MapSounds mapSounds, string? lobbyMusic, bool csharp)
        {
            _mapInfo = mapInfo;
            _mapDoodads = mapDoodads;
            _mapUnits = mapUnits;
            _mapRegions = mapRegions;
            _mapSounds = mapSounds;
            _lobbyMusic = lobbyMusic;
            _csharp = csharp;
        }

        public MapInfo MapInfo => _mapInfo;

        public MapDoodads MapDoodads => _mapDoodads;

        public MapUnits MapUnits => _mapUnits;

        public MapRegions MapRegions => _mapRegions;

        public MapSounds MapSounds => _mapSounds;

        public string? LobbyMusic => _lobbyMusic;

        public bool CSharp => _csharp;
    }
}