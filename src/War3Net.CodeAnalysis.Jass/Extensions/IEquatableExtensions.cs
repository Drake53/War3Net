// ------------------------------------------------------------------------------
// <copyright file="IEquatableExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class IEquatableExtensions
    {
        public static bool NullableEquals<T>(this IEquatable<T>? objA, T? objB)
        {
            return ReferenceEquals(objA, objB) || objA?.Equals(objB) == true;
        }
    }
}