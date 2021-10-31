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
        /// Gets the highest row index for which at least one cell is not empty.
        /// </summary>
        public int Rows => _rows;

        /// <summary>
        /// Gets the highest column index for which at least one cell is not empty.
        /// </summary>
        public int Columns => _columns;

        /// <summary>
        /// Gets or sets the content of a single cell in the <see cref="SylkTable"/>.
        /// </summary>
        public object this[int column, int row]
        {
            get => _values[column, row];
            set
            {
                if (column < 0 || column + 1 > _width)
                {
                    throw new ArgumentOutOfRangeException(nameof(column));
                }

                if (row < 0 || row + 1 > _height)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }

                _values[column, row] = value;
                if (value is null)
                {
                    // TODO: recalculate _rows and _columns
                }
                else
                {
                    _rows = _rows < row + 1 ? row : _rows;
                    _columns = _columns < column + 1 ? column : _columns;
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

        public SylkTable Shrink()
        {
            if (_columns == _width && _rows == _height)
            {
                return this;
            }

            var newTable = new SylkTable(_columns, _rows+1);
            for (var row = 0; row <= _rows; row++)
            {
                for (var column = 0; column < _columns; column++)
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

        public SylkTable Combine(SylkTable other, object thisColumn, object otherColumn, object newColumn)
        {
            var newTable = new SylkTable(_columns + other._columns - 1, _rows + other._rows - 1);

            var thisColumnInt = this[thisColumn].Single();
            var otherColumnInt = other[otherColumn].Single();

            var thisRowMap = new Dictionary<object, int>();
            for (var row = 1; row <= _rows; row++)
            {
                thisRowMap.Add(_values[thisColumnInt, row], row);
            }

            var otherRow = _rows;
            var otherRowMap = new Dictionary<object, int>();
            for (var row = 1; row <= other._rows; row++)
            {
                var key = other._values[otherColumnInt, row];
                otherRowMap.Add(key, thisRowMap.TryGetValue(key, out var thisRow) ? thisRow : otherRow++);
            }

            newTable._rows = otherRow;
            newTable._columns = newTable._width;

            for (var row = 0; row <= _rows; row++)
            {
                var key = _values[thisColumnInt, row];
                for (var column = 0; column < _columns; column++)
                {
                    if (row == 0)
                    {
                        newTable._values[column, 0] = _values[column, 0];
                    }
                    else
                    {
                        newTable._values[column, thisRowMap[key]] = _values[column, row];
                    }
                }
            }

            for (var row = 0; row <= other._rows; row++)
            {
                var key = other._values[otherColumnInt, row];
                var seenOtherColumn = false;
                for (var column = 0; column < other._columns; column++)
                {
                    if (column == otherColumnInt)
                    {
                        if (row != 0)
                        {
                            var thisRowName = newTable._values[thisColumnInt, otherRowMap[key]];
                            var otherRowName = other._values[column, row];
                            if (!Equals(thisRowName, otherRowName))
                            {
                                if (otherRowMap[key] < _rows)
                                {
                                    throw new Exception();
                                }

                                newTable._values[thisColumnInt, otherRowMap[key]] = other._values[column, row];
                            }
                        }

                        seenOtherColumn = true;
                        continue;
                    }
                    else if (row == 0)
                    {
                        newTable._values[column + _columns + (seenOtherColumn ? -1 : 0), 0] = other._values[column, 0];
                    }
                    else
                    {
                        newTable._values[column + _columns + (seenOtherColumn ? -1 : 0), otherRowMap[key]] = other._values[column, row];
                    }
                }
            }

            _values[thisColumnInt, 0] = newColumn;

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
    }
}