// ------------------------------------------------------------------------------
// <copyright file="UTF8EncodingProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

namespace War3Net.Common.Providers
{
    public static class UTF8EncodingProvider
    {
        private static readonly Lazy<Encoding> _utf8 = new(new UTF8Encoding(false, false));
        private static readonly Lazy<Encoding> _strictUtf8 = new(new UTF8Encoding(false, true));

        /// <summary>
        /// Gets an <see cref="UTF8Encoding"/> that does not use a byte order mark.
        /// </summary>
        public static Encoding UTF8 => _utf8.Value;

        /// <summary>
        /// Gets an <see cref="UTF8Encoding"/> that does not use a byte order mark, and throws on invalid bytes.
        /// </summary>
        public static Encoding StrictUTF8 => _strictUtf8.Value;
    }
}