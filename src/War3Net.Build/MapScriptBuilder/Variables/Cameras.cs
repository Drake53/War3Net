// ------------------------------------------------------------------------------
// <copyright file="Cameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual IEnumerable<MemberDeclarationSyntax> CamerasApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return Cameras(map).Select(camera => transpiler.Transpile(camera));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Cameras(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapCameras = map.Cameras;
            if (mapCameras is null)
            {
                yield break;
            }

            foreach (var camera in mapCameras.Cameras)
            {
                yield return SyntaxFactory.GlobalDeclaration(
                    SyntaxFactory.ParseTypeName(TypeName.CameraSetup),
                    camera.GetVariableName(),
                    JassNullLiteralExpressionSyntax.Value);
            }
        }
    }
}