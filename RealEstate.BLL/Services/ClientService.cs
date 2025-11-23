using RealEstateApp.BLL.Exceptions;
using RealEstateApp.DAL.Interfaces;
using RealEstateApp.DAL.Models;

namespace RealEstateApp.BLL.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ??
                throw new ArgumentNullException(nameof(clientRepository));
        }

        public IEnumerable<Client> GetAllClients() => _clientRepository.GetAll();

        public Client GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);
            return client ?? throw new NotFoundException("Client", id);
        }

        public void AddClient(Client client)
        {
            ValidateClient(client);
            _clientRepository.Add(client);
        }

        public void UpdateClient(Client client)
        {
            ValidateClient(client);
            if (_clientRepository.GetById(client.Id) == null)
                throw new NotFoundException("Client", client.Id);

            _clientRepository.Update(client);
        }

        public void DeleteClient(int id)
        {
            if (_clientRepository.GetById(id) == null)
                throw new NotFoundException("Client", id);

            _clientRepository.Delete(id);
        }

        public IEnumerable<Client> SearchClients(string keyword) =>
            _clientRepository.Search(keyword);

        public IEnumerable<Client> SortClientsByName() =>
            _clientRepository.SortByName();

        public IEnumerable<Client> SortClientsByLastName() =>
            _clientRepository.SortByLastName();

        public IEnumerable<Client> SortClientsByBankAccountPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix) || !prefix.All(char.IsDigit))
                throw new ValidationException("Bank account prefix must contain only digits");

            return _clientRepository.SortByBankAccountPrefix(prefix);
        }

        private void ValidateClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (string.IsNullOrWhiteSpace(client.FirstName))
                throw new ValidationException("First name is required");

            if (string.IsNullOrWhiteSpace(client.LastName))
                throw new ValidationException("Last name is required");

            if (string.IsNullOrWhiteSpace(client.BankAccount) ||
                !client.BankAccount.All(char.IsDigit))
                throw new ValidationException("Bank account must contain only digits");

            if (string.IsNullOrWhiteSpace(client.ContactInfo))
                throw new ValidationException("Contact info is required");
        }
    }
}