// ------------------------------------------------------------------------------
// <copyright file="CommentDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileComment(
            JassCommentSyntax comment,
            ref List<TriggerFunction> functions)
        {
            if (comment.Comment.Length > 1 && comment.Comment.StartsWith(' '))
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
                            Value = comment.Comment[1..],
                        },
                    },
                });

                return true;
            }

            return false;
        }
    }
}