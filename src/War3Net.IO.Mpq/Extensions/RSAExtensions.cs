// ------------------------------------------------------------------------------
// <copyright file="RSAExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Cryptography;

#if NETSTANDARD2_0
using System.IO;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
#endif

namespace War3Net.IO.Mpq.Extensions
{
    internal static class RSAExtensions
    {
#if NETSTANDARD2_0
        internal static bool VerifyMpqSignature(this RSA rsa, byte[] archiveBytes, byte[] signatureBytes, string publicKey)
        {
            rsa.ImportFromPem(publicKey, isPrivate: false);
            return rsa.VerifyData(archiveBytes, signatureBytes.Reverse().ToArray(), HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }

        // https://stackoverflow.com/a/76927316
        internal static void ImportFromPem(this RSA rsa, string input, bool isPrivate)
        {
            using var stringReader = new StringReader(input);
            var pemReader = new PemReader(stringReader);

            var key = pemReader.ReadObject();

            var rsaParameters = isPrivate
                ? DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)((AsymmetricCipherKeyPair)key).Private)
                : DotNetUtilities.ToRSAParameters((RsaKeyParameters)key);

            rsa.ImportParameters(rsaParameters);
        }
#else
        internal static bool VerifyMpqSignature(this RSA rsa, byte[] archiveBytes, byte[] signatureBytes, ReadOnlySpan<char> publicKey)
        {
            rsa.ImportFromPem(publicKey);
            return rsa.VerifyData(archiveBytes, signatureBytes.Reverse().ToArray(), HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
#endif
    }
}