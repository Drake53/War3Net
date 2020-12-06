// ------------------------------------------------------------------------------
// <copyright file="TypeTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this TypeSyntax typeNode, ref StringBuilder sb)
        {
            // _ = typeNode ?? throw new ArgumentNullException(nameof(typeNode));

            throw new NotSupportedException();
        }

        public static void TranspileToLua(this TypeSyntax typeNode)
        {
            // _ = typeNode ?? throw new ArgumentNullException(nameof(typeNode));

            throw new NotSupportedException();
        }
    }
}