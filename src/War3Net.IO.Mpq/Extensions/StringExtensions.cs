// ------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Mpq.Extensions
{
    internal static class StringExtensions
    {
        internal static ulong GetStringHash(this string s) => MpqHash.GetHashedFileName(s);
    }
}