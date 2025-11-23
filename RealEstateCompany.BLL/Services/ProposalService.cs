using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;
using RealEstateCompany.BLL.Exceptions;

namespace RealEstateCompany.BLL.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<RealEstate> _realEstateRepository;

        public ProposalService(IRepository<Client> clientRepository, IRepository<RealEstate> realEstateRepository)
        {
            _clientRepository = clientRepository;
            _realEstateRepository = realEstateRepository;
        }

        public void AddToProposal(int clientId, int realEstateId)
        {
            var client = _clientRepository.GetById(clientId);
            var realEstate = _realEstateRepository.GetById(realEstateId);

            if (client == null)
                throw new ClientNotFoundException(clientId);

            if (realEstate == null)
                throw new RealEstateNotFoundException(realEstateId);

            if (client.ProposedRealEstateIds.Count >= 5)
                throw new MaximumProposalsExceededException();

            if (!client.ProposedRealEstateIds.Contains(realEstateId))
            {
                client.ProposedRealEstateIds.Add(realEstateId);
                _clientRepository.Update(client);
            }
        }

        public void RemoveFromProposal(int clientId, int realEstateId)
        {
            var client = _clientRepository.GetById(clientId);

            if (client == null)
                throw new ArgumentException($"Client with ID {clientId} not found");

            client.ProposedRealEstateIds.Remove(realEstateId);
            _clientRepository.Update(client);
        }

        public IEnumerable<RealEstate> GetClientProposals(int clientId)
        {
            var client = _clientRepository.GetById(clientId);
            if (client == null)
                return Enumerable.Empty<RealEstate>();

            return client.ProposedRealEstateIds
                .Select(id => _realEstateRepository.GetById(id))
                .Where(r => r != null)!;
        }

        public bool CheckAvailability(RealEstateType type, decimal price)
        {
            return _realEstateRepository.GetAll()
                .Any(r => r.Type == type && r.Price <= price && r.IsAvailable);
        }
    }
}