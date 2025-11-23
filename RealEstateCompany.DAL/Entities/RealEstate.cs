namespace RealEstateCompany.DAL.Entities
{
    public class RealEstate
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public RealEstateType Type { get; set; }
        public decimal Price { get; set; }
        public double Area { get; set; }
        public int Rooms { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public enum RealEstateType
    {
        OneRoomApartment,
        TwoRoomApartment,
        ThreeRoomApartment,
        PrivatePlot
    }
}