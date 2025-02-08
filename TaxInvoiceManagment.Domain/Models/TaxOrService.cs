using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxInvoiceManagment.Domain.Models
{
    public class TaxOrService
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
