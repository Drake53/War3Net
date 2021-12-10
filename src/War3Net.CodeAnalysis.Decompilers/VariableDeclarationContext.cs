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