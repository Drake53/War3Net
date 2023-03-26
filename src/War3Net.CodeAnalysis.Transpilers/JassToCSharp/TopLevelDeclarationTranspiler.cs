// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IEnumerable<MemberDeclarationSyntax> Transpile(JassTopLevelDeclarationSyntax declaration)
        {
            return declaration switch
            {
                JassTypeDeclarationSyntax typeDeclaration => new[] { Transpile(typeDeclaration) },
                JassGlobalsDeclarationSyntax globalDeclarationList => Transpile(globalDeclarationList),
                JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration => new[] { Transpile(nativeFunctionDeclaration) },
                JassFunctionDeclarationSyntax functionDeclaration => new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<MemberDeclarationSyntax>(),
            };
        }
    }
}