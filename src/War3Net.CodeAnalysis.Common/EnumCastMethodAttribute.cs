// ------------------------------------------------------------------------------
// <copyright file="EnumCastMethodAttribute.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Common
{
    /// <summary>
    /// Indicates that this enum type cannot be cast to or from an integer, neither implicitly nor explicitly.
    /// Instead, there is a one-way conversion from integer to this enum type, defined by <see cref="CastFunctionName"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public class EnumCastMethodAttribute : Attribute
    {
        private readonly string _func;

        public EnumCastMethodAttribute(string castFunction)
        {
            _func = castFunction;
        }

        public string CastFunctionName => _func;
    }
}