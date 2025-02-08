using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxInvoiceManagment.Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string PdfPath { get; set; } = null!;
        public bool IsPaid { get; set; }
        public DateTime DueDate1 { get; set; }
        public DateTime? DueDate2 { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public int TaxOrServiceId { get; set; }
        public TaxOrService TaxOrService { get; set; } = null!;
        public string? PaymentReference { get; set; }
        public string? Notes { get; set; }
    }
}
