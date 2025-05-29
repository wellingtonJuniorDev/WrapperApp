using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WrapperApp.Library.Adapters
{
    public class EdgePersistenceAdapter
    {
        private readonly IPersistenceAdapter _adapter = new PersistenceAdapter();

        public async Task<object> Add(dynamic input)
        {
            string name = input.name;
            int id = _adapter.Add(name);
            return await Task.FromResult(id);
        }

        public async Task<object> Get(dynamic input)
        {
            int id = input.id;
            var item = _adapter.Get(id);

            dynamic result = new ExpandoObject();
            result.id = item.Id;
            result.name = item.Name;
            return await Task.FromResult(result);
        }

        public async Task<object> Remove(dynamic input)
        {
            int id = input.id;
            bool success = _adapter.Remove(id);
            return await Task.FromResult(success);
        }

        public async Task<object> GetCount(dynamic input)
        {
            return await Task.FromResult(_adapter.GetCount());
        }

        public async Task<object> Update(dynamic input)
        {
            int id = input.id;
            string name = input.name;
            _adapter.Update(id, name);
            return await Task.FromResult(true);
        }

        public async Task<object> ListAll(dynamic input)
        {
            var items = _adapter.ListAll().Select(item =>
            {
                dynamic obj = new ExpandoObject();
                obj.id = item.Id;
                obj.name = item.Name;
                return obj;
            }).ToList();

            return await Task.FromResult(items);
        }
    }

}

