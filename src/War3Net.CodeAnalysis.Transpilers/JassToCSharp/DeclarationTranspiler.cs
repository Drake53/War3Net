// ------------------------------------------------------------------------------
// <copyright file="DeclarationTranspiler.cs" company="Drake53">
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
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Jass.Syntax.DeclarationSyntax declarationNode)
        {
            _ = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            if (declarationNode.GlobalsBlock != null)
            {
                foreach (var globalDeclaration in declarationNode.GlobalsBlock.Transpile())
                {
                    yield return globalDeclaration;
                }
            }
            else if (declarationNode.TypeDefinition != null)
            {
                foreach (var typeDefinition in declarationNode.TypeDefinition.Transpile())
                {
                    yield return typeDefinition;
                }
            }
            else
            {
                yield return declarationNode.NativeFunctionDeclaration.Transpile();
            }
        }
    }
}