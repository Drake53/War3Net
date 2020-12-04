// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static GlobalsDeclarationListSyntax GlobalsDeclarationList(params GlobalDeclarationSyntax[] globals)
        {
            return globals.Any() ? new GlobalsDeclarationListSyntax(globals) : new GlobalsDeclarationListSyntax(Empty());
        }

        public static GlobalsDeclarationListSyntax GlobalsDeclarationList(IEnumerable<GlobalDeclarationSyntax> globals)
        {
            return globals.Any() ? new GlobalsDeclarationListSyntax(globals.ToArray()) : new GlobalsDeclarationListSyntax(Empty());
        }

        public static NewDeclarationSyntax GlobalDeclarations(params GlobalDeclarationSyntax[] globals)
        {
            return GlobalDeclarations(GlobalsDeclarationList(globals));
        }

        public static NewDeclarationSyntax GlobalDeclarations(IEnumerable<GlobalDeclarationSyntax> globals)
        {
            return GlobalDeclarations(GlobalsDeclarationList(globals.ToArray()));
        }

        public static NewDeclarationSyntax GlobalDeclarations(IEnumerable<GlobalDeclarationSyntax> globals, LineDelimiterSyntax? afterGlobals, LineDelimiterSyntax? afterEndglobals)
        {
            return GlobalDeclarations(GlobalsDeclarationList(globals.ToArray()), afterGlobals, afterEndglobals);
        }

        private static NewDeclarationSyntax GlobalDeclarations(
            GlobalsDeclarationListSyntax globals,
            LineDelimiterSyntax? afterGlobals = null,
            LineDelimiterSyntax? afterEndglobals = null)
        {
            return new NewDeclarationSyntax(
                new DeclarationSyntax(new GlobalsBlockSyntax(
                    Token(SyntaxTokenType.GlobalsKeyword),
                    afterGlobals ?? Newlines(),
                    globals,
                    Token(SyntaxTokenType.EndglobalsKeyword))),
                afterEndglobals ?? Newlines());
        }
    }
}