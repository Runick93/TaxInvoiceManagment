namespace TaxInvoiceManagment.Domain.Models
{
    public class Invoice
    {
        /*
         * Id - PK
         * TaxOrServiceId - FK
         * Numero de cuota.
         * Mes de la cuota.
         * Monto.
         * Estado del pago.
         * Fecha de pago.
         * Fecha de vencimiento 1.
         * Fecha de vencimiento 2.
         * Path de la boleta pdf.
         * Path del conprobante de pago pdf.
         * Notas.
         */
        public int Id { get; set; } // PK
        public int TaxOrServiceId { get; set; } // FK
        public TaxOrService TaxOrService { get; set; } = null!; // ??
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
    }
}
