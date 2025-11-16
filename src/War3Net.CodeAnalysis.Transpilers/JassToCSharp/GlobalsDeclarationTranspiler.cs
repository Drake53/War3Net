// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IEnumerable<MemberDeclarationSyntax> Transpile(JassGlobalsDeclarationSyntax globalsDeclaration)
        {
            var declarations = globalsDeclaration.GlobalDeclarations;
            for (var i = 0; i < declarations.Length; i++)
            {
                if (i == 0)
                {
                    if (i + 1 == declarations.Length)
                    {
                        yield return Transpile(
                            MergeTrivia(globalsDeclaration.GlobalsToken, declarations[i].GetLeadingTrivia()),
                            declarations[i],
                            MergeTrivia(declarations[i].GetTrailingTrivia(), globalsDeclaration.EndGlobalsToken));
                    }
                    else
                    {
                        yield return Transpile(
                            MergeTrivia(globalsDeclaration.GlobalsToken, declarations[i].GetLeadingTrivia()),
                            declarations[i]);
                    }
                }
                else if (i + 1 == declarations.Length)
                {
                    yield return Transpile(
                        declarations[i],
                        MergeTrivia(declarations[i].GetTrailingTrivia(), globalsDeclaration.EndGlobalsToken));
                }
                else
                {
                    yield return Transpile(declarations[i]);
                }
            }
        }
    }
}