// ------------------------------------------------------------------------------
// <copyright file="JassToCSharpTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public JassToCSharpTranspiler()
        {
        }

        /// <summary>
        /// Used when <see cref="ApplyCSharpLuaTemplateAttribute"/> is <see langword="true"/>.
        /// </summary>
        public JassToLuaTranspiler? JassToLuaTranspiler { get; set; }

        public bool ApplyCSharpLuaTemplateAttribute { get; set; }
    }
}