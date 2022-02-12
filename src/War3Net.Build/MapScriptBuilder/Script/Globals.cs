// ------------------------------------------------------------------------------
// <copyright file="Globals.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassGlobalDeclarationListSyntax Globals(Map map)
        {
            var globalDeclarationList = new List<IGlobalDeclarationSyntax>();
            var generatedGlobals = new List<JassGlobalDeclarationSyntax>();

            generatedGlobals.AddRange(Regions(map));
            generatedGlobals.AddRange(Cameras(map));
            generatedGlobals.AddRange(Sounds(map));
            generatedGlobals.AddRange(Triggers(map));
            generatedGlobals.AddRange(Units(map));
            generatedGlobals.AddRange(Destructables(map));
            generatedGlobals.AddRange(RandomUnitTables(map));

            var userDefinedGlobals = new List<JassGlobalDeclarationSyntax>(Variables(map));

            if (userDefinedGlobals.Any())
            {
                globalDeclarationList.Add(new JassCommentSyntax(" User-defined"));
                globalDeclarationList.AddRange(userDefinedGlobals);

                if (generatedGlobals.Any())
                {
                    globalDeclarationList.Add(JassEmptySyntax.Value);
                }
            }

            if (generatedGlobals.Any())
            {
                globalDeclarationList.Add(new JassCommentSyntax(" Generated"));
                globalDeclarationList.AddRange(generatedGlobals);
            }

            return new JassGlobalDeclarationListSyntax(globalDeclarationList.ToImmutableArray());
        }
    }
}