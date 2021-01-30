// ------------------------------------------------------------------------------
// <copyright file="TypeTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public TypeSyntax Transpile(JassTypeSyntax type)
        {
            if (type.Equals(JassTypeSyntax.Boolean))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));
            }

            if (type.Equals(JassTypeSyntax.Code))
            {
                return SyntaxFactory.ParseTypeName(typeof(Action).FullName!);
            }

            if (type.Equals(JassTypeSyntax.Handle))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));
            }

            if (type.Equals(JassTypeSyntax.Integer))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));
            }

            if (type.Equals(JassTypeSyntax.Nothing))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
            }

            if (type.Equals(JassTypeSyntax.Real))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword));
            }

            if (type.Equals(JassTypeSyntax.String))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
            }

            return SyntaxFactory.ParseTypeName(Transpile(type.TypeName).Text);
        }
    }
}