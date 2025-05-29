using System.Collections.Generic;

namespace WrapperApp.Library.Adapters
{
    public interface IPersistenceAdapter
    {
        int Add(string name);
        Item Get(int id);
        bool Remove(int id);
        int GetCount();
        void Update(int id, string name);
        IEnumerable<Item> ListAll();
    }

    public class Item
    {
        public Item(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}


