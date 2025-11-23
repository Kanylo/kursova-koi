using RealEstate.BLL.Exceptions;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;

namespace RealEstate.BLL.Services
{
    public class RealEstateService
    {
        private readonly IRealEstateRepository _realEstateRepository;

        public RealEstateService(IRealEstateRepository realEstateRepository)
        {
            _realEstateRepository = realEstateRepository;
        }

        public async Task<RealEstateProperty> AddRealEstateAsync(RealEstateProperty realEstate)
        {
            ValidateRealEstate(realEstate);
            await _realEstateRepository.AddAsync(realEstate);
            return realEstate;
        }

        public async Task UpdateRealEstateAsync(RealEstateProperty realEstate)
        {
            ValidateRealEstate(realEstate);

            if (!await _realEstateRepository.ExistsAsync(realEstate.Id))
            {
                throw new EntityNotFoundException($"Real estate with ID {realEstate.Id} not found");
            }

            await _realEstateRepository.UpdateAsync(realEstate);
        }

        public async Task DeleteRealEstateAsync(int id)
        {
            if (!await _realEstateRepository.ExistsAsync(id))
            {
                throw new EntityNotFoundException($"Real estate with ID {id} not found");
            }

            await _realEstateRepository.DeleteAsync(id);
        }

        public async Task<RealEstateProperty> GetRealEstateAsync(int id)
        {
            var realEstate = await _realEstateRepository.GetByIdAsync(id);
            if (realEstate == null)
            {
                throw new EntityNotFoundException($"Real estate with ID {id} not found");
            }
            return realEstate;
        }

        public async Task<IEnumerable<RealEstateProperty>> GetAllRealEstatesAsync()
        {
            return await _realEstateRepository.GetAllAsync();
        }

        public async Task<IEnumerable<RealEstateProperty>> GetRealEstatesSortedByTypeAsync()
        {
            return await _realEstateRepository.GetSortedByTypeAsync();
        }

        public async Task<IEnumerable<RealEstateProperty>> GetRealEstatesSortedByPriceAsync()
        {
            return await _realEstateRepository.GetSortedByPriceAsync();
        }

        public async Task<IEnumerable<RealEstateProperty>> SearchRealEstatesAsync(string keyword)
        {
            return await _realEstateRepository.SearchAsync(keyword);
        }

        public async Task<IEnumerable<RealEstateProperty>> FindRealEstatesByCriteriaAsync(RealEstateType? type, decimal? minPrice, decimal? maxPrice)
        {
            return await _realEstateRepository.FindByCriteriaAsync(type, minPrice, maxPrice);
        }

        private void ValidateRealEstate(RealEstateProperty realEstate)
        {
            if (string.IsNullOrWhiteSpace(realEstate.Address))
                throw new ValidationException("Address is required");

            if (realEstate.Price <= 0)
                throw new ValidationException("Price must be greater than 0");

            if (realEstate.Area <= 0)
                throw new ValidationException("Area must be greater than 0");
        }
    }
}