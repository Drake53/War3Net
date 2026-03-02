using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
        {
            if (nativeFunctionDeclaration.ConstantToken is not null)
            {
                Render(nativeFunctionDeclaration.ConstantToken);
                WriteSpace();
            }

            Render(nativeFunctionDeclaration.NativeToken);
            WriteSpace();
            Render(nativeFunctionDeclaration.IdentifierName);
            WriteSpace();
            Render(nativeFunctionDeclaration.ParameterList);
            WriteSpace();
            Render(nativeFunctionDeclaration.ReturnClause);
        }
    }
}