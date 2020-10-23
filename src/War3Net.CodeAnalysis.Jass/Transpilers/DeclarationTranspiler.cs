// ------------------------------------------------------------------------------
// <copyright file="DeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Syntax.DeclarationSyntax declarationNode)
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

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.DeclarationSyntax declarationNode, ref StringBuilder sb)
        {
            _ = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            declarationNode.GlobalsBlock?.Transpile(ref sb);
            declarationNode.TypeDefinition?.Transpile(ref sb);
            declarationNode.NativeFunctionDeclaration?.Transpile(ref sb);
        }
    }
}