using System.Text.Json;
using RealEstateCompany.DAL.Entities;

namespace RealEstateCompany.DAL.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;
        private List<T> _entities;
        private int _nextId = 1;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _entities = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

                // Calculate next ID
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty != null && _entities.Any())
                {
                    _nextId = _entities.Max(e => (int)idProperty.GetValue(e)!) + 1;
                }
            }
            else
            {
                _entities = new List<T>();
            }
        }

        private void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_entities, options);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<T> GetAll() => _entities;

        public T? GetById(int id)
        {
            var idProperty = typeof(T).GetProperty("Id");
            return _entities.FirstOrDefault(e => (int)idProperty!.GetValue(e)! == id);
        }

        public void Add(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            idProperty?.SetValue(entity, _nextId++);
            _entities.Add(entity);
            SaveData();
        }

        public void Update(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            var id = (int)idProperty!.GetValue(entity)!;

            var existing = GetById(id);
            if (existing != null)
            {
                var index = _entities.IndexOf(existing);
                _entities[index] = entity;
                SaveData();
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                SaveData();
            }
        }

        public void Save() => SaveData();
    }
}