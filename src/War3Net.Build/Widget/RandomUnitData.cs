// ------------------------------------------------------------------------------
// <copyright file="RandomUnitData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Widget
{
    // used if _typeId is uDNR or iDNR
    // for non-random units, _mode is 0 with its data also 0
    public sealed class RandomUnitData : IEnumerable<(char[] id, int chance)>
    {
        // Mode 2:
        private readonly List<(char[] id, int chance)> _randomUnits;

        private int _mode;

        // Mode 0:
        private int _levelAndClass; // class should be 0 for units

        // Mode 1:
        private int _unitGroupTableIndex;
        private int _column;

        public RandomUnitData()
        {
            _randomUnits = new List<(char[] id, int chance)>();
        }

        public int Mode => _mode;

        public int Level => _levelAndClass == 0x00ffffff ? -1 : _levelAndClass;

        // Signed little-endian 24-bit number
        public int ItemLevel
        {
            get
            {
                // Doesn't work because this assumes the number is big-endian
                // (int)((uint)((_levelAndClass & 0x7fffff00) >> 8) | ((uint)_levelAndClass & 0x80000000));

                // hacky
                if ((_levelAndClass & 0x00ffff00) != 0)
                {
                    return -1;
                }

                var @byte = (byte)(((uint)_levelAndClass & 0xff000000) >> 24);
                return @byte - 1;
            }
        }

        public byte ItemClass
        {
            get
            {
                var @byte = (byte)(_levelAndClass & 0xff);
                if (@byte == byte.MaxValue)
                {
                    // Any class
                    return 8;
                }
                else
                {
                    // Unknown class
                    return @byte >= 8 ? (byte)7 : @byte;
                }
            }
        }

        public int UnitGroupTableIndex => _unitGroupTableIndex;

        public int UnitGroupTableColumn => _column;

        public static RandomUnitData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new RandomUnitData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._mode = reader.ReadInt32();
                switch (data._mode)
                {
                    case 0:
                        data._levelAndClass = reader.ReadInt32();
                        break;

                    case 1:
                        data._unitGroupTableIndex = reader.ReadInt32();
                        data._column = reader.ReadInt32();
                        break;

                    case 2:
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            data._randomUnits.Add((reader.ReadChars(4), reader.ReadInt32()));
                        }

                        break;

                    default: throw new InvalidDataException();
                }
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_mode);
            switch (_mode)
            {
                case 0:
                    writer.Write(_levelAndClass);
                    break;

                case 1:
                    writer.Write(_unitGroupTableIndex);
                    writer.Write(_column);
                    break;

                case 2:
                    writer.Write(_randomUnits.Count);
                    foreach (var (id, chance) in _randomUnits)
                    {
                        writer.Write(id);
                        writer.Write(chance);
                    }

                    break;
            }
        }

        public IEnumerator<(char[] id, int chance)> GetEnumerator()
        {
            return ((IEnumerable<(char[] id, int chance)>)_randomUnits).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<(char[] id, int chance)>)_randomUnits).GetEnumerator();
        }
    }
}