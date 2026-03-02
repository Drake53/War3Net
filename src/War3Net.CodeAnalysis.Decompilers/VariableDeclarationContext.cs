namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class VariableDeclarationContext
    {
        public VariableDeclarationContext(JassGlobalVariableDeclarationSyntax globalVariableDeclaration)
        {
            GlobalVariableDeclaration = globalVariableDeclaration;
            IsArray = globalVariableDeclaration.Declarator is JassArrayDeclaratorSyntax;

            Type = globalVariableDeclaration.Declarator.GetVariableType().GetToken().Text;
        }

        public JassGlobalVariableDeclarationSyntax GlobalVariableDeclaration { get; }

        public bool IsArray { get; }

        public VariableDefinition? VariableDefinition { get; set; }

        public string Type { get; set; }

        public bool Handled { get; set; }
    }
}