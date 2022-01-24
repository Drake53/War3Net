// ------------------------------------------------------------------------------
// <copyright file="TriggerRendererContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass;

namespace War3Net.Build
{
    public class TriggerRendererContext
    {
        private readonly JassRenderer _renderer;
        private readonly TrigFunctionIdentifierBuilder _builder;

        public TriggerRendererContext(JassRenderer renderer, TrigFunctionIdentifierBuilder builder)
        {
            _renderer = renderer;
            _builder = builder;
        }

        public JassRenderer Renderer => _renderer;

        public TrigFunctionIdentifierBuilder TrigFunctionIdentifierBuilder => _builder;
    }
}