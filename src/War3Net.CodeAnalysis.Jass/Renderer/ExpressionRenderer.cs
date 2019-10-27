// ------------------------------------------------------------------------------
// <copyright file="ExpressionRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(ExpressionSyntax expression)
        {
            if (expression.UnaryExpression != null)
            {
                Render(expression.UnaryExpression);
            }
            else if (expression.FunctionCall != null)
            {
                Render(expression.FunctionCall);
            }
            else if (expression.ArrayReference != null)
            {
                Render(expression.ArrayReference);
            }
            else if (expression.FunctionReference != null)
            {
                Render(expression.FunctionReference);
            }
            else if (expression.Identifier != null)
            {
                if (_options.InlineConstants && _constants.ContainsKey(expression.Identifier.ValueText))
                {
                    Render(_constants[expression.Identifier.ValueText]);
                }
                else
                {
                    Render(expression.Identifier);
                }
            }
            else if (expression.ConstantExpression != null)
            {
                Render(expression.ConstantExpression);
            }
            else
            {
                Render(expression.ParenthesizedExpressionSyntax);
            }
        }
    }
}