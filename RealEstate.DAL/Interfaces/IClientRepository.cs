using RealEstateApp.DAL.Models;

namespace RealEstateApp.DAL.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        IEnumerable<Client> SortByName();
        IEnumerable<Client> SortByLastName();
        IEnumerable<Client> SortByBankAccountPrefix(string prefix);
    }
}