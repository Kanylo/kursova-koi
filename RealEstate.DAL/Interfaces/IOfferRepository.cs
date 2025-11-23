using RealEstateApp.DAL.Models;

namespace RealEstateApp.DAL.Interfaces
{
    public interface IOfferRepository : IRepository<Offer>
    {
        bool CanAddMoreRealEstates(int clientId);
        void AddRealEstateToOffer(int offerId, int realEstateId);
        void RejectRealEstate(int clientId, int realEstateId);
        IEnumerable<RealEstate> FindMatchingRealEstates(RealEstateType type, decimal maxPrice);
    }
}