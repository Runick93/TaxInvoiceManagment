namespace TaxInvoiceManagment.Domain.Models
{
    public class TaxOrService
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
        public string? ServiceName { get; set; }
        public string ? ServiceDescription { get; set; }
        public string? Owner { get; set; }
        public string? ServiceType { get; set; }
        public PaymentFrequency PayFrequency { get; set; }
        public bool AnnualPayment { get; set; }
        public string? ClientNumber { get; set; }
        //public bool? AutoDebit { get; set; } // ToDo: Next feature.
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
