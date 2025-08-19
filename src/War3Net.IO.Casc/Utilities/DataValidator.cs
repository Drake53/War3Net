// ------------------------------------------------------------------------------
// <copyright file="DataValidator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Provides data validation utilities for CASC operations.
    /// </summary>
    public static class DataValidator
    {
        /// <summary>
        /// Validates that a size value is within acceptable bounds.
        /// </summary>
        /// <param name="size">The size to validate.</param>
        /// <param name="maxSize">The maximum allowed size.</param>
        /// <param name="paramName">The parameter name for error reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when size exceeds maximum.</exception>
        public static void ValidateSize(long size, long maxSize, string paramName)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Size cannot be negative: {size}");
            }

            if (size > maxSize)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Size {size} exceeds maximum allowed size of {maxSize} bytes");
            }
        }

        /// <summary>
        /// Validates that a buffer has sufficient capacity.
        /// </summary>
        /// <param name="buffer">The buffer to validate.</param>
        /// <param name="offset">The offset in the buffer.</param>
        /// <param name="count">The number of bytes required.</param>
        /// <param name="paramName">The parameter name for error reporting.</param>
        /// <exception cref="ArgumentException">Thrown when buffer is insufficient.</exception>
        public static void ValidateBuffer(byte[] buffer, int offset, int count, string paramName)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative");
            }

            if (buffer.Length - offset < count)
            {
                throw new ArgumentException($"Buffer too small. Required: {count} bytes at offset {offset}, Available: {buffer.Length - offset} bytes", paramName);
            }
        }

        /// <summary>
        /// Validates array bounds for safe access.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="array">The array to validate.</param>
        /// <param name="index">The index to access.</param>
        /// <param name="paramName">The parameter name for error reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of bounds.</exception>
        public static void ValidateArrayBounds<T>(T[] array, int index, string paramName)
        {
            if (array == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of bounds for array of length {array.Length}");
            }
        }

        /// <summary>
        /// Validates that a value is within a specified range.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="paramName">The parameter name for error reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        public static void ValidateRange<T>(T value, T min, T max, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Value {value} is outside the valid range [{min}, {max}]");
            }
        }
    }
}