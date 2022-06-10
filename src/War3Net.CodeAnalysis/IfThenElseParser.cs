// ------------------------------------------------------------------------------
// <copyright file="IfThenElseParser.cs" company="Drake53">
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
    internal class IfThenElseParser<TToken, TLeading, TItem, TIfDeclarator, TElseIfDeclarator, TElseDeclarator, TEndIf, TIfClause, TElseIfClause, TElseClause, TResult> : Parser<TToken, TResult>
        where TElseIfDeclarator : class
        where TElseDeclarator : class
        where TIfClause : class
        where TElseClause : class
        where TResult : class
    {
        private readonly Parser<TToken, TLeading> _leadingParser;
        private readonly Parser<TToken, TItem> _itemParser;
        private readonly Parser<TToken, TIfDeclarator> _ifDeclaratorParser;
        private readonly Parser<TToken, TElseIfDeclarator> _elseIfDeclaratorParser;
        private readonly Parser<TToken, TElseDeclarator> _elseDeclaratorParser;
        private readonly Parser<TToken, TEndIf> _endIfParser;
        private readonly Func<TIfDeclarator, IEnumerable<TItem>, TIfClause> _ifClauseSelector;
        private readonly Func<TElseIfDeclarator, IEnumerable<TItem>, TElseIfClause> _elseIfClauseSelector;
        private readonly Func<TElseDeclarator, IEnumerable<TItem>, TElseClause> _elseClauseSelector;
        private readonly Func<TLeading, TItem, TItem> _itemLeadingSelector;
        private readonly Func<TLeading, TElseIfDeclarator, TElseIfDeclarator> _elseIfDeclaratorLeadingSelector;
        private readonly Func<TLeading, TElseDeclarator, TElseDeclarator> _elseDeclaratorLeadingSelector;
        private readonly Func<TIfClause, IEnumerable<TElseIfClause>, TElseClause?, TLeading, TEndIf, TResult> _resultSelector;

        public IfThenElseParser(
            Parser<TToken, TLeading> leadingParser,
            Parser<TToken, TItem> itemParser,
            Parser<TToken, TIfDeclarator> ifDeclaratorParser,
            Parser<TToken, TElseIfDeclarator> elseIfDeclaratorParser,
            Parser<TToken, TElseDeclarator> elseDeclaratorParser,
            Parser<TToken, TEndIf> endIfParser,
            Func<TIfDeclarator, IEnumerable<TItem>, TIfClause> ifClauseSelector,
            Func<TElseIfDeclarator, IEnumerable<TItem>, TElseIfClause> elseIfClauseSelector,
            Func<TElseDeclarator, IEnumerable<TItem>, TElseClause> elseClauseSelector,
            Func<TLeading, TItem, TItem> itemLeadingSelector,
            Func<TLeading, TElseIfDeclarator, TElseIfDeclarator> elseIfDeclaratorLeadingSelector,
            Func<TLeading, TElseDeclarator, TElseDeclarator> elseDeclaratorLeadingSelector,
            Func<TIfClause, IEnumerable<TElseIfClause>, TElseClause?, TLeading, TEndIf, TResult> resultSelector)
        {
            _leadingParser = leadingParser;
            _itemParser = itemParser;
            _ifDeclaratorParser = ifDeclaratorParser;
            _elseIfDeclaratorParser = elseIfDeclaratorParser;
            _elseDeclaratorParser = elseDeclaratorParser;
            _endIfParser = endIfParser;
            _ifClauseSelector = ifClauseSelector;
            _elseIfClauseSelector = elseIfClauseSelector;
            _elseClauseSelector = elseClauseSelector;
            _elseIfDeclaratorLeadingSelector = elseIfDeclaratorLeadingSelector;
            _elseDeclaratorLeadingSelector = elseDeclaratorLeadingSelector;
            _itemLeadingSelector = itemLeadingSelector;
            _resultSelector = resultSelector;
        }

        public override bool TryParse(
            ref ParseState<TToken> state,
            ref PooledList<Expected<TToken>> expecteds,
            [MaybeNullWhen(false)] out TResult result)
        {
            if (!_ifDeclaratorParser.TryParse(ref state, ref expecteds, out var ifResult))
            {
                result = null;
                return false;
            }

            var items = new List<TItem>();

            var elseIfDeclarator = (TElseIfDeclarator?)null;
            var elseDeclarator = (TElseDeclarator?)null;

            var ifClause = (TIfClause?)null;
            var elseIfClauses = new List<TElseIfClause>();
            var elseClause = (TElseClause?)null;

            var leadingExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var elseIfExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var elseExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var endIfExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            var itemExpecteds = new PooledList<Expected<TToken>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<TToken>>());
            while (true)
            {
                if (!_leadingParser.TryParse(ref state, ref leadingExpecteds, out var leadingResult))
                {
                    expecteds.AddRange(leadingExpecteds.AsSpan());
                    leadingExpecteds.Dispose();
                    elseIfExpecteds.Dispose();
                    elseExpecteds.Dispose();
                    endIfExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                var terminatorStartLoc = state.Location;
                if (_endIfParser.TryParse(ref state, ref endIfExpecteds, out var endIfResult))
                {
                    leadingExpecteds.Dispose();
                    elseIfExpecteds.Dispose();
                    elseExpecteds.Dispose();
                    endIfExpecteds.Dispose();
                    itemExpecteds.Dispose();

                    if (ifClause is null)
                    {
                        ifClause = _ifClauseSelector(ifResult, items);
                    }
                    else if (elseDeclarator is not null)
                    {
                        elseClause = _elseClauseSelector(elseDeclarator, items);
                    }
                    else
                    {
                        elseIfClauses.Add(_elseIfClauseSelector(elseIfDeclarator, items));
                    }

                    result = _resultSelector(ifClause, elseIfClauses, elseClause, leadingResult, endIfResult);
                    return true;
                }

                if (state.Location > terminatorStartLoc)
                {
                    expecteds.AddRange(endIfExpecteds.AsSpan());
                    leadingExpecteds.Dispose();
                    elseIfExpecteds.Dispose();
                    elseExpecteds.Dispose();
                    endIfExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                if (elseDeclarator is null)
                {
                    if (_elseDeclaratorParser.TryParse(ref state, ref elseExpecteds, out var elseResult))
                    {
                        leadingExpecteds.Clear();
                        elseExpecteds.Clear();
                        endIfExpecteds.Clear();

                        if (ifClause is null)
                        {
                            ifClause = _ifClauseSelector(ifResult, items);
                        }
                        else
                        {
                            elseIfClauses.Add(_elseIfClauseSelector(elseIfDeclarator, items));
                        }

                        elseDeclarator = _elseDeclaratorLeadingSelector(leadingResult, elseResult);

                        items.Clear();
                        continue;
                    }

                    if (state.Location > terminatorStartLoc)
                    {
                        expecteds.AddRange(elseExpecteds.AsSpan());
                        leadingExpecteds.Dispose();
                        elseIfExpecteds.Dispose();
                        elseExpecteds.Dispose();
                        endIfExpecteds.Dispose();
                        itemExpecteds.Dispose();
                        result = null;
                        return false;
                    }

                    if (_elseIfDeclaratorParser.TryParse(ref state, ref elseIfExpecteds, out var elseIfResult))
                    {
                        leadingExpecteds.Clear();
                        elseIfExpecteds.Clear();
                        elseExpecteds.Clear();
                        endIfExpecteds.Clear();

                        if (state.Location <= terminatorStartLoc)
                        {
                            throw new InvalidOperationException("IfThenElse() used with an elseif parser which consumed no input");
                        }

                        if (ifClause is null)
                        {
                            ifClause = _ifClauseSelector(ifResult, items);
                        }
                        else
                        {
                            elseIfClauses.Add(_elseIfClauseSelector(elseIfDeclarator, items));
                        }

                        elseIfDeclarator = _elseIfDeclaratorLeadingSelector(leadingResult, elseIfResult);

                        items.Clear();
                        continue;
                    }

                    if (state.Location > terminatorStartLoc)
                    {
                        expecteds.AddRange(elseIfExpecteds.AsSpan());
                        leadingExpecteds.Dispose();
                        elseIfExpecteds.Dispose();
                        elseExpecteds.Dispose();
                        endIfExpecteds.Dispose();
                        itemExpecteds.Dispose();
                        result = null;
                        return false;
                    }
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
                        if (elseDeclarator is null)
                        {
                            expecteds.AddRange(elseIfExpecteds.AsSpan());
                            expecteds.AddRange(elseExpecteds.AsSpan());
                        }

                        expecteds.AddRange(endIfExpecteds.AsSpan());
                        expecteds.AddRange(itemExpecteds.AsSpan());
                    }

                    leadingExpecteds.Dispose();
                    elseIfExpecteds.Dispose();
                    elseExpecteds.Dispose();
                    endIfExpecteds.Dispose();
                    itemExpecteds.Dispose();
                    result = null;
                    return false;
                }

                leadingExpecteds.Clear();
                elseIfExpecteds.Clear();
                elseExpecteds.Clear();
                endIfExpecteds.Clear();
                itemExpecteds.Clear();

                if (state.Location <= itemStartLoc)
                {
                    throw new InvalidOperationException("IfThenElse() used with a parser which consumed no input");
                }

                items.Add(_itemLeadingSelector(leadingResult, itemResult));
            }
        }
    }
}