// ------------------------------------------------------------------------------
// <copyright file="StatementRenderer.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(StatementSyntax statement)
        {
            if (statement.SetStatementNode != null)
            {
                Render(statement.SetStatementNode);
            }
            else if (statement.CallStatementNode != null)
            {
                Render(statement.CallStatementNode);
            }
            else if (statement.IfStatementNode != null)
            {
                Render(statement.IfStatementNode);
            }
            else if (statement.LoopStatementNode != null)
            {
                Render(statement.LoopStatementNode);
            }
            else if (statement.ExitStatementNode != null)
            {
                Render(statement.ExitStatementNode);
            }
            else if (statement.ReturnStatementNode != null)
            {
                Render(statement.ReturnStatementNode);
            }
            else
            {
                Render(statement.DebugStatementNode);
            }
        }
    }
}