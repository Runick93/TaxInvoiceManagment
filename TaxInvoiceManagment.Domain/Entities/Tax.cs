namespace TaxInvoiceManagment.Domain.Entities
{
    public class Tax
    {
        public enum PaymentFrequency
        {
            Monthly,      
            Quarterly,              
            SemiAnnually,  
            Annually,      
        }

        public int Id { get; set; } //PK
        public int TaxableItemId { get; set; } //FK
        public TaxableItem TaxableItem { get; set; } = null!; //??
        public string? Name { get; set; }
        public string ? ServiceDescription { get; set; }
        public string? Owner { get; set; }
        public string? ServiceType { get; set; }
        public PaymentFrequency PayFrequency { get; set; }
        public bool AnnualPayment { get; set; }
        public string? ClientNumber { get; set; }
        public bool? AutoDebit { get; set; }
        public ICollection<Invoice> Invoices { get; } = new List<Invoice>();
    }
}
