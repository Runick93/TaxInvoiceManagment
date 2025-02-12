namespace TaxInvoiceManagment.Domain.Models
{
    public class User
    {
        /*
         * Id - PK.
         * UserName.
         * Email.
         * Password.
         */
        public int Id { get; set; } //PK
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public ICollection<TaxableItem> TaxableItems { get; set; } = new List<TaxableItem>();
    }
}
