// ------------------------------------------------------------------------------
// <copyright file="RSAExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Cryptography;

namespace War3Net.IO.Mpq.Extensions
{
    internal static class RSAExtensions
    {
        internal static bool VerifyMpqSignature(this RSA rsa, byte[] archiveBytes, byte[] signatureBytes, ReadOnlySpan<char> publicKey)
        {
            rsa.ImportFromPem(publicKey);
            return rsa.VerifyData(archiveBytes, signatureBytes.Reverse().ToArray(), HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
    }
}