
namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogError(string message, Exception ex = null);
        void LogInformation(string message);
        void LogWarning(string message);
    }
}