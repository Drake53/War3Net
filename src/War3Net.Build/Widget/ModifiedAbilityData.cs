// ------------------------------------------------------------------------------
// <copyright file="ModifiedAbilityData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    public sealed class ModifiedAbilityData
    {
        private char[] _abilityId;
        private int _isAutocastActive;
        private int _heroAbilityLevel;

        public static ModifiedAbilityData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new ModifiedAbilityData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._abilityId = reader.ReadChars(4);
                data._isAutocastActive = reader.ReadInt32(); // 0 == no, 1 == active
                data._heroAbilityLevel = reader.ReadInt32();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_abilityId);
            writer.Write(_isAutocastActive);
            writer.Write(_heroAbilityLevel);
        }
    }
}