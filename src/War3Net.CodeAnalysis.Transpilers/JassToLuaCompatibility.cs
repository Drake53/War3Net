// ------------------------------------------------------------------------------
// <copyright file="JassToLuaCompatibility.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static class JassToLuaCompatibility
    {
        public static string ConcatNil => Encoding.UTF8.GetString(Resources.JassToLuaCompatibility.concatnil);

        public static string SubString => Encoding.UTF8.GetString(Resources.JassToLuaCompatibility.substring);
    }
}