using RealEstateCompany.DAL.Entities;
using RealEstateCompany.BLL.Services;

namespace RealEstateCompany.PL.ConsoleHelpers
{
    public static class SearchMenuHelper
    {
        public static void SearchClients(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Search Clients ===");

            Console.Write("Enter search keyword: ");
            var keyword = Console.ReadLine() ?? "";

            var results = service.Search(keyword);
            Console.WriteLine($"Found {results.Count()} clients:");

            foreach (var client in results)
            {
                Console.WriteLine($"ID: {client.Id}, Name: {client.FullName}, Bank: {client.BankAccount}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void SearchRealEstates(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Search Real Estates ===");

            Console.Write("Enter search keyword: ");
            var keyword = Console.ReadLine() ?? "";

            var results = service.Search(keyword);
            Console.WriteLine($"Found {results.Count()} real estates:");

            foreach (var realEstate in results)
            {
                Console.WriteLine($"ID: {realEstate.Id}, Type: {realEstate.Type}, Address: {realEstate.Address}, Price: {realEstate.Price:C}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void GlobalSearch(IClientService clientService, IRealEstateService realEstateService)
        {
            Console.Clear();
            Console.WriteLine("=== Global Search ===");

            Console.Write("Enter search keyword: ");
            var keyword = Console.ReadLine() ?? "";

            var clients = clientService.Search(keyword);
            var realEstates = realEstateService.Search(keyword);

            Console.WriteLine("=== Clients ===");
            foreach (var client in clients)
            {
                Console.WriteLine($"Client ID: {client.Id}, Name: {client.FullName}, Bank: {client.BankAccount}");
            }

            Console.WriteLine("=== Real Estates ===");
            foreach (var realEstate in realEstates)
            {
                Console.WriteLine($"Real Estate ID: {realEstate.Id}, Type: {realEstate.Type}, Address: {realEstate.Address}, Price: {realEstate.Price:C}");
            }

            Console.WriteLine($"Total found: {clients.Count()} clients, {realEstates.Count()} real estates");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void AdvancedClientSearch(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Advanced Client Search ===");

            Console.Write("Enter last name (or leave empty): ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Desired Property Type (leave empty for any):");
            Console.WriteLine("1. One Room Apartment");
            Console.WriteLine("2. Two Room Apartment");
            Console.WriteLine("3. Three Room Apartment");
            Console.WriteLine("4. Private Plot");
            Console.Write("Select type (1-4): ");
            var typeInput = Console.ReadLine();
            RealEstateType? desiredType = null;

            if (!string.IsNullOrWhiteSpace(typeInput) && int.TryParse(typeInput, out int typeValue) && typeValue >= 1 && typeValue <= 4)
            {
                desiredType = (RealEstateType)(typeValue - 1);
            }

            var results = service.AdvancedSearch(lastName ?? "", desiredType);
            Console.WriteLine($"Found {results.Count()} clients:");

            foreach (var client in results)
            {
                Console.WriteLine($"ID: {client.Id}, Name: {client.FullName}, Desired: {client.DesiredType}, Max Price: {client.DesiredPrice:C}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}