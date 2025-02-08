using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxInvoiceManagment.Domain.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // e.g., "House", "Car"
        public string Type { get; set; } = null!; // "House" or "Car"
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<TaxOrService> TaxesOrServices { get; set; } = new List<TaxOrService>();
    }
}
