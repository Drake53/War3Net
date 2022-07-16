// ------------------------------------------------------------------------------
// <copyright file="VJassImportScriptTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassImportScriptTypeSyntax : VJassSyntaxNode
    {
        public static readonly VJassImportScriptTypeSyntax VJass = new(new(new VJassSyntaxToken(VJassSyntaxKind.VJassKeyword, VJassKeyword.VJass, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassImportScriptTypeSyntax Zinc = new(new(new VJassSyntaxToken(VJassSyntaxKind.ZincKeyword, VJassKeyword.Zinc, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassImportScriptTypeSyntax Comment = new(new(new VJassSyntaxToken(VJassSyntaxKind.CommentKeyword, VJassKeyword.Comment, VJassSyntaxTriviaList.SingleSpace)));

        internal VJassImportScriptTypeSyntax(
            VJassIdentifierNameSyntax scriptTypeName)
        {
            ScriptTypeName = scriptTypeName;
        }

        public VJassIdentifierNameSyntax ScriptTypeName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassImportScriptTypeSyntax importScriptType
                && ScriptTypeName.IsEquivalentTo(importScriptType.ScriptTypeName);
        }

        public override void WriteTo(TextWriter writer)
        {
            ScriptTypeName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ScriptTypeName.ProcessTo(writer, context);
        }

        public override string ToString() => ScriptTypeName.ToString();

        public override VJassSyntaxToken GetFirstToken() => ScriptTypeName.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => ScriptTypeName.GetLastToken();

        protected internal override VJassImportScriptTypeSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassImportScriptTypeSyntax(ScriptTypeName.ReplaceFirstToken(newToken));
        }

        protected internal override VJassImportScriptTypeSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassImportScriptTypeSyntax(ScriptTypeName.ReplaceLastToken(newToken));
        }
    }
}