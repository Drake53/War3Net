// ------------------------------------------------------------------------------
// <copyright file="SeparatedParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Pidgin;

namespace War3Net.CodeAnalysis
{
    internal sealed class SeparatedParser<TToken, TItem, TSeparator> : Parser<TToken, SeparatedSyntaxList<TItem, TSeparator>>
    {
        private readonly Parser<TToken, TItem> _itemParser;
        private readonly Parser<TToken, TSeparator> _separatorParser;

        public SeparatedParser(
            Parser<TToken, TItem> itemParser,
            Parser<TToken, TSeparator> separatorParser)
        {
            _itemParser = itemParser;
            _separatorParser = separatorParser;
        }

        public override bool TryParse(
            ref ParseState<TToken> state,
            ref PooledList<Expected<TToken>> expecteds,
            [MaybeNullWhen(false)] out SeparatedSyntaxList<TItem, TSeparator> result)
        {
            if (!_itemParser.TryParse(ref state, ref expecteds, out var firstResult))
            {
                result = SeparatedSyntaxList<TItem, TSeparator>.Empty;
                return true;
            }

            var builder = SeparatedSyntaxList<TItem, TSeparator>.CreateBuilder(firstResult);

            var childExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            while (true)
            {
                var separatorStartLoc = state.Location;
                if (!_separatorParser.TryParse(ref state, ref childExpecteds, out var separatorResult))
                {
                    if (state.Location <= separatorStartLoc)
                    {
                        childExpecteds.Dispose();
                        result = builder.ToSeparatedSyntaxList();
                        return true;
                    }

                    expecteds.AddRange(childExpecteds.AsSpan());
                    childExpecteds.Dispose();
                    result = null;
                    return false;
                }

                childExpecteds.Clear();

                var itemStartLoc = state.Location;
                if (!_itemParser.TryParse(ref state, ref childExpecteds, out var itemResult))
                {
                    expecteds.AddRange(childExpecteds.AsSpan());
                    childExpecteds.Dispose();
                    result = null;
                    return false;
                }

                childExpecteds.Clear();

                if (state.Location <= itemStartLoc)
                {
                    childExpecteds.Dispose();

                    throw new InvalidOperationException("Separated() used with a parser which consumed no input");
                }

                builder.Add(separatorResult, itemResult);
            }
        }
    }
}