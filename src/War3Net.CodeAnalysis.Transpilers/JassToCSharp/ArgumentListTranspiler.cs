// ------------------------------------------------------------------------------
// <copyright file="ArgumentListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<ArgumentSyntax> Transpile(this Jass.Syntax.ArgumentListSyntax argumentListNode)
        {
            _ = argumentListNode ?? throw new ArgumentNullException(nameof(argumentListNode));

            return argumentListNode.Select(argumentNode => SyntaxFactory.Argument(argumentNode.Transpile()));
        }
    }
}