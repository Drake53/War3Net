// ------------------------------------------------------------------------------
// <copyright file="FileTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Syntax.FileSyntax fileNode, bool apiOnly = false)
        {
            _ = fileNode ?? throw new ArgumentNullException(nameof(fileNode));

            foreach (var declaration in fileNode.DeclarationList.Transpile())
            {
                yield return declaration;
            }

            foreach (var function in fileNode.FunctionList)
            {
                if (apiOnly)
                {
                    // Transform function into native function to remove function body.
                    yield return function.AsNativeFunction().Transpile();
                }
                else
                {
                    yield return function.Transpile();
                }
            }

            foreach (var enumDeclaration in TranspileToEnumHandler.GetEnums())
            {
                yield return enumDeclaration;
            }

            TranspileToEnumHandler.Reset();
        }
    }
}