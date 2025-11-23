using RealEstateApp.DAL.Models;

namespace RealEstateApp.DAL.Interfaces
{
    public interface IRealEstateRepository : IRepository<RealEstate>
    {
        IEnumerable<RealEstate> SortByType();
        IEnumerable<RealEstate> SortByPrice();
        IEnumerable<RealEstate> FindByCriteria(RealEstateType? type, decimal? maxPrice);
    }
}