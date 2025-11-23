using RealEstateCompany.DAL.Entities;

namespace RealEstateCompany.BLL.Services
{
    public interface IRealEstateService
    {
        IEnumerable<RealEstate> GetAllRealEstates();
        RealEstate? GetRealEstateById(int id);
        void AddRealEstate(RealEstate realEstate);
        void UpdateRealEstate(RealEstate realEstate);
        void DeleteRealEstate(int id);

        IEnumerable<RealEstate> SortByType();
        IEnumerable<RealEstate> SortByPrice();

        IEnumerable<RealEstate> Search(string keyword);
        IEnumerable<RealEstate> FindByCriteria(RealEstateType? type, decimal? maxPrice);
    }
}