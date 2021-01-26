// ------------------------------------------------------------------------------
// <copyright file="JassRendererOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRendererOptions
    {
        private static readonly JassRendererOptions _default = new JassRendererOptions();

        public string NewLineString { get; init; }

        public string IndentationString { get; init; }

        public JassRendererOptions()
        {
            NewLineString = "\r\n";
            IndentationString = new string(' ', 4);
        }

        public static JassRendererOptions Default => _default;
    }
}