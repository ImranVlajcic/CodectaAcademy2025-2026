using ExpenseTracker.Application.StandardExpenseFolders.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Infrastructure.BackgroundServices
{
    public class StandardExpenseBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StandardExpenseBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); 

        public StandardExpenseBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<StandardExpenseBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Standard Expense Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    if (now.Hour == 0 && now.Minute < 5) 
                    {
                        await ProcessStandardExpensesAsync(stoppingToken);
                    }

                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Standard Expense Background Service");

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }

            _logger.LogInformation("Standard Expense Background Service stopped");
        }

        private async Task ProcessStandardExpensesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing standard expenses at {Time}", DateTime.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var processor = scope.ServiceProvider.GetRequiredService<StandardExpenseProcessor>();

                var result = await processor.ProcessDueExpensesAsync(cancellationToken);

                if (result.IsError)
                {
                    _logger.LogError("Failed to process standard expenses: {Errors}",
                        string.Join(", ", result.Errors));
                }
                else
                {
                    _logger.LogInformation("Successfully processed {Count} standard expenses", result.Value);
                }
            }
        }
    }
}