using System.Text.Json.Serialization;

namespace RealEstateCompany.DAL.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string BankAccount { get; set; } = string.Empty;
        public RealEstateType DesiredType { get; set; }
        public decimal DesiredPrice { get; set; }
        public List<int> ProposedRealEstateIds { get; set; } = new();

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}