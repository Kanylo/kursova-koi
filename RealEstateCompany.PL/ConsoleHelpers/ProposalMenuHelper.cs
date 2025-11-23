using RealEstateCompany.DAL.Entities;
using RealEstateCompany.BLL.Services;

namespace RealEstateCompany.PL.ConsoleHelpers
{
    public static class ProposalMenuHelper
    {
        public static void AddToProposal(IProposalService proposalService, IClientService clientService, IRealEstateService realEstateService)
        {
            Console.Clear();
            Console.WriteLine("=== Add Property to Client Proposal ===");

            try
            {
                Console.Write("Enter Client ID: ");
                if (!int.TryParse(Console.ReadLine(), out int clientId))
                {
                    Console.WriteLine("Invalid Client ID format.");
                    return;
                }

                Console.Write("Enter Real Estate ID: ");
                if (!int.TryParse(Console.ReadLine(), out int realEstateId))
                {
                    Console.WriteLine("Invalid Real Estate ID format.");
                    return;
                }

                proposalService.AddToProposal(clientId, realEstateId);
                Console.WriteLine("Property added to client proposal successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void RemoveFromProposal(IProposalService proposalService, IClientService clientService, IRealEstateService realEstateService)
        {
            Console.Clear();
            Console.WriteLine("=== Remove Property from Client Proposal ===");

            try
            {
                Console.Write("Enter Client ID: ");
                if (!int.TryParse(Console.ReadLine(), out int clientId))
                {
                    Console.WriteLine("Invalid Client ID format.");
                    return;
                }

                Console.Write("Enter Real Estate ID: ");
                if (!int.TryParse(Console.ReadLine(), out int realEstateId))
                {
                    Console.WriteLine("Invalid Real Estate ID format.");
                    return;
                }

                proposalService.RemoveFromProposal(clientId, realEstateId);
                Console.WriteLine("Property removed from client proposal successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void ViewClientProposals(IProposalService proposalService, IClientService clientService)
        {
            Console.Clear();
            Console.WriteLine("=== Client Proposals ===");

            Console.Write("Enter Client ID: ");
            if (int.TryParse(Console.ReadLine(), out int clientId))
            {
                var client = clientService.GetClientById(clientId);
                if (client != null)
                {
                    var proposals = proposalService.GetClientProposals(clientId);
                    Console.WriteLine($"Proposals for {client.FullName}:");

                    if (!proposals.Any())
                    {
                        Console.WriteLine("No proposals found.");
                    }
                    else
                    {
                        foreach (var proposal in proposals)
                        {
                            Console.WriteLine($"ID: {proposal.Id}, Address: {proposal.Address}, Type: {proposal.Type}, Price: {proposal.Price:C}");
                        }
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

        public static void CheckAvailability(IProposalService proposalService)
        {
            Console.Clear();
            Console.WriteLine("=== Check Property Availability ===");

            try
            {
                Console.WriteLine("Property Type:");
                Console.WriteLine("1. One Room Apartment");
                Console.WriteLine("2. Two Room Apartment");
                Console.WriteLine("3. Three Room Apartment");
                Console.WriteLine("4. Private Plot");
                Console.Write("Select type (1-4): ");
                var typeInput = Console.ReadLine();
                if (!int.TryParse(typeInput, out int typeValue) || typeValue < 1 || typeValue > 4)
                {
                    Console.WriteLine("Invalid type selected.");
                    return;
                }

                Console.Write("Enter maximum price: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                {
                    Console.WriteLine("Invalid price format.");
                    return;
                }

                var type = (RealEstateType)(typeValue - 1);
                var isAvailable = proposalService.CheckAvailability(type, maxPrice);

                if (isAvailable)
                {
                    Console.WriteLine($"Properties of type {type} with price up to {maxPrice:C} are available!");
                }
                else
                {
                    Console.WriteLine($"No properties of type {type} with price up to {maxPrice:C} are currently available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}