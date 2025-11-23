namespace RealEstateApp.DAL.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public List<int> RealEstateIds { get; set; } = new List<int>();
        public DateTime OfferDate { get; set; } = DateTime.UtcNow;
        public Dictionary<int, bool> ClientResponses { get; set; } = new Dictionary<int, bool>();
    }
}