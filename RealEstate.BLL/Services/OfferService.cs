using RealEstate.BLL.Exceptions;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;

namespace RealEstate.BLL.Services
{
    public class OfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IRealEstateRepository _realEstateRepository;

        public OfferService(IOfferRepository offerRepository, IClientRepository clientRepository, IRealEstateRepository realEstateRepository)
        {
            _offerRepository = offerRepository;
            _clientRepository = clientRepository;
            _realEstateRepository = realEstateRepository;
        }

        public async Task<Offer> CreateOfferAsync(Offer offer)
        {
            ValidateOffer(offer);

            // Check if client exists
            if (!await _clientRepository.ExistsAsync(offer.ClientId))
            {
                throw new EntityNotFoundException($"Client with ID {offer.ClientId} not found");
            }

            // Check if the client already has an offer
            if (await _offerRepository.ClientHasOfferAsync(offer.ClientId))
            {
                throw new BusinessRuleException("Client already has an offer");
            }

            await _offerRepository.AddAsync(offer);
            return offer;
        }

        public async Task UpdateOfferAsync(Offer offer)
        {
            ValidateOffer(offer);

            if (!await _offerRepository.ExistsAsync(offer.Id))
            {
                throw new EntityNotFoundException($"Offer with ID {offer.Id} not found");
            }

            await _offerRepository.UpdateAsync(offer);
        }

        public async Task DeleteOfferAsync(int id)
        {
            if (!await _offerRepository.ExistsAsync(id))
            {
                throw new EntityNotFoundException($"Offer with ID {id} not found");
            }

            await _offerRepository.DeleteAsync(id);
        }

        public async Task<Offer> GetOfferAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer == null)
            {
                throw new EntityNotFoundException($"Offer with ID {id} not found");
            }
            return offer;
        }

        public async Task<IEnumerable<Offer>> GetAllOffersAsync()
        {
            return await _offerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Offer>> GetClientOffersAsync(int clientId)
        {
            return await _offerRepository.GetClientOffersAsync(clientId);
        }

        public async Task<bool> AddRealEstateToOfferAsync(int offerId, int realEstateId)
        {
            // Check if offer exists
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                throw new EntityNotFoundException($"Offer with ID {offerId} not found");
            }

            // Check if real estate exists
            if (!await _realEstateRepository.ExistsAsync(realEstateId))
            {
                throw new EntityNotFoundException($"Real estate with ID {realEstateId} not found");
            }

            return await _offerRepository.AddRealEstateToOfferAsync(offerId, realEstateId);
        }

        public async Task<bool> RemoveRealEstateFromOfferAsync(int offerId, int realEstateId)
        {
            // Check if offer exists
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                throw new EntityNotFoundException($"Offer with ID {offerId} not found");
            }

            // Check if real estate exists
            if (!await _realEstateRepository.ExistsAsync(realEstateId))
            {
                throw new EntityNotFoundException($"Real estate with ID {realEstateId} not found");
            }

            return await _offerRepository.RemoveRealEstateFromOfferAsync(offerId, realEstateId);
        }

        public async Task<IEnumerable<RealEstateProperty>> FindMatchingRealEstatesAsync(RealEstateType type, decimal maxPrice)
        {
            return await _realEstateRepository.FindByCriteriaAsync(type, 0, maxPrice);
        }

        private void ValidateOffer(Offer offer)
        {
            if (string.IsNullOrWhiteSpace(offer.Title))
                throw new ValidationException("Offer title is required");

            if (offer.ClientId <= 0)
                throw new ValidationException("Client ID is required");
        }
    }
}