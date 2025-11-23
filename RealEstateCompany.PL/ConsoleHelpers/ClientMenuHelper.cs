using RealEstateCompany.DAL.Entities;
using RealEstateCompany.BLL.Services;

namespace RealEstateCompany.PL.ConsoleHelpers
{
    public static class ClientMenuHelper
    {
        public static void AddClient(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Add New Client ===");

            try
            {
                var client = new Client();

                Console.Write("First Name: ");
                client.FirstName = Console.ReadLine() ?? "";

                Console.Write("Last Name: ");
                client.LastName = Console.ReadLine() ?? "";

                Console.Write("Bank Account: ");
                client.BankAccount = Console.ReadLine() ?? "";

                Console.WriteLine("Desired Property Type:");
                Console.WriteLine("1. One Room Apartment");
                Console.WriteLine("2. Two Room Apartment");
                Console.WriteLine("3. Three Room Apartment");
                Console.WriteLine("4. Private Plot");
                Console.Write("Select type (1-4): ");
                var typeInput = Console.ReadLine();
                if (int.TryParse(typeInput, out int typeValue) && typeValue >= 1 && typeValue <= 4)
                {
                    client.DesiredType = (RealEstateType)(typeValue - 1);
                }
                else
                {
                    Console.WriteLine("Invalid type selected. Defaulting to One Room Apartment.");
                    client.DesiredType = RealEstateType.OneRoomApartment;
                }

                Console.Write("Desired Max Price: ");
                var priceInput = Console.ReadLine();
                if (decimal.TryParse(priceInput, out decimal price))
                {
                    client.DesiredPrice = price;
                }
                else
                {
                    Console.WriteLine("Invalid price. Setting to 0.");
                    client.DesiredPrice = 0;
                }

                service.AddClient(client);
                Console.WriteLine("Client added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewAllClients(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== All Clients ===");

            var clients = service.GetAllClients();
            if (!clients.Any())
            {
                Console.WriteLine("No clients found.");
            }
            else
            {
                foreach (var client in clients)
                {
                    Console.WriteLine($"ID: {client.Id}, Name: {client.FullName}, Bank: {client.BankAccount}, Desired: {client.DesiredType} up to {client.DesiredPrice:C}");
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewClientDetails(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Client Details ===");

            Console.Write("Enter Client ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var client = service.GetClientById(id);
                if (client != null)
                {
                    Console.WriteLine($"ID: {client.Id}");
                    Console.WriteLine($"Name: {client.FullName}");
                    Console.WriteLine($"Bank Account: {client.BankAccount}");
                    Console.WriteLine($"Desired Property: {client.DesiredType}");
                    Console.WriteLine($"Max Price: {client.DesiredPrice:C}");
                    Console.WriteLine($"Proposed Properties: {client.ProposedRealEstateIds.Count}");
                }
                else
                {
                    Console.WriteLine("Client not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void UpdateClient(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Update Client ===");

            Console.Write("Enter Client ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var client = service.GetClientById(id);
                if (client != null)
                {
                    try
                    {
                        Console.Write($"First Name ({client.FirstName}): ");
                        var firstName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(firstName))
                            client.FirstName = firstName;

                        Console.Write($"Last Name ({client.LastName}): ");
                        var lastName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(lastName))
                            client.LastName = lastName;

                        Console.Write($"Bank Account ({client.BankAccount}): ");
                        var bankAccount = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(bankAccount))
                            client.BankAccount = bankAccount;

                        Console.WriteLine($"Current Desired Type: {client.DesiredType}");
                        Console.WriteLine("New Desired Property Type (leave empty to keep current):");
                        Console.WriteLine("1. One Room Apartment");
                        Console.WriteLine("2. Two Room Apartment");
                        Console.WriteLine("3. Three Room Apartment");
                        Console.WriteLine("4. Private Plot");
                        Console.Write("Select type (1-4): ");
                        var typeInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(typeInput) && int.TryParse(typeInput, out int typeValue) && typeValue >= 1 && typeValue <= 4)
                        {
                            client.DesiredType = (RealEstateType)(typeValue - 1);
                        }

                        Console.Write($"Desired Max Price ({client.DesiredPrice:C}): ");
                        var priceInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal price))
                        {
                            client.DesiredPrice = price;
                        }

                        service.UpdateClient(client);
                        Console.WriteLine("Client updated successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Client not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void DeleteClient(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Delete Client ===");

            Console.Write("Enter Client ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var client = service.GetClientById(id);
                if (client != null)
                {
                    Console.Write($"Are you sure you want to delete {client.FullName}? (y/n): ");
                    var confirm = Console.ReadLine()?.ToLower();
                    if (confirm == "y")
                    {
                        service.DeleteClient(id);
                        Console.WriteLine("Client deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.");
                    }
                }
                else
                {
                    Console.WriteLine("Client not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void SortClients(IClientService service)
        {
            Console.Clear();
            Console.WriteLine("=== Sort Clients ===");
            Console.WriteLine("1. Sort by First Name");
            Console.WriteLine("2. Sort by Last Name");
            Console.WriteLine("3. Sort by Bank Account First Digit");
            Console.Write("Select sorting option: ");

            var choice = Console.ReadLine();
            IEnumerable<Client> sortedClients;

            switch (choice)
            {
                case "1":
                    sortedClients = service.SortByName();
                    Console.WriteLine("=== Clients Sorted by First Name ===");
                    break;
                case "2":
                    sortedClients = service.SortByLastName();
                    Console.WriteLine("=== Clients Sorted by Last Name ===");
                    break;
                case "3":
                    sortedClients = service.SortByBankAccountFirstDigit();
                    Console.WriteLine("=== Clients Sorted by Bank Account First Digit ===");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    return;
            }

            foreach (var client in sortedClients)
            {
                Console.WriteLine($"ID: {client.Id}, Name: {client.FullName}, Bank: {client.BankAccount}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}