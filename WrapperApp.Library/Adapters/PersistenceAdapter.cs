using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WrapperApp.Library.Adapters
{
    public class PersistenceAdapter : IPersistenceAdapter
    {
        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddItem(string name);

        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetItem(int id);

        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool RemoveItem(int id);

        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetItemCount();

        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void UpdateItem(int id, string name);

        [DllImport("WrapperApp.Persistence.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ListItems();

        public int Add(string name) => AddItem(name);

        public Item Get(int id)
        {
            var result = GetItem(id);
            return result == IntPtr.Zero 
                ? null 
                : new Item(id, Marshal.PtrToStringAnsi(result));
        }

        public bool Remove(int id) => RemoveItem(id);

        public int GetCount() => GetItemCount();

        public void Update(int id, string name) => UpdateItem(id, name);

        public IEnumerable<Item> ListAll()
        {
            IntPtr ptr = ListItems();
            if (ptr == IntPtr.Zero) return Enumerable.Empty<Item>();

            string result = Marshal.PtrToStringAnsi(ptr);
            var items = new List<Item>();

            foreach (var entry in result.Split(';'))
            {
                var parts = entry.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    items.Add(new Item(id, parts[1]));
                }
            }

            return items.OrderBy(item => item.Name);
        }
    }
}
