namespace TaxInvoiceManagment.Application.Models.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int TaxId { get; set; }
        public int Number { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal InvoiceAmount { get; set; }
        public bool? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime PrimaryDueDate { get; set; }
        public DateTime? SecondaryDueDate { get; set; }
        public string? InvoiceReceiptPath { get; set; }
        public string? PaymentReceiptPath { get; set; }
        public string? Notes { get; set; }
    }
}
