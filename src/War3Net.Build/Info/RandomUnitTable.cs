// ------------------------------------------------------------------------------
// <copyright file="RandomUnitTable.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build
{
    public sealed class RandomUnitTable
    {
        private readonly List<WidgetType> _positionTypes;
        private readonly List<RandomUnitSet> _sets;

        private int _tableNumber;
        private string _tableName;

        public RandomUnitTable()
        {
            _positionTypes = new List<WidgetType>();
            _sets = new List<RandomUnitSet>();
        }

        public static RandomUnitTable Parse(Stream stream, bool leaveOpen = false)
        {
            var table = new RandomUnitTable();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                table._tableNumber = reader.ReadInt32();
                table._tableName = reader.ReadChars();

                var positionCount = reader.ReadInt32(); // amount of columns
                for (var x = 0; x < positionCount; x++)
                {
                    table._positionTypes.Add((WidgetType)reader.ReadInt32());
                }

                var setCount = reader.ReadInt32(); // amount of rows
                for (var y = 0; y < setCount; y++)
                {
                    var set = new RandomUnitSet(reader.ReadInt32());
                    for (var x = 0; x < positionCount; x++)
                    {
                        set.AddId(reader.ReadChars(4));
                    }

                    table._sets.Add(set);
                }
            }

            return table;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_tableNumber);
            writer.WriteString(_tableName);

            writer.Write(_positionTypes.Count);
            foreach (var positionType in _positionTypes)
            {
                writer.Write((int)positionType);
            }

            writer.Write(_sets.Count);
            foreach (var set in _sets)
            {
                writer.Write(set.Chance);
                foreach (var id in set)
                {
                    writer.Write(id);
                }
            }
        }
    }
}