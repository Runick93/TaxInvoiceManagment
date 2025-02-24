using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Domain.Interfaces;

namespace TaxInvoiceManagment.Infrastructure.Logging
{
    public class SerilogLoggingService : ILoggingService
    {
        private readonly ILogger<SerilogLoggingService> _logger;

        public SerilogLoggingService(ILogger<SerilogLoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            _logger.LogError(ex, message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }
    }
}
