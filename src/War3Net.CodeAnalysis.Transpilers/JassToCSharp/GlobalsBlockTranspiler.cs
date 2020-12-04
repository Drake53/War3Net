// ------------------------------------------------------------------------------
// <copyright file="GlobalsBlockTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Jass.Syntax.GlobalsBlockSyntax globalsBlockNode)
        {
            _ = globalsBlockNode ?? throw new ArgumentNullException(nameof(globalsBlockNode));

            foreach (var declaration in globalsBlockNode.GlobalDeclarationListNode)
            {
                yield return declaration.Transpile();
            }
        }
    }
}