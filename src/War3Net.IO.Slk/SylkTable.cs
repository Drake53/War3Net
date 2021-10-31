// ------------------------------------------------------------------------------
// <copyright file="SylkTable.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.IO.Slk
{
    public sealed class SylkTable : IEnumerable<object[]>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly object[,] _values;

        private int _rows;
        private int _columns;

        public SylkTable(int width, int height)
        {
            _width = width;
            _height = height;
            _values = new object[_width, _height];
        }

        /// <summary>
        /// Gets the maximum amount of columns that can fit in the <see cref="SylkTable"/>.
        /// </summary>
        public int Width => _width;

        /// <summary>
        /// Gets the maximum amount of rows that can fit in the <see cref="SylkTable"/>.
        /// </summary>
        public int Height => _height;

        /// <summary>
        /// Gets the highest 0-indexed row index for which at least one cell is not empty.
        /// </summary>
        public int Rows => _rows;

        /// <summary>
        /// Gets the highest 0-indexed column index for which at least one cell is not empty.
        /// </summary>
        public int Columns => _columns;

        /// <summary>
        /// Gets or sets the content of a single cell in the <see cref="SylkTable"/>.
        /// </summary>
        /// <param name="column">The 0-indexed X position.</param>
        /// <param name="row">The 0-indexed Y position.</param>
        public object this[int column, int row]
        {
            get => _values[column, row];
            set
            {
                if (column < 0 || column >= _width)
                {
                    throw new ArgumentOutOfRangeException(nameof(column));
                }

                if (row < 0 || row >= _height)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }

                var oldValue = _values[column, row];
                _values[column, row] = value;

                if (value is null)
                {
                    if (oldValue is not null)
                    {
                        RecalculateRowsAndColumns(true);
                    }
                }
                else
                {
                    _rows = _rows <= row ? row : _rows;
                    _columns = _columns <= column ? column : _columns;
                }
            }
        }

        public IEnumerable<int> this[object columnName]
        {
            get
            {
                for (var column = 0; column < _width; column++)
                {
                    if (Equals(_values[column, 0], columnName))
                    {
                        yield return column;
                    }
                }
            }
        }

        /// <summary>
        /// Reduces the width and height of a table by clipping off empty rows and columns.
        /// </summary>
        /// <returns>A new table with empty rows and columns clipped off, or the same table if its size can't be shrunk.</returns>
        public SylkTable Shrink()
        {
            var newWidth = _columns + 1;
            var newHeight = _rows + 1;

            if (newWidth == _width && newHeight == _height)
            {
                return this;
            }

            var newTable = new SylkTable(newWidth, newHeight);
            for (var row = 0; row < newHeight; row++)
            {
                for (var column = 0; column < newWidth; column++)
                {
                    newTable._values[column, row] = _values[column, row];
                }
            }

            newTable._columns = _columns;
            newTable._rows = _rows;

            return newTable;
        }

        public SylkTable Combine(SylkTable other, object thisColumn, object otherColumn)
        {
            return Combine(other, thisColumn, otherColumn, thisColumn);
        }

        /// <param name="newColumn">The column name of the column on which the tables were joined in the resulting table.</param>
        public SylkTable Combine(SylkTable other, object thisColumn, object otherColumn, object newColumn)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var thisColumnIndex = this[thisColumn].Single();
            var otherColumnIndex = other[otherColumn].Single();

            var newTable = Combine(other, thisColumnIndex, otherColumnIndex);

            newTable._values[thisColumnIndex, 0] = newColumn;

            return newTable;
        }

        /// <param name="other">The table with which to combine this table into a new table.</param>
        /// <param name="thisColumnIndex">The 0-indexed column index in this table on which to join the tables.</param>
        /// <param name="otherColumnIndex">The 0-indexed column index in the other table on which to join the tables.</param>
        public SylkTable Combine(SylkTable other, int thisColumnIndex, int otherColumnIndex)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var keyToRowMappings = new Dictionary<object, int>();

            for (var row = 1; row < _height; row++)
            {
                var key = _values[thisColumnIndex, row];
                keyToRowMappings.Add(key, row);
            }

            for (var row = 1; row < other._height; row++)
            {
                var key = other._values[otherColumnIndex, row];
                if (!keyToRowMappings.ContainsKey(key))
                {
                    keyToRowMappings.Add(key, keyToRowMappings.Count + 1);
                }
            }

            var newWidth = _width + other._width - 1;
            var newHeight = keyToRowMappings.Count + 1;

            var newTable = new SylkTable(newWidth, newHeight);

            // Copy this table into new table.
            for (var row = 0; row < _height; row++)
            {
                for (var column = 0; column < _width; column++)
                {
                    newTable._values[column, row] = _values[column, row];
                }
            }

            // Copy other table into new table.
            for (var row = 0; row < other._height; row++)
            {
                var targetRow = row == 0 ? 0 : keyToRowMappings[other._values[otherColumnIndex, row]];

                for (var column = 0; column < other._width; column++)
                {
                    if (column == otherColumnIndex)
                    {
                        continue;
                    }

                    var targetColumn = _width + column + (column > otherColumnIndex ? -1 : 0);

                    newTable._values[targetColumn, targetRow] = other._values[column, row];
                }
            }

            newTable.RecalculateRowsAndColumns(false);

            return newTable;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            for (var row = 0; row < _height; row++)
            {
                var rowData = new object[_width];
                for (var column = 0; column < _width; column++)
                {
                    rowData[column] = _values[column, row];
                }

                yield return rowData;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var row = 0; row < _height; row++)
            {
                var rowData = new object[_width];
                for (var column = 0; column < _width; column++)
                {
                    rowData[column] = _values[column, row];
                }

                yield return rowData;
            }
        }

        private void RecalculateRowsAndColumns(bool useCurrentValues)
        {
            var maxColumns = useCurrentValues ? _columns : _width - 1;
            var maxRows = useCurrentValues ? _rows : _height - 1;

            if (maxColumns > maxRows)
            {
                RecalculateColumns(maxColumns, maxRows);
                RecalculateRows(useCurrentValues);
            }
            else
            {
                RecalculateRows(maxRows, maxColumns);
                RecalculateColumns(useCurrentValues);
            }
        }

        private void RecalculateRows(bool useCurrentValues)
        {
            if (useCurrentValues)
            {
                RecalculateRows(_rows, _columns);
            }
            else
            {
                RecalculateRows(_height - 1, _width - 1);
            }
        }

        private void RecalculateRows(int maxRows, int maxColumns)
        {
            for (var row = maxRows; row > 0; row--)
            {
                for (var column = 0; column <= maxColumns; column++)
                {
                    if (_values[column, row] is not null)
                    {
                        _rows = row;
                        return;
                    }
                }
            }

            _rows = 0;
        }

        private void RecalculateColumns(bool useCurrentValues)
        {
            if (useCurrentValues)
            {
                RecalculateColumns(_columns, _rows);
            }
            else
            {
                RecalculateColumns(_width - 1, _height - 1);
            }
        }

        private void RecalculateColumns(int maxColumns, int maxRows)
        {
            for (var column = maxColumns; column > 0; column--)
            {
                for (var row = 0; row <= maxRows; row++)
                {
                    if (_values[column, row] is not null)
                    {
                        _columns = column;
                        return;
                    }
                }
            }

            _columns = 0;
        }
    }
}