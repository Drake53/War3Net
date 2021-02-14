// ------------------------------------------------------------------------------
// <copyright file="Cameras.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual IEnumerable<JassGlobalDeclarationSyntax> Cameras(Map map)
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
                    SyntaxFactory.ParseTypeName(nameof(camerasetup)),
                    camera.GetVariableName(),
                    JassNullLiteralExpressionSyntax.Value);
            }
        }
    }
}