using RealEstateApp.BLL.Exceptions;
using RealEstateApp.BLL.Services;
using RealEstateApp.DAL.Models;
using RealEstateApp.DAL.Repositories;
using RealEstateApp.PL.Utilities;

namespace RealEstateApp.PL
{
    class Program
    {
        private static ClientService _clientService = null!;
        private static RealEstateService _realEstateService = null!;
        private static OfferService _offerService = null!;

        static void Main(string[] args)
        {
            InitializeServices();
            ShowMainMenu();
        }

        static void InitializeServices()
        {
            var clientRepo = new ClientRepository();
            var realEstateRepo = new RealEstateRepository();
            var offerRepo = new OfferRepository();

            _clientService = new ClientService(clientRepo);
            _realEstateService = new RealEstateService(realEstateRepo);
            _offerService = new OfferService(offerRepo, realEstateRepo, clientRepo);
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Real Estate Management System ===");
                Console.WriteLine("1. Client Management");
                Console.WriteLine("2. Real Estate Management");
                Console.WriteLine("3. Offer Management");
                Console.WriteLine("4. Search");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowClientMenu();
                        break;
                    case "2":
                        ShowRealEstateMenu();
                        break;
                    case "3":
                        ShowOfferMenu();
                        break;
                    case "4":
                        ShowSearchMenu();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowClientMenu()
        {
            // Implementation for client management
            // Similar structure for other menus
        }
    }
}