﻿// ------------------------------------------------------------------------------
// <copyright file="MapShadowMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace War3Net.Build.Environment
{
    public class MapShadowMap
    {
        public const string FileName = "war3map.shd";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapShadowMap"/> class.
        /// </summary>
        public MapShadowMap()
        {
        }

        internal MapShadowMap(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        // True = 0xff, false = 0x00
        public List<byte> Cells { get; init; } = new();

        public override string ToString() => FileName;

        internal void ReadFrom(BinaryReader reader)
        {
            Cells.AddRange(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Cells.ToArray());
        }
    }
}