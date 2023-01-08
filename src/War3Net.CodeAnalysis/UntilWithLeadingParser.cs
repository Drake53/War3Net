// ------------------------------------------------------------------------------
// <copyright file="UntilWithLeadingParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Pidgin;

namespace War3Net.CodeAnalysis
{
    internal sealed class UntilWithLeadingParser<TToken, TLeading, TOpen, TItem, TClose, TResult> : Parser<TToken, TResult>
        where TResult : class
    {
        private readonly Parser<TToken, TLeading> _leadingParser;
        private readonly Parser<TToken, TItem> _itemParser;
        private readonly Parser<TToken, TOpen> _openParser;
        private readonly Parser<TToken, TClose> _closeParser;
        private readonly Func<TLeading, TItem, TItem> _itemLeadingSelector;
        private readonly Func<TOpen, IEnumerable<TItem>, TLeading, TClose, TResult> _selector;

        public UntilWithLeadingParser(
            Parser<TToken, TLeading> leadingParser,
            Parser<TToken, TItem> itemParser,
            Parser<TToken, TOpen> openParser,
            Parser<TToken, TClose> closeParser,
            Func<TLeading, TItem, TItem> itemLeadingSelector,
            Func<TOpen, IEnumerable<TItem>, TLeading, TClose, TResult> selector)
        {
            _leadingParser = leadingParser;
            _itemParser = itemParser;
            _openParser = openParser;
            _closeParser = closeParser;
            _itemLeadingSelector = itemLeadingSelector;
            _selector = selector;
        }

        public override bool TryParse(
            ref ParseState<TToken> state,
            ref PooledList<Expected<TToken>> expecteds,
            [MaybeNullWhen(false)] out TResult result)
        {
            if (!_openParser.TryParse(ref state, ref expecteds, out var openResult))
            {
                result = null;
                return false;
            }

            var items = new List<TItem>();

            var leadingExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var closeExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var itemExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            while (true)
            {
                if (!_leadingParser.TryParse(ref state, ref leadingExpecteds, out var leadingResult))
                {
                    expecteds.AddRange(leadingExpecteds.AsSpan());
                    leadingExpecteds.Dispose();
                    closeExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                var closeStartLoc = state.Location;
                if (_closeParser.TryParse(ref state, ref closeExpecteds, out var closeResult))
                {
                    leadingExpecteds.Dispose();
                    closeExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = _selector(openResult, items, leadingResult, closeResult);
                    return true;
                }

                if (state.Location > closeStartLoc)
                {
                    expecteds.AddRange(closeExpecteds.AsSpan());
                    leadingExpecteds.Dispose();
                    closeExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                var itemStartLoc = state.Location;
                if (!_itemParser.TryParse(ref state, ref itemExpecteds, out var itemResult))
                {
                    if (state.Location > itemStartLoc)
                    {
                        expecteds.AddRange(itemExpecteds.AsSpan());
                    }
                    else
                    {
                        expecteds.AddRange(itemExpecteds.AsSpan());
                        expecteds.AddRange(closeExpecteds.AsSpan());
                    }

                    leadingExpecteds.Dispose();
                    closeExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                leadingExpecteds.Clear();
                closeExpecteds.Clear();
                itemExpecteds.Clear();

                if (state.Location <= itemStartLoc)
                {
                    throw new InvalidOperationException("UntilWithLeading() used with a parser which consumed no input");
                }

                items.Add(_itemLeadingSelector(leadingResult, itemResult));
            }
        }
    }
}