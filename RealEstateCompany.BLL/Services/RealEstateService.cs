using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;

namespace RealEstateCompany.BLL.Services
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IRepository<RealEstate> _repository;

        public RealEstateService(IRepository<RealEstate> repository)
        {
            _repository = repository;
        }

        public IEnumerable<RealEstate> GetAllRealEstates() => _repository.GetAll();

        public RealEstate? GetRealEstateById(int id) => _repository.GetById(id);

        public void AddRealEstate(RealEstate realEstate)
        {
            ValidateRealEstate(realEstate);
            _repository.Add(realEstate);
        }

        public void UpdateRealEstate(RealEstate realEstate)
        {
            ValidateRealEstate(realEstate);
            _repository.Update(realEstate);
        }

        public void DeleteRealEstate(int id) => _repository.Delete(id);

        public IEnumerable<RealEstate> SortByType() =>
            _repository.GetAll().OrderBy(r => r.Type);

        public IEnumerable<RealEstate> SortByPrice() =>
            _repository.GetAll().OrderBy(r => r.Price);

        public IEnumerable<RealEstate> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repository.GetAll();

            return _repository.GetAll().Where(r =>
                r.Address.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                r.Type.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                r.Price.ToString().Contains(keyword) ||
                r.Rooms.ToString().Contains(keyword));
        }

        public IEnumerable<RealEstate> FindByCriteria(RealEstateType? type, decimal? maxPrice)
        {
            var query = _repository.GetAll().AsEnumerable();

            if (type.HasValue)
                query = query.Where(r => r.Type == type.Value);

            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice.Value);

            return query.Where(r => r.IsAvailable);
        }

        private void ValidateRealEstate(RealEstate realEstate)
        {
            if (string.IsNullOrWhiteSpace(realEstate.Address))
                throw new ArgumentException("Address is required");

            if (realEstate.Price <= 0)
                throw new ArgumentException("Price must be greater than 0");

            if (realEstate.Area <= 0)
                throw new ArgumentException("Area must be greater than 0");
        }
    }
}