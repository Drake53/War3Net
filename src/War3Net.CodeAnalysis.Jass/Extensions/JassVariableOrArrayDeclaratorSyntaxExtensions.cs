namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassVariableOrArrayDeclaratorSyntaxExtensions
    {
        public static JassIdentifierNameSyntax GetIdentifierName(this JassVariableOrArrayDeclaratorSyntax declarator)
        {
            if (declarator is JassVariableDeclaratorSyntax variableDeclarator)
            {
                return variableDeclarator.IdentifierName;
            }
            else if (declarator is JassArrayDeclaratorSyntax arrayDeclarator)
            {
                return arrayDeclarator.IdentifierName;
            }
            else
            {
                throw new ArgumentException("Unknown declarator type.", nameof(declarator));
            }
        }

        public static JassTypeSyntax GetVariableType(this JassVariableOrArrayDeclaratorSyntax declarator)
        {
            if (declarator is JassVariableDeclaratorSyntax variableDeclarator)
            {
                return variableDeclarator.Type;
            }
            else if (declarator is JassArrayDeclaratorSyntax arrayDeclarator)
            {
                return arrayDeclarator.Type;
            }
            else
            {
                throw new ArgumentException("Unknown declarator type.", nameof(declarator));
            }
        }
    }
}