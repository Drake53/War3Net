// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalsDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedGlobalsDeclarationSyntax : IScopedDeclarationSyntax
    {
        public VJassScopedGlobalsDeclarationSyntax(VJassScopedGlobalDeclarationListSyntax globals)
        {
            Globals = globals;
        }

        public VJassScopedGlobalDeclarationListSyntax Globals { get; }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassScopedGlobalsDeclarationSyntax scopedGlobalsDeclaration
                && Globals.Equals(scopedGlobalsDeclaration.Globals);
        }

        public override string ToString() => $"{VJassKeyword.Globals} [{Globals.Globals.Length}]";
    }
}