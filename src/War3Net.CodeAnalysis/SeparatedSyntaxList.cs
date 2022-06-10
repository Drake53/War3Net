// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxList.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace War3Net.CodeAnalysis
{
    public class SeparatedSyntaxList<TItem, TSeparator>
    {
        public static readonly SeparatedSyntaxList<TItem, TSeparator> Empty = new();

        private readonly ImmutableArray<TItem> _items;
        private readonly ImmutableArray<TSeparator> _separators;

        private SeparatedSyntaxList()
        {
            _items = ImmutableArray<TItem>.Empty;
            _separators = ImmutableArray<TSeparator>.Empty;
        }

        private SeparatedSyntaxList(ImmutableArray<TItem> items, ImmutableArray<TSeparator> separators)
        {
            _items = items;
            _separators = separators;
        }

        public ImmutableArray<TItem> Items => _items;

        public ImmutableArray<TSeparator> Separators => _separators;

        public static Builder CreateBuilder(TItem firstItem)
        {
            return new Builder(firstItem);
        }

        public class Builder
        {
            private readonly ImmutableArray<TItem>.Builder _itemBuilder;
            private readonly ImmutableArray<TSeparator>.Builder _separatorBuilder;

            internal Builder(TItem firstItem)
            {
                _itemBuilder = ImmutableArray.CreateBuilder<TItem>();
                _itemBuilder.Add(firstItem);
                _separatorBuilder = ImmutableArray.CreateBuilder<TSeparator>();
            }

            public void Add(TSeparator separator, TItem item)
            {
                _itemBuilder.Add(item);
                _separatorBuilder.Add(separator);
            }

            public SeparatedSyntaxList<TItem, TSeparator> ToSeparatedSyntaxList()
            {
                return new SeparatedSyntaxList<TItem, TSeparator>(
                    _itemBuilder.ToImmutable(),
                    _separatorBuilder.ToImmutable());
            }
        }
    }
}