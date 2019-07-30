// ------------------------------------------------------------------------------
// <copyright file="ManyParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A parser combinator that implements the unary * operator.
    /// </summary>
    internal abstract class ManyParser : AlternativeParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManyParser"/> class.
        /// </summary>
        public ManyParser(IParser baseParser = null)
        {
            if (baseParser != null)
            {
                SetParser(baseParser);
            }
        }

        public override void AddParser(IParser baseParser)
        {
            throw new NotSupportedException($"The {nameof(ManyParser)} is a special case of {nameof(AlternativeParser)}, and must have exactly two alternatives, one of which {nameof(EmptyParser)}. Use the method {nameof(SetParser)} instead.");
        }

        public void SetParser(IParser baseParser)
        {
            if (Alternatives == 0)
            {
                base.AddParser(new Many1Parser(baseParser ?? throw new ArgumentNullException(nameof(baseParser))));
                base.AddParser(EmptyParser.Get);
            }
            else
            {
                throw new InvalidOperationException("Base parser cannot be overwritten after it's been set.");
            }
        }
    }
}