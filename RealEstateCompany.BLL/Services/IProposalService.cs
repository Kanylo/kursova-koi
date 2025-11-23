using RealEstateCompany.DAL.Entities;

namespace RealEstateCompany.BLL.Services
{
    public interface IProposalService
    {
        void AddToProposal(int clientId, int realEstateId);
        void RemoveFromProposal(int clientId, int realEstateId);
        IEnumerable<RealEstate> GetClientProposals(int clientId);
        bool CheckAvailability(RealEstateType type, decimal price);
    }
}