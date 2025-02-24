using System.IO;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Domain.Interfaces;

namespace TaxInvoiceManagment.Infrastructure.FileStorage
{
    public class FileSystemService : IFileSystemService
    {
        private readonly ILogger<FileSystemService> _logger;
        private readonly string _basePath = "C:\\Invoices"; // Cambia esto según tu configuración.

        public FileSystemService(ILogger<FileSystemService> logger)
        {
            _logger = logger;
        }

        public void CreateUserFolder(int userId, string userName)
        {
            string path = Path.Combine(_basePath, $"{userId}_{userName}");
            CreateFolder(path);
        }

        public void CreateTaxableItemFolder(int userId, int taxableItemId, string taxableItemName)
        {
            string path = Path.Combine(_basePath, $"{userId}", "TaxableItems", $"{taxableItemId}_{taxableItemName}");
            CreateFolder(path);
        }

        public void CreateTaxFolder(int userId, int taxableItemId, int taxId, string taxName)
        {
            string path = Path.Combine(_basePath, $"{userId}", "TaxableItems", $"{taxableItemId}", $"{taxId}_{taxName}");
            CreateFolder(path);

            string invoicesPath = Path.Combine(path, "Invoices");
            CreateFolder(invoicesPath);
        }

        public void CreateInvoiceFolder(int userId, int taxableItemId, int taxId, int invoiceId, string month)
        {
            string path = Path.Combine(_basePath, $"{userId}", "TaxableItems", $"{taxableItemId}", $"{taxId}", "Invoices", $"{invoiceId}_{month}");
            CreateFolder(path);
        }

        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation($"Carpeta creada: {path}");
            }
        }
    }
}
