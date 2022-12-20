// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract partial class ObjectDataModification
    {
        internal ObjectDataModification()
        {
        }

        public int Id { get; set; }

        public ObjectDataType Type { get; set; }

        public object Value { get; set; }

        internal int SanityCheck { get; set; }

        public int ValueAsInt => Value is int i ? i : throw new InvalidOperationException();

        public float ValueAsFloat => Value is float f ? f : throw new InvalidOperationException();

        public string ValueAsString => Value is string s ? s : throw new InvalidOperationException();

        [Obsolete]
        public bool ValueAsBool => Value is bool b ? b : throw new InvalidOperationException();

        [Obsolete]
        public char ValueAsChar => Value is char c ? c : throw new InvalidOperationException();

        public override string ToString() => Id.ToRawcode();
    }
}