namespace TaxInvoiceManagment.Domain.Models
{
    public class TaxOrService
    {
        /*
         * Id - PK.
         * AssetId - FK.
         * Nombre del servicio.
         * Descripcion del servicio.
         * Nombre del responsable.
         * Tipo de servicio.
         * Frecuencia de pago.
         * Pago anual.
         * Numero de cliente.
         */

        public enum PaymentFrequency
        {
            Monthly,       // Pago mensual
            Quarterly,     // Pago trimestral            
            SemiAnnually,  // Pago semestral
            Annually,      // Pago anual
        }

        public int Id { get; set; } //PK
        public int TaxableItemId { get; set; } //FK - Ojo, es nulleable, controlarlo por FluentValidation
        public TaxableItem TaxableItem { get; set; } = null!; //??


        public string? ServiceName { get; set; }
        public string ? ServiceDescription { get; set; }
        public string? Owner { get; set; }
        public string? ServiceType { get; set; }
        public PaymentFrequency PayFrequency { get; set; }
        public bool AnnualPayment { get; set; }
        public string? ClientNumber { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
