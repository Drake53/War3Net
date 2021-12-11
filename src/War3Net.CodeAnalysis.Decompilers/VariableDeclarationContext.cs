// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal class VariableDeclarationContext
    {
        public VariableDeclarationContext(JassGlobalDeclarationSyntax globalDeclaration)
        {
            GlobalDeclaration = globalDeclaration;
            IsArray = globalDeclaration.Declarator is JassArrayDeclaratorSyntax;

            Type = globalDeclaration.Declarator.Type.TypeName.Name;
        }

        public JassGlobalDeclarationSyntax GlobalDeclaration { get; }

        public bool IsArray { get; }

        public VariableDefinition? VariableDefinition { get; set; }

        public string Type { get; set; }

        public bool Handled { get; set; }
    }
}