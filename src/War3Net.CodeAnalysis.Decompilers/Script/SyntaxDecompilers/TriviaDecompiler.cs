// ------------------------------------------------------------------------------
// <copyright file="TriviaDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileComment(
            JassSyntaxTrivia trivia,
            ref List<TriggerFunction> functions)
        {
            if (trivia.Text.Length > 3 && trivia.Text.StartsWith("// ", StringComparison.Ordinal))
            {
                functions.Add(new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "CommentString",
                    Parameters = new()
                    {
                        new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.String,
                            Value = trivia.Text[3..],
                        },
                    },
                });

                return true;
            }

            return false;
        }
    }
}