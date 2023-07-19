using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class CommandItems : IReadOnlyList<Item>
    {
        public CommandItems(IReadOnlyList<Item> items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public CommandItems(IEnumerable<Item> items) : this(items?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(items))) { }

        private readonly IReadOnlyList<Item> _items;

        public Item this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<Item> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_items).GetEnumerator();

        public int ArgumentIndexOf()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Type is ItemType.Argument or ItemType.ArgumentError)
                    return i;
            }

            return -1;
        }

        public int ItemIndexOf(int bufferIndex)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (bufferIndex >= _items[i].StartIndex)
                    return i;
            }
            return -1;
        }

        public Item? ItemOf(int bufferIndex)
        {
            int index = ItemIndexOf(bufferIndex);
            if (index != -1)
                return _items[index];
            return null;
        }
    }
}
