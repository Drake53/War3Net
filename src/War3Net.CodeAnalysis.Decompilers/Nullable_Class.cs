// ------------------------------------------------------------------------------
// <copyright file="DecompilationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Decompilers
{
    public class Nullable_Class<T> where T : struct
    {
        private Nullable<T> _value { get; }
        public T Value
        {
            get
            {
                return _value.Value;
            }
        }

        public Nullable_Class(T value)
            : base()
        {
            _value = new Nullable<T>(value);
        }

        public bool HasValue
        {
            get
            {
                return _value.HasValue;
            }
        }

        public T GetValueOrDefault(T defaultValue = default)
        {
            return _value ?? defaultValue;
        }

        public override string ToString()
        {
            return _value?.ToString();
        }

        public override bool Equals(object? other)
        {
            return _value.Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator Nullable_Class<T>(T value)
        {
            return new Nullable_Class<T>(value);
        }

        public static explicit operator T(Nullable_Class<T> value)
        {
            return value.Value;
        }

        public static implicit operator Nullable_Class<T>(Nullable<T> value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return new Nullable_Class<T>(value.Value);
        }

        public static implicit operator Nullable<T>(Nullable_Class<T> value)
        {
            if (value == null || !value.HasValue)
            {
                return null;
            }

            return new Nullable<T>(value.Value);
        }
    }
}