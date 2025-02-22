namespace TaxInvoiceManagment.Application.Models.Dtos
{
    public class TaxDto
    {
        public int Id { get; set; }
        public int TaxableItemId { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set; }
        public string? Owner { get; set; }
        public string? ServiceType { get; set; }
        public string PayFrequency { get; set; } = "Monthly";
        public bool AnnualPayment { get; set; }
        public string? ClientNumber { get; set; }
        //public bool? AutoDebit { get; set; } // ToDo: Next feature.
    }
}
