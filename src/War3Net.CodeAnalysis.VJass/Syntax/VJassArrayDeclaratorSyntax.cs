// ------------------------------------------------------------------------------
// <copyright file="VJassArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassArrayDeclaratorSyntax : VJassVariableOrArrayDeclaratorSyntax
    {
        internal VJassArrayDeclaratorSyntax(
            VJassTypeSyntax type,
            VJassSyntaxToken arrayToken,
            VJassIdentifierNameSyntax identifierName)
        {
            Type = type;
            ArrayToken = arrayToken;
            IdentifierName = identifierName;
        }

        public override VJassTypeSyntax Type { get; }

        public VJassSyntaxToken ArrayToken { get; }

        public override VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassArrayDeclaratorSyntax arrayDeclarator
                && Type.IsEquivalentTo(arrayDeclarator.Type)
                && IdentifierName.IsEquivalentTo(arrayDeclarator.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            ArrayToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => $"{Type} {ArrayToken} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassArrayDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassArrayDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                ArrayToken,
                IdentifierName);
        }

        protected internal override VJassArrayDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassArrayDeclaratorSyntax(
                Type,
                ArrayToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}