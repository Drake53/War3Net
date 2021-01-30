// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, IEnumerable<IStatementSyntax> statements)
        {
            return new JassFunctionDeclarationSyntax(functionDeclarator, StatementList(statements));
        }

        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, params IStatementSyntax[] statements)
        {
            return new JassFunctionDeclarationSyntax(functionDeclarator, StatementList(statements));
        }
    }
}