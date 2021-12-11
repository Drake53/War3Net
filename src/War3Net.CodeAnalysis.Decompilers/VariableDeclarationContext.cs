// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal class VariableDeclarationContext
    {
        public VariableDeclarationContext(JassGlobalDeclarationSyntax globalDeclaration)
        {
            GlobalDeclaration = globalDeclaration;
            Type = globalDeclaration.Declarator.Type.TypeName.Name;
            IsArray = globalDeclaration.Declarator is JassArrayDeclaratorSyntax;
        }

        public JassGlobalDeclarationSyntax GlobalDeclaration { get; }

        public string Type { get; }

        public bool IsArray { get; }

        public bool Handled { get; set; }
    }
}