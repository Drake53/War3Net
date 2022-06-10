// ------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Pidgin;

namespace War3Net.CodeAnalysis
{
    public static class ParserExtensions
    {
        public static Parser<TToken, TResult> IfThenElse<TToken, TLeading, TItem, TIfDeclarator, TElseIfDeclarator, TElseDeclarator, TEndIf, TIfClause, TElseIfClause, TElseClause, TResult>(
            this Parser<TToken, TItem> itemParser,
            Parser<TToken, TLeading> leadingParser,
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
            where TElseIfDeclarator : class
            where TElseDeclarator : class
            where TIfClause : class
            where TElseClause : class
            where TResult : class
        {
#pragma warning disable IDE0011, SA1503
            if (leadingParser is null) throw new ArgumentNullException(nameof(leadingParser));
            if (itemParser is null) throw new ArgumentNullException(nameof(itemParser));
            if (ifDeclaratorParser is null) throw new ArgumentNullException(nameof(ifDeclaratorParser));
            if (elseIfDeclaratorParser is null) throw new ArgumentNullException(nameof(elseIfDeclaratorParser));
            if (elseDeclaratorParser is null) throw new ArgumentNullException(nameof(elseDeclaratorParser));
            if (endIfParser is null) throw new ArgumentNullException(nameof(endIfParser));
            if (ifClauseSelector is null) throw new ArgumentNullException(nameof(ifClauseSelector));
            if (elseIfClauseSelector is null) throw new ArgumentNullException(nameof(elseIfClauseSelector));
            if (elseClauseSelector is null) throw new ArgumentNullException(nameof(elseClauseSelector));
            if (itemLeadingSelector is null) throw new ArgumentNullException(nameof(itemLeadingSelector));
            if (elseIfDeclaratorLeadingSelector is null) throw new ArgumentNullException(nameof(elseIfDeclaratorLeadingSelector));
            if (elseDeclaratorLeadingSelector is null) throw new ArgumentNullException(nameof(elseDeclaratorLeadingSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
#pragma warning restore IDE0011, SA1503

            return new IfThenElseParser<TToken, TLeading, TItem, TIfDeclarator, TElseIfDeclarator, TElseDeclarator, TEndIf, TIfClause, TElseIfClause, TElseClause, TResult>(
                leadingParser,
                itemParser,
                ifDeclaratorParser,
                elseIfDeclaratorParser,
                elseDeclaratorParser,
                endIfParser,
                ifClauseSelector,
                elseIfClauseSelector,
                elseClauseSelector,
                itemLeadingSelector,
                elseIfDeclaratorLeadingSelector,
                elseDeclaratorLeadingSelector,
                resultSelector);
        }

        public static Parser<TToken, SeparatedSyntaxList<TItem, TSeparator>> SeparatedList<TToken, TItem, TSeparator>(
            this Parser<TToken, TItem> itemParser,
            Parser<TToken, TSeparator> separatorParser)
        {
#pragma warning disable IDE0011, SA1503
            if (itemParser is null) throw new ArgumentNullException(nameof(itemParser));
            if (separatorParser is null) throw new ArgumentNullException(nameof(separatorParser));
#pragma warning restore IDE0011, SA1503

            return new SeparatedParser<TToken, TItem, TSeparator>(
                itemParser,
                separatorParser);
        }

        public static Parser<TToken, TResult> UntilWithLeading<TToken, TLeading, TOpen, TItem, TClose, TResult>(
            this Parser<TToken, TItem> itemParser,
            Parser<TToken, TLeading> leadingParser,
            Parser<TToken, TOpen> openParser,
            Parser<TToken, TClose> closeParser,
            Func<TLeading, TItem, TItem> itemLeadingSelector,
            Func<TOpen, IEnumerable<TItem>, TLeading, TClose, TResult> selector)
            where TResult : class
        {
#pragma warning disable IDE0011, SA1503
            if (itemParser is null) throw new ArgumentNullException(nameof(itemParser));
            if (leadingParser is null) throw new ArgumentNullException(nameof(leadingParser));
            if (openParser is null) throw new ArgumentNullException(nameof(openParser));
            if (closeParser is null) throw new ArgumentNullException(nameof(closeParser));
            if (itemLeadingSelector is null) throw new ArgumentNullException(nameof(itemLeadingSelector));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
#pragma warning restore IDE0011, SA1503

            return new UntilWithLeadingParser<TToken, TLeading, TOpen, TItem, TClose, TResult>(
                leadingParser,
                itemParser,
                openParser,
                closeParser,
                itemLeadingSelector,
                selector);
        }
    }
}