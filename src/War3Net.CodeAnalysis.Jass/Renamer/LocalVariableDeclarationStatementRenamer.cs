// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameLocalVariableDeclarationStatement(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedLocalVariableDeclarationStatement)
        {
            _localVariableNames.Add(localVariableDeclarationStatement.Declarator.GetIdentifierName().Token.Text);

            if (TryRenameVariableOrArrayDeclarator(localVariableDeclarationStatement.Declarator, out var renamedDeclarator))
            {
                renamedLocalVariableDeclarationStatement = new JassLocalVariableDeclarationStatementSyntax(
                    localVariableDeclarationStatement.LocalToken,
                    renamedDeclarator);

                return true;
            }

            renamedLocalVariableDeclarationStatement = null;
            return false;
        }
    }
}