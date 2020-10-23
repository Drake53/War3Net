// ------------------------------------------------------------------------------
// <copyright file="ArgumentListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<ArgumentSyntax> Transpile(this Syntax.ArgumentListSyntax argumentListNode)
        {
            _ = argumentListNode ?? throw new ArgumentNullException(nameof(argumentListNode));

            return argumentListNode.Select(argumentNode => SyntaxFactory.Argument(argumentNode.Transpile()));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ArgumentListSyntax argumentListNode, ref StringBuilder sb)
        {
            _ = argumentListNode ?? throw new ArgumentNullException(nameof(argumentListNode));

            var firstArgument = true;
            foreach (var argumentNode in argumentListNode)
            {
                if (firstArgument)
                {
                    firstArgument = false;
                }
                else
                {
                    sb.Append(", ");
                }

                argumentNode.Transpile(ref sb);
            }
        }
    }
}