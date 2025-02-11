namespace TaxInvoiceManagment.Domain.Models
{
    public class TaxableItem
    {
        /*
         * Id - PK.
         * UserId - FK.
         * Nombre de la propiedad.
         * Tipo de propiedad.
         * Nombre de calle.
         * Numero de piso.
         * Departamento.
         */
        public int Id { get; set; } //PK
        public int UserId { get; set; } //FK
        public User User { get; set; } = null!; //??


        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Address { get; set; }
        public string? VehicleNumberPlate { get; set; }
        public ICollection<TaxOrService> TaxesOrServices { get; set; } = new List<TaxOrService>();
    }
}
