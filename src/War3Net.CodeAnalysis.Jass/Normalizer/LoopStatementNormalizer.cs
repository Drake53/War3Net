﻿// ------------------------------------------------------------------------------
// <copyright file="LoopStatementNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteLoopStatement(JassLoopStatementSyntax loopStatement, out JassStatementSyntax result)
        {
            _nodes.Add(loopStatement);
            var normalized = base.RewriteLoopStatement(loopStatement, out result);
            _nodes.RemoveAt(_nodes.Count - 1);

            return normalized;
        }
    }
}