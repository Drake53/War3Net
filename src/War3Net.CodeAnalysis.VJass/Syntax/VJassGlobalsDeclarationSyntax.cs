// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalsDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalsDeclarationSyntax : ITopLevelDeclarationSyntax
    {
        public VJassGlobalsDeclarationSyntax(VJassGlobalDeclarationListSyntax globals)
        {
            Globals = globals;
        }

        public VJassGlobalDeclarationListSyntax Globals { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassGlobalsDeclarationSyntax globalsDeclaration
                && Globals.Equals(globalsDeclaration.Globals);
        }

        public override string ToString() => $"{VJassKeyword.Globals} [{Globals.Globals.Length}]";
    }
}