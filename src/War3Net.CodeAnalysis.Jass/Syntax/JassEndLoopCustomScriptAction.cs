﻿// ------------------------------------------------------------------------------
// <copyright file="JassEndLoopCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEndLoopCustomScriptAction : IStatementLineSyntax
    {
        public static readonly JassEndLoopCustomScriptAction Value = new JassEndLoopCustomScriptAction();

        private JassEndLoopCustomScriptAction()
        {
        }

        public bool Equals(IStatementLineSyntax? other) => other is JassEndLoopCustomScriptAction;

        public override string ToString() => JassKeyword.EndLoop;
    }
}