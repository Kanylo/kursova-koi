namespace RealEstateApp.DAL.Models
{
    public enum RealEstateType
    {
        OneRoom = 1,
        TwoRooms = 2,
        ThreeRooms = 3,
        PrivatePlot = 4
    }

    public class RealEstate
    {
        public int Id { get; set; }
        public RealEstateType Type { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Area { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}