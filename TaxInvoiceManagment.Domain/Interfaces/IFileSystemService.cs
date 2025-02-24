namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IFileSystemService
    {
        void CreateInvoiceFolder(int userId, int taxableItemId, int taxId, int invoiceId, string month);
        void CreateTaxableItemFolder(int userId, int taxableItemId, string taxableItemName);
        void CreateTaxFolder(int userId, int taxableItemId, int taxId, string taxName);
        void CreateUserFolder(int userId, string userName);
    }
}