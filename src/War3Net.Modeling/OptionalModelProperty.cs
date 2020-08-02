// ------------------------------------------------------------------------------
// <copyright file="OptionalModelProperty.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;

using War3Net.Common.Extensions;
using War3Net.Modeling.DataStructures;

namespace War3Net.Modeling
{
    internal sealed class OptionalModelProperty
    {
        private readonly int _tag;
        private readonly PropertyInfo _propertyInfo;
        private readonly Func<BinaryReader, object> _parser;

        public OptionalModelProperty(int tag, string propertyName, Func<BinaryReader, object> parser)
            : this(typeof(Model), propertyName, tag, parser)
        {
        }

        public OptionalModelProperty(Type type, string propertyName, int tag, Func<BinaryReader, object> parser)
        {
            _tag = tag;
            _propertyInfo = type.GetProperty(propertyName);
            _parser = parser;
        }

        public int Tag => _tag;

        public string TagAsString => _tag.ToRawcode();

        public void ParseForObject(object obj, BinaryReader reader)
        {
            if (_propertyInfo.GetValue(obj) != null)
            {
                throw new ArgumentException("Property has already been set.", nameof(obj));
            }

            _propertyInfo.SetValue(obj, _parser.Invoke(reader));
        }
    }
}