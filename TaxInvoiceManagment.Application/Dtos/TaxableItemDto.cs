namespace TaxInvoiceManagment.Application.Dtos
{
    public class TaxableItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? VehicleNumberPlate { get; set; }
    }
}
