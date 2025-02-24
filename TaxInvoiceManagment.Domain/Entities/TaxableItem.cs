namespace TaxInvoiceManagment.Domain.Entities
{
    public class TaxableItem
    {
        public int Id { get; set; } //PK
        public int UserId { get; set; } //FK
        public User User { get; set; } = null!; //??
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Address { get; set; }
        public string? VehicleNumberPlate { get; set; }
        public ICollection<Tax> Taxes { get; } = new List<Tax>();
    }
}
