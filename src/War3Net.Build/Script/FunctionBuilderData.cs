// ------------------------------------------------------------------------------
// <copyright file="FunctionBuilderData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Script
{
    internal sealed class FunctionBuilderData
    {
        private readonly MapInfo _mapInfo;
        private readonly MapUnits _mapUnits;

        public FunctionBuilderData(MapInfo mapInfo, MapUnits mapUnits)
        {
            _mapInfo = mapInfo;
            _mapUnits = mapUnits;
        }

        public MapInfo MapInfo => _mapInfo;

        public MapUnits MapUnits => _mapUnits;
    }
}