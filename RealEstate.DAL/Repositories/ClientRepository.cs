using RealEstateApp.DAL.Interfaces;
using RealEstateApp.DAL.Models;

namespace RealEstateApp.DAL.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository() : base("clients.json") { }

        public IEnumerable<Client> SortByName() =>
            _entities.OrderBy(c => c.FirstName);

        public IEnumerable<Client> SortByLastName() =>
            _entities.OrderBy(c => c.LastName);

        public IEnumerable<Client> SortByBankAccountPrefix(string prefix) =>
            _entities.Where(c => c.BankAccount.StartsWith(prefix))
                    .OrderBy(c => c.BankAccount);

        public override IEnumerable<Client> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAll();

            return _entities.Where(c =>
                c.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.BankAccount.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.ContactInfo.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}