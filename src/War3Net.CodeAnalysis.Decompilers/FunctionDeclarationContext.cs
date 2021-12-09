using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal class FunctionDeclarationContext
    {
        public FunctionDeclarationContext(JassFunctionDeclarationSyntax functionDeclaration, IEnumerable<JassCommentDeclarationSyntax> comments)
        {
            FunctionDeclaration = functionDeclaration;
            Comments = comments.ToImmutableList();
            Handled = false;
        }

        public JassFunctionDeclarationSyntax FunctionDeclaration { get; }

        public ImmutableList<JassCommentDeclarationSyntax> Comments { get; }

        public bool Handled { get; set; }
    }
}