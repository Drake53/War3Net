// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public sealed class FunctionDeclarationContext
    {
        public FunctionDeclarationContext(JassFunctionDeclarationSyntax functionDeclaration, IEnumerable<JassCommentSyntax> comments)
        {
            FunctionDeclaration = functionDeclaration;
            Comments = comments.ToImmutableList();

            if (functionDeclaration.FunctionDeclarator.ParameterList.Parameters.IsEmpty)
            {
                IsActionsFunction = functionDeclaration.FunctionDeclarator.ReturnType == JassTypeSyntax.Nothing;
                IsConditionsFunction = functionDeclaration.FunctionDeclarator.ReturnType == JassTypeSyntax.Boolean;
            }

            Handled = false;
        }

        public JassFunctionDeclarationSyntax FunctionDeclaration { get; }

        public ImmutableList<JassCommentSyntax> Comments { get; }

        public bool IsActionsFunction { get; }

        public bool IsConditionsFunction { get; }

        public bool Handled { get; set; }
    }
}