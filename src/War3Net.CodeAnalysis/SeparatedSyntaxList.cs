// ------------------------------------------------------------------------------
// <copyright file="SeparatedSyntaxList.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Text;

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

        public bool IsEmpty => _items.IsEmpty;

        public static SeparatedSyntaxList<TItem, TSeparator> Create(TItem item)
        {
            return new SeparatedSyntaxList<TItem, TSeparator>(ImmutableArray.Create(item), ImmutableArray<TSeparator>.Empty);
        }

        public static SeparatedSyntaxList<TItem, TSeparator> Create(ImmutableArray<TItem> items, ImmutableArray<TSeparator> separators)
        {
            if (items.IsEmpty)
            {
                if (!separators.IsEmpty)
                {
                    throw new ArgumentException("Separators must be empty if items is empty.", nameof(separators));
                }

                return Empty;
            }
            else if (items.Length - 1 != separators.Length)
            {
                throw new ArgumentException("Amount of separators must be 1 less than amount of items.", nameof(separators));
            }

            return new SeparatedSyntaxList<TItem, TSeparator>(items, separators);
        }

        public static Builder CreateBuilder(TItem firstItem)
        {
            return new Builder(firstItem);
        }

        public static Builder CreateBuilder(TItem firstItem, int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            return new Builder(firstItem, initialCapacity);
        }

        public override string ToString()
        {
            if (Items.IsEmpty)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            sb.Append(Items[0]);
            for (var i = 1; i < Items.Length; i++)
            {
                sb.Append(Separators[i - 1]);
                sb.Append(' ');
                sb.Append(Items[i]);
            }

            return sb.ToString();
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

            internal Builder(TItem firstItem, int initialCapacity)
            {
                _itemBuilder = ImmutableArray.CreateBuilder<TItem>(initialCapacity);
                _itemBuilder.Add(firstItem);
                _separatorBuilder = ImmutableArray.CreateBuilder<TSeparator>(initialCapacity - 1);
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