﻿// ------------------------------------------------------------------------------
// <copyright file="StatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileActionStatement(
            JassStatementListSyntax statementList,
            ref int i,
            ref List<TriggerFunction> functions)
        {
            return statementList.Statements[i] switch
            {
                JassCommentSyntax comment => TryDecompileComment(comment, ref functions),
                JassSetStatementSyntax setStatement => TryDecompileSetStatement(setStatement, statementList, ref i, ref functions),
                JassCallStatementSyntax callStatement => TryDecompileCallStatement(callStatement, ref functions),
                JassIfStatementSyntax ifStatement => TryDecompileIfStatement(ifStatement, ref functions),
                JassLoopStatementSyntax loopStatement => TryDecompileLoopStatement(loopStatement, ref functions),
                JassReturnStatementSyntax returnStatement => TryDecompileReturnStatement(returnStatement, ref functions),

                _ => false,
            };
        }

        /// <param name="returnValue"><see langword="true"/> for AND conditions, <see langword="false"/> for OR conditions.</param>
        private bool TryDecompileConditionStatement(
            IStatementSyntax statement,
            bool returnValue,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            function = null;

            return statement switch
            {
                JassIfStatementSyntax ifStatement => TryDecompileIfStatement(ifStatement, returnValue, out function),
                JassReturnStatementSyntax returnStatement => TryDecompileReturnStatement(returnStatement, out function),

                _ => false,
            };
        }
    }
}