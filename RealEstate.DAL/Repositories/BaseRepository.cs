using System.Text.Json;
using RealEstateApp.DAL.Interfaces;
using RealEstateApp.DAL.Models;

namespace RealEstateApp.DAL.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly string _filePath;
        protected List<T> _entities = new List<T>();

        protected BaseRepository(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        public virtual IEnumerable<T> GetAll() => _entities;

        public virtual T? GetById(int id)
        {
            var property = typeof(T).GetProperty("Id");
            return _entities.FirstOrDefault(e => (int)property!.GetValue(e)! == id);
        }

        public virtual void Add(T entity)
        {
            _entities.Add(entity);
            SaveData();
        }

        public virtual void Update(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            var id = (int)property!.GetValue(entity)!;

            var existing = GetById(id);
            if (existing != null)
            {
                _entities.Remove(existing);
                _entities.Add(entity);
                SaveData();
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                SaveData();
            }
        }

        public abstract IEnumerable<T> Search(string keyword);

        protected virtual void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _entities = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }

        protected virtual void SaveData()
        {
            var json = JsonSerializer.Serialize(_entities, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}