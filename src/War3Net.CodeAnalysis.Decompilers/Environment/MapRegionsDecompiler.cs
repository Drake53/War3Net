// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// 
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using Region = War3Net.Build.Environment.Region;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        [RegisterStatementParser]
        internal void ParseRegionCreation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            if (variableAssignment == null || !variableAssignment.StartsWith("gg_rct_"))
            {
                return;
            }

            var region = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "Rect")
            .SafeMapFirst(x =>
            {
                return new Region
                {
                    Left = x.Arguments.Arguments[0].GetValueOrDefault<float>(),
                    Bottom = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    Right = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    Top = x.Arguments.Arguments[3].GetValueOrDefault<float>(),
                    Color = System.Drawing.Color.FromArgb(unchecked((int)0xFF8080FF)),
                    CreationNumber = Context.GetNextCreationNumber()
                };
            });

            if (region != null)
            {
                region.Name = variableAssignment["gg_rct_".Length..].Replace('_', ' ');
                Context.Add(region, variableAssignment);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseAddWeatherEffect(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "AddWeatherEffect")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    WeatherType = (WeatherType)((JassFourCCLiteralExpressionSyntax)x.Arguments.Arguments[1]).Value.InvertEndianness(),
                };
            });

            if (match != null)
            {
                var region = Context.Get<Region>(match.VariableName) ?? Context.GetLastCreated<Region>();
                if (region != null)
                {
                    region.WeatherType = match.WeatherType;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundPosition(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundPosition")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    x = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    y = x.Arguments.Arguments[2].GetValueOrDefault<float>()
                };
            });

            if (match != null)
            {
                var region = Context.Get<Region>(match.VariableName) ?? Context.GetLastCreated<Region>();
                if (region != null)
                {
                    if (region.CenterX == match.x && region.CenterY == match.y)
                    {
                        region.AmbientSound = match.VariableName;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseRegisterStackedSound(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "RegisterStackedSound")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Width = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    Height = x.Arguments.Arguments[3].GetValueOrDefault<float>()
                };
            });

            if (match != null)
            {
                var region = Context.Get<Region>(match.VariableName) ?? Context.GetLastCreated<Region>();
                if (region != null)
                {
                    if (region.Width == match.Width && region.Height == match.Height)
                    {
                        region.AmbientSound = match.VariableName;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }
    }
}