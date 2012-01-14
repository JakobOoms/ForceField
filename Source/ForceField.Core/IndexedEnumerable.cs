using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ForceField.Core
{
    internal class IndexedEnumerable<TItem, TKey> : KeyedCollection<TKey, TItem>, IIndexerEnumerable<TItem, TKey>
    {
        private readonly Func<TItem, TKey> _keySelector;

        public IndexedEnumerable(Func<TItem, TKey> keySelector) 
            : this(keySelector, Enumerable.Empty<TItem>())
        {
        }

        [DebuggerStepThrough]
        public IndexedEnumerable(Func<TItem, TKey> keySelector, IEnumerable<TItem> items )
        {
            _keySelector = keySelector;
            foreach (var item in items)
            {
                Add(item);
            }
        }

        [DebuggerStepThrough]
        protected override TKey GetKeyForItem(TItem item)
        {
            return _keySelector(item);
        }
    }
}