// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax Transpile(this Jass.Syntax.GlobalVariableDeclarationSyntax globalVariableDeclarationNode)
        {
            _ = globalVariableDeclarationNode ?? throw new ArgumentNullException(nameof(globalVariableDeclarationNode));

            return globalVariableDeclarationNode.DeclarationNode.TranspileMember();
        }
    }
}