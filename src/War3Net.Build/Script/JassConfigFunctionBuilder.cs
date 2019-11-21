// ------------------------------------------------------------------------------
// <copyright file="JassConfigFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Net.Build.Providers.ConfigFunctionStatementsProvider<
    War3Net.Build.Script.JassConfigFunctionBuilder,
    War3Net.CodeAnalysis.Jass.Syntax.FunctionSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.NewStatementSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.NewExpressionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class JassConfigFunctionBuilder : JassFunctionBuilder, IConfigFunctionBuilder<NewStatementSyntax>
    {
        public JassConfigFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public string LobbyMusic { get; set; }

        public override FunctionSyntax Build()
        {
            return Build(GetConfigFunctionName, GetStatements(this).ToArray());
        }

        protected override LocalVariableListSyntax GetLocalDeclarations()
        {
            return JassSyntaxFactory.LocalVariableList();
        }
    }
}