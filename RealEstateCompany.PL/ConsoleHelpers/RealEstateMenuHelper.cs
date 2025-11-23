using RealEstateCompany.DAL.Entities;
using RealEstateCompany.BLL.Services;

namespace RealEstateCompany.PL.ConsoleHelpers
{
    public static class RealEstateMenuHelper
    {
        public static void AddRealEstate(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Add New Real Estate ===");

            try
            {
                var realEstate = new RealEstate();

                Console.Write("Address: ");
                realEstate.Address = Console.ReadLine() ?? "";

                Console.WriteLine("Property Type:");
                Console.WriteLine("1. One Room Apartment");
                Console.WriteLine("2. Two Room Apartment");
                Console.WriteLine("3. Three Room Apartment");
                Console.WriteLine("4. Private Plot");
                Console.Write("Select type (1-4): ");
                var typeInput = Console.ReadLine();
                if (int.TryParse(typeInput, out int typeValue) && typeValue >= 1 && typeValue <= 4)
                {
                    realEstate.Type = (RealEstateType)(typeValue - 1);
                }
                else
                {
                    Console.WriteLine("Invalid type selected. Defaulting to One Room Apartment.");
                    realEstate.Type = RealEstateType.OneRoomApartment;
                }

                Console.Write("Price: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal price))
                {
                    realEstate.Price = price;
                }
                else
                {
                    Console.WriteLine("Invalid price. Setting to 0.");
                    realEstate.Price = 0;
                }

                Console.Write("Area (sq m): ");
                if (double.TryParse(Console.ReadLine(), out double area))
                {
                    realEstate.Area = area;
                }
                else
                {
                    Console.WriteLine("Invalid area. Setting to 0.");
                    realEstate.Area = 0;
                }

                if (realEstate.Type != RealEstateType.PrivatePlot)
                {
                    Console.Write("Number of Rooms: ");
                    if (int.TryParse(Console.ReadLine(), out int rooms))
                    {
                        realEstate.Rooms = rooms;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room count. Setting to 0.");
                        realEstate.Rooms = 0;
                    }
                }

                service.AddRealEstate(realEstate);
                Console.WriteLine("Real estate added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewAllRealEstates(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== All Real Estates ===");

            var realEstates = service.GetAllRealEstates();
            if (!realEstates.Any())
            {
                Console.WriteLine("No real estates found.");
            }
            else
            {
                foreach (var realEstate in realEstates)
                {
                    Console.WriteLine($"ID: {realEstate.Id}, Type: {realEstate.Type}, Address: {realEstate.Address}, Price: {realEstate.Price:C}, Available: {realEstate.IsAvailable}");
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewRealEstateDetails(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Real Estate Details ===");

            Console.Write("Enter Real Estate ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var realEstate = service.GetRealEstateById(id);
                if (realEstate != null)
                {
                    Console.WriteLine($"ID: {realEstate.Id}");
                    Console.WriteLine($"Address: {realEstate.Address}");
                    Console.WriteLine($"Type: {realEstate.Type}");
                    Console.WriteLine($"Price: {realEstate.Price:C}");
                    Console.WriteLine($"Area: {realEstate.Area} sq m");
                    if (realEstate.Type != RealEstateType.PrivatePlot)
                    {
                        Console.WriteLine($"Rooms: {realEstate.Rooms}");
                    }
                    Console.WriteLine($"Available: {realEstate.IsAvailable}");
                }
                else
                {
                    Console.WriteLine("Real estate not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void UpdateRealEstate(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Update Real Estate ===");

            Console.Write("Enter Real Estate ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var realEstate = service.GetRealEstateById(id);
                if (realEstate != null)
                {
                    try
                    {
                        Console.Write($"Address ({realEstate.Address}): ");
                        var address = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(address))
                            realEstate.Address = address;

                        Console.WriteLine($"Current Type: {realEstate.Type}");
                        Console.WriteLine("New Property Type (leave empty to keep current):");
                        Console.WriteLine("1. One Room Apartment");
                        Console.WriteLine("2. Two Room Apartment");
                        Console.WriteLine("3. Three Room Apartment");
                        Console.WriteLine("4. Private Plot");
                        Console.Write("Select type (1-4): ");
                        var typeInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(typeInput) && int.TryParse(typeInput, out int typeValue) && typeValue >= 1 && typeValue <= 4)
                        {
                            realEstate.Type = (RealEstateType)(typeValue - 1);
                        }

                        Console.Write($"Price ({realEstate.Price:C}): ");
                        var priceInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal price))
                        {
                            realEstate.Price = price;
                        }

                        Console.Write($"Area ({realEstate.Area} sq m): ");
                        var areaInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(areaInput) && double.TryParse(areaInput, out double area))
                        {
                            realEstate.Area = area;
                        }

                        if (realEstate.Type != RealEstateType.PrivatePlot)
                        {
                            Console.Write($"Rooms ({realEstate.Rooms}): ");
                            var roomsInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(roomsInput) && int.TryParse(roomsInput, out int rooms))
                            {
                                realEstate.Rooms = rooms;
                            }
                        }

                        service.UpdateRealEstate(realEstate);
                        Console.WriteLine("Real estate updated successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Real estate not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void DeleteRealEstate(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Delete Real Estate ===");

            Console.Write("Enter Real Estate ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var realEstate = service.GetRealEstateById(id);
                if (realEstate != null)
                {
                    Console.Write($"Are you sure you want to delete {realEstate.Address}? (y/n): ");
                    var confirm = Console.ReadLine()?.ToLower();
                    if (confirm == "y")
                    {
                        service.DeleteRealEstate(id);
                        Console.WriteLine("Real estate deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.");
                    }
                }
                else
                {
                    Console.WriteLine("Real estate not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void SortRealEstates(IRealEstateService service)
        {
            Console.Clear();
            Console.WriteLine("=== Sort Real Estates ===");
            Console.WriteLine("1. Sort by Type");
            Console.WriteLine("2. Sort by Price");
            Console.Write("Select sorting option: ");

            var choice = Console.ReadLine();
            IEnumerable<RealEstate> sortedRealEstates;

            switch (choice)
            {
                case "1":
                    sortedRealEstates = service.SortByType();
                    Console.WriteLine("=== Real Estates Sorted by Type ===");
                    break;
                case "2":
                    sortedRealEstates = service.SortByPrice();
                    Console.WriteLine("=== Real Estates Sorted by Price ===");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    return;
            }

            foreach (var realEstate in sortedRealEstates)
            {
                Console.WriteLine($"ID: {realEstate.Id}, Type: {realEstate.Type}, Address: {realEstate.Address}, Price: {realEstate.Price:C}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}