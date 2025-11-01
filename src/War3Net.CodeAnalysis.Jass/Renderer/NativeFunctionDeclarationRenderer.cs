// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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