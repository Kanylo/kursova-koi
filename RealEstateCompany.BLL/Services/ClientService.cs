using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;

namespace RealEstateCompany.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _repository;

        public ClientService(IRepository<Client> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Client> GetAllClients() => _repository.GetAll();

        public Client? GetClientById(int id) => _repository.GetById(id);

        public void AddClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ValidateClient(client);
            _repository.Add(client);
        }

        public void UpdateClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ValidateClient(client);
            _repository.Update(client);
        }

        public void DeleteClient(int id) => _repository.Delete(id);

        public IEnumerable<Client> SortByName() =>
            _repository.GetAll().OrderBy(c => c.FirstName);

        public IEnumerable<Client> SortByLastName() =>
            _repository.GetAll().OrderBy(c => c.LastName);

        public IEnumerable<Client> SortByBankAccountFirstDigit() =>
            _repository.GetAll().OrderBy(c => c.BankAccount.FirstOrDefault());

        public IEnumerable<Client> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repository.GetAll();

            return _repository.GetAll().Where(c =>
                c.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.BankAccount.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Client> AdvancedSearch(string lastName, RealEstateType? desiredType)
        {
            var query = _repository.GetAll().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));

            if (desiredType.HasValue)
                query = query.Where(c => c.DesiredType == desiredType.Value);

            return query;
        }

        private void ValidateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.FirstName))
                throw new ArgumentException("First name is required");

            if (string.IsNullOrWhiteSpace(client.LastName))
                throw new ArgumentException("Last name is required");

            if (string.IsNullOrWhiteSpace(client.BankAccount))
                throw new ArgumentException("Bank account is required");
        }
    }
}