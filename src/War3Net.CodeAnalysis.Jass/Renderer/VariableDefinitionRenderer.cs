// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionRenderer.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System.IO;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(VariableDefinitionSyntax variableDefinition)
        {
            Render(variableDefinition.TypeNameNode);
            WriteSpace();
            Render(variableDefinition.IdentifierNameNode);
            if (variableDefinition.EmptyEqualsValueClause is null)
            {
                Render(variableDefinition.EqualsValueClause);
            }
        }
    }
}