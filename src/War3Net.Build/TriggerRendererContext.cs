// ------------------------------------------------------------------------------
// <copyright file="TriggerRendererContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis;

namespace War3Net.Build
{
    public class TriggerRendererContext
    {
        private readonly IndentedTextWriter _writer;
        private readonly TrigFunctionIdentifierBuilder _builder;

        public TriggerRendererContext(
            IndentedTextWriter writer,
            TrigFunctionIdentifierBuilder builder)
        {
            _writer = writer;
            _builder = builder;
        }

        public IndentedTextWriter Writer => _writer;

        public TrigFunctionIdentifierBuilder TrigFunctionIdentifierBuilder => _builder;
    }
}