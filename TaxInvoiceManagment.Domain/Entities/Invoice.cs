namespace TaxInvoiceManagment.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; } // PK
        public int TaxId { get; set; } // FK      
        public int Number { get; set; }
        public string Month { get; set; }
        public decimal InvoiceAmount { get; set; }
        public bool? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime PrimaryDueDate { get; set; }
        public DateTime? SecondaryDueDate { get; set; }
        public string? InvoiceReceiptPath { get; set; }
        public string? PaymentReceiptPath { get; set; }
        public string? Notes { get; set; }
        public Tax Tax { get; set; } = null!; // ??
    }
}
