// ------------------------------------------------------------------------------
// <copyright file="TypeTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public TypeSyntax Transpile(JassTypeSyntax type)
        {
            return type.GetToken().SyntaxKind switch
            {
                JassSyntaxKind.BooleanKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                JassSyntaxKind.CodeKeyword => SyntaxFactory.ParseTypeName(typeof(Action).FullName!),
                JassSyntaxKind.HandleKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                JassSyntaxKind.IntegerKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                JassSyntaxKind.NothingKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                JassSyntaxKind.RealKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword)),
                JassSyntaxKind.StringKeyword => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),

                JassSyntaxKind.IdentifierToken => SyntaxFactory.ParseTypeName(Transpile((JassIdentifierNameSyntax)type).Text),
            };
        }
    }
}