﻿// ------------------------------------------------------------------------------
// <copyright file="JassEndFunctionCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEndFunctionCustomScriptAction : IStatementLineSyntax
    {
        public static readonly JassEndFunctionCustomScriptAction Value = new JassEndFunctionCustomScriptAction();

        private JassEndFunctionCustomScriptAction()
        {
        }

        public bool Equals(IStatementLineSyntax? other) => other is JassEndFunctionCustomScriptAction;

        public override string ToString() => JassKeyword.EndFunction;
    }
}