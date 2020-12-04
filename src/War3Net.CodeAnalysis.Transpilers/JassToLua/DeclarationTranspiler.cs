// ------------------------------------------------------------------------------
// <copyright file="DeclarationTranspiler.cs" company="Drake53">
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
        public static void Transpile(this DeclarationSyntax declarationNode, ref StringBuilder sb)
        {
            _ = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            declarationNode.GlobalsBlock?.Transpile(ref sb);
            declarationNode.TypeDefinition?.Transpile(ref sb);
            declarationNode.NativeFunctionDeclaration?.Transpile(ref sb);
        }
    }
}