// ------------------------------------------------------------------------------
// <copyright file="FileTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this FileSyntax fileNode, ref StringBuilder sb, bool resetStringConcatenationHandler = true)
        {
            _ = fileNode ?? throw new ArgumentNullException(nameof(fileNode));

            foreach (var declaration in fileNode.DeclarationList)
            {
                declaration.Transpile(ref sb);
            }

            foreach (var function in fileNode.FunctionList)
            {
                function.Transpile(ref sb);
            }

            if (resetStringConcatenationHandler)
            {
                TranspileStringConcatenationHandler.Reset();
            }
        }
    }
}