using RealEstateCompany.DAL.Entities;
using RealEstateCompany.DAL.Repositories;
using RealEstateCompany.BLL.Services;
using RealEstateCompany.PL.ConsoleHelpers;
using RealEstateCompany.BLL.Exceptions;

namespace RealEstateCompany.PL
{
    class Program
    {
        private static IClientService _clientService = null!;
        private static IRealEstateService _realEstateService = null!;
        private static IProposalService _proposalService = null!;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            try
            {
                InitializeServices();
                Console.WriteLine("=== Real Estate Company Management System ===");
                Console.WriteLine("Система успішно ініціалізована!");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
                ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критична помилка при запуску програми: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
                Console.ReadKey();
            }
        }

        static void InitializeServices()
        {
            try
            {
                var clientRepo = new JsonRepository<Client>("clients.json");
                var realEstateRepo = new JsonRepository<RealEstate>("realestates.json");

                _clientService = new ClientService(clientRepo);
                _realEstateService = new RealEstateService(realEstateRepo);
                _proposalService = new ProposalService(clientRepo, realEstateRepo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка ініціалізації сервісів: {ex.Message}", ex);
            }
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Система управління ріелторською фірмою ===");
                    Console.WriteLine("1. Управління клієнтами");
                    Console.WriteLine("2. Управління нерухомістю");
                    Console.WriteLine("3. Управління пропозиціями");
                    Console.WriteLine("4. Пошук");
                    Console.WriteLine("5. Вихід");
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ShowClientMenu(); break;
                        case "2": ShowRealEstateMenu(); break;
                        case "3": ShowProposalMenu(); break;
                        case "4": ShowSearchMenu(); break;
                        case "5":
                            Console.WriteLine("Дякуємо за використання програми!");
                            return;
                        default:
                            Console.WriteLine("Некоректна опція! Спробуйте ще раз.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        static void ShowClientMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Управління клієнтами ===");
                    Console.WriteLine("1. Додати клієнта");
                    Console.WriteLine("2. Переглянути всіх клієнтів");
                    Console.WriteLine("3. Переглянути деталі клієнта");
                    Console.WriteLine("4. Оновити дані клієнта");
                    Console.WriteLine("5. Видалити клієнта");
                    Console.WriteLine("6. Сортувати клієнтів");
                    Console.WriteLine("7. Повернутися до головного меню");
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ClientMenuHelper.AddClient(_clientService); break;
                        case "2": ClientMenuHelper.ViewAllClients(_clientService); break;
                        case "3": ClientMenuHelper.ViewClientDetails(_clientService); break;
                        case "4": ClientMenuHelper.UpdateClient(_clientService); break;
                        case "5": ClientMenuHelper.DeleteClient(_clientService); break;
                        case "6": ClientMenuHelper.SortClients(_clientService); break;
                        case "7": return;
                        default:
                            Console.WriteLine("Некоректна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        static void ShowRealEstateMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Управління нерухомістю ===");
                    Console.WriteLine("1. Додати об'єкт нерухомості");
                    Console.WriteLine("2. Переглянути всі об'єкти нерухомості");
                    Console.WriteLine("3. Переглянути деталі об'єкта");
                    Console.WriteLine("4. Оновити дані об'єкта");
                    Console.WriteLine("5. Видалити об'єкт нерухомості");
                    Console.WriteLine("6. Сортувати нерухомість");
                    Console.WriteLine("7. Повернутися до головного меню");
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": RealEstateMenuHelper.AddRealEstate(_realEstateService); break;
                        case "2": RealEstateMenuHelper.ViewAllRealEstates(_realEstateService); break;
                        case "3": RealEstateMenuHelper.ViewRealEstateDetails(_realEstateService); break;
                        case "4": RealEstateMenuHelper.UpdateRealEstate(_realEstateService); break;
                        case "5": RealEstateMenuHelper.DeleteRealEstate(_realEstateService); break;
                        case "6": RealEstateMenuHelper.SortRealEstates(_realEstateService); break;
                        case "7": return;
                        default:
                            Console.WriteLine("Некоректна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        static void ShowProposalMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Управління пропозиціями ===");
                    Console.WriteLine("1. Додати нерухомість до пропозиції клієнта");
                    Console.WriteLine("2. Видалити нерухомість з пропозиції клієнта");
                    Console.WriteLine("3. Переглянути пропозиції клієнта");
                    Console.WriteLine("4. Перевірити доступність нерухомості");
                    Console.WriteLine("5. Повернутися до головного меню");
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ProposalMenuHelper.AddToProposal(_proposalService, _clientService, _realEstateService); break;
                        case "2": ProposalMenuHelper.RemoveFromProposal(_proposalService, _clientService, _realEstateService); break;
                        case "3": ProposalMenuHelper.ViewClientProposals(_proposalService, _clientService); break;
                        case "4": ProposalMenuHelper.CheckAvailability(_proposalService); break;
                        case "5": return;
                        default:
                            Console.WriteLine("Некоректна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        static void ShowSearchMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Пошук ===");
                    Console.WriteLine("1. Пошук клієнтів");
                    Console.WriteLine("2. Пошук нерухомості");
                    Console.WriteLine("3. Глобальний пошук");
                    Console.WriteLine("4. Розширений пошук клієнтів");
                    Console.WriteLine("5. Повернутися до головного меню");
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": SearchMenuHelper.SearchClients(_clientService); break;
                        case "2": SearchMenuHelper.SearchRealEstates(_realEstateService); break;
                        case "3": SearchMenuHelper.GlobalSearch(_clientService, _realEstateService); break;
                        case "4": SearchMenuHelper.AdvancedClientSearch(_clientService); break;
                        case "5": return;
                        default:
                            Console.WriteLine("Некоректна опція!");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        static void HandleException(Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("=== Сталася помилка ===");

            switch (ex)
            {
                case RealEstateException realEstateEx:
                    Console.WriteLine($"Помилка бізнес-логіки: {realEstateEx.Message}");
                    break;
                case ArgumentException argEx:
                    Console.WriteLine($"Помилка введених даних: {argEx.Message}");
                    break;
                case InvalidOperationException opEx:
                    Console.WriteLine($"Операція не може бути виконана: {opEx.Message}");
                    break;
                case System.Text.Json.JsonException jsonEx:
                    Console.WriteLine("Помилка читання даних. Можливо, файли пошкоджені.");
                    break;
                case IOException ioEx:
                    Console.WriteLine($"Помилка роботи з файлом: {ioEx.Message}");
                    break;
                default:
                    Console.WriteLine($"Неочікувана помилка: {ex.Message}");
                    break;
            }

            Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }
}