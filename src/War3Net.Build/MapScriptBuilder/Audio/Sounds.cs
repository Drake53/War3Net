// ------------------------------------------------------------------------------
// <copyright file="Sounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.Build.Audio;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual IEnumerable<MemberDeclarationSyntax> SoundsApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return Sounds(map).Select(sound => transpiler.Transpile(sound));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Sounds(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapSounds = map.Sounds;
            if (mapSounds is null)
            {
                yield break;
            }

            foreach (var sound in mapSounds.Sounds)
            {
                if (sound.Flags.HasFlag(SoundFlags.Music))
                {
                    yield return SyntaxFactory.GlobalDeclaration(
                        JassTypeSyntax.String,
                        sound.Name);
                }
                else
                {
                    yield return SyntaxFactory.GlobalDeclaration(
                        SyntaxFactory.ParseTypeName(TypeName.Sound),
                        sound.Name,
                        JassNullLiteralExpressionSyntax.Value);
                }
            }
        }
    }
}