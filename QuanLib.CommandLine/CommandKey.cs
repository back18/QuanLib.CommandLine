using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandKey
    {
        public CommandKey(string keytext)
        {
            if (string.IsNullOrEmpty(keytext))
                throw new ArgumentException($"“{nameof(keytext)}”不能为 null 或空。", nameof(keytext));

            KeyItems = new(keytext.Split(' '));

            if (KeyItems.Count == 0)
                throw new ArgumentException("至少需要有一个 key item", nameof(keytext));

            foreach (var item in KeyItems)
            {
                if (string.IsNullOrEmpty(item))
                    throw new ArgumentException("key item 不能为空", nameof(keytext));
            }
        }

        public CommandKey(IEnumerable<string> keyitems)
        {
            if (keyitems is null)
                throw new ArgumentNullException(nameof(keyitems));

            KeyItems = keyitems.ToList().AsReadOnly();

            if (KeyItems.Count == 0)
                throw new ArgumentException("至少需要有一个 key item", nameof(keyitems));

            foreach (var item in KeyItems)
            {
                if (string.IsNullOrEmpty(item))
                    throw new ArgumentException("key item 不能为空", nameof(keyitems));

                if (item.Contains(' '))
                    throw new ArgumentException("key item 不能包含空格", nameof(keyitems));
            }
        }

        public ReadOnlyCollection<string> KeyItems { get; }

        public int KeyItemCount => KeyItems.Count;

        public bool IsSubKey(CommandKey key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (key.KeyItemCount < KeyItemCount)
                return false;

            for (int i = 0; i < key.KeyItemCount; i++)
                if (key.KeyItems[i] != KeyItems[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return string.Join(' ', KeyItems);
        }
    }
}
