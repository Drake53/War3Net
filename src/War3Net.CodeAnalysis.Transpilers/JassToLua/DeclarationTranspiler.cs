// ------------------------------------------------------------------------------
// <copyright file="DeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this DeclarationSyntax declarationNode, ref StringBuilder sb)
        {
            _ = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            declarationNode.GlobalsBlock?.Transpile(ref sb);
            declarationNode.TypeDefinition?.Transpile(ref sb);
            declarationNode.NativeFunctionDeclaration?.Transpile(ref sb);
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this DeclarationSyntax declarationNode)
        {
            _ = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            if (declarationNode.GlobalsBlock is not null)
            {
                return declarationNode.GlobalsBlock.TranspileToLua();
            }
            else
            {
                // TypeDefinition and NativeFunctionDeclaration are not supported.
                return Array.Empty<LuaStatementSyntax>();
            }
        }
    }
}