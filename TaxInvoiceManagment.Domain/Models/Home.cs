﻿namespace TaxInvoiceManagment.Domain.Models
{
    public class Home
    {
        public int Id { get; set; } //PK
        public int UserId { get; set; } //FK
        public User User { get; set; } = null!; //??


        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Address { get; set; }
        public ICollection<TaxOrService> TaxesOrServices { get; set; } = new List<TaxOrService>();
    }
}
