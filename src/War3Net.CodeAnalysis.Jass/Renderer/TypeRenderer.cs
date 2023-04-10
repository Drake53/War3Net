﻿// ------------------------------------------------------------------------------
// <copyright file="TypeRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassTypeSyntax type)
        {
            switch (type)
            {
                case JassIdentifierNameSyntax identifierName: Render(identifierName); break;
                case JassPredefinedTypeSyntax predefinedType: Render(predefinedType); break;

                default: throw new NotSupportedException();
            }
        }
    }
}