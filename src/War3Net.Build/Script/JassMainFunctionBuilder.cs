// ------------------------------------------------------------------------------
// <copyright file="JassMainFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Net.Build.Providers.MainFunctionStatementsProvider<
    War3Net.Build.Script.JassMainFunctionBuilder,
    War3Net.CodeAnalysis.Jass.Syntax.FunctionSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.NewStatementSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.NewExpressionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class JassMainFunctionBuilder : JassFunctionBuilder
    {
        public JassMainFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override FunctionSyntax Build()
        {
            return Build(GetMainFunctionName, GetStatements(this).ToArray());
        }

        protected override LocalVariableListSyntax GetLocalDeclarations()
        {
            return JassSyntaxFactory.LocalVariableList(GenerateLocalDeclaration(nameof(War3Api.Common.unit), MainFunctionProvider.LocalUnitVariableName));
        }
    }
}