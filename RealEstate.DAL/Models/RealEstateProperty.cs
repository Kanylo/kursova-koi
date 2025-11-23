namespace RealEstate.DAL.Models
{
    public enum RealEstateType
    {
        OneRoomApartment,
        TwoRoomApartment,
        ThreeRoomApartment,
        PrivatePlot
    }

    public class RealEstateProperty
    {
        public int Id { get; set; }
        public RealEstateType Type { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Area { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}