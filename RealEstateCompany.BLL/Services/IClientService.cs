using RealEstateCompany.DAL.Entities;

namespace RealEstateCompany.BLL.Services
{
    public interface IClientService
    {
        IEnumerable<Client> GetAllClients();
        Client? GetClientById(int id);
        void AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(int id);

        IEnumerable<Client> SortByName();
        IEnumerable<Client> SortByLastName();
        IEnumerable<Client> SortByBankAccountFirstDigit();

        IEnumerable<Client> Search(string keyword);
        IEnumerable<Client> AdvancedSearch(string lastName, RealEstateType? desiredType);
    }
}