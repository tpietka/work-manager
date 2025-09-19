using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using WorkManager.Infrastructure.Data;

namespace WorkManager.WPF;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;
    protected override void OnStartup(StartupEventArgs e)
    {
        _host = CreateHostBuilder().Build();

        // Initialize database on startup
        using (var scope = _host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WorkManagerDbContext>();
            try
            {
                // Ensure database is created and up to date
                context.Database.EnsureCreated();

                // For production, use: context.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Log database initialization errors
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();
                logger.LogError(ex, "Failed to initialize database during startup");

                MessageBox.Show($"Database initialization failed: {ex.Message}",
                               "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Database Configuration
                services.AddDbContext<WorkManagerDbContext>(options =>
                {
                    var connectionString = WorkManagerDbContext.GetConnectionString();
                    options.UseSqlite(connectionString, sqliteOptions =>
                    {
                        sqliteOptions.CommandTimeout(30);
                    });

                    // Performance optimizations
                    options.EnableServiceProviderCaching();
                    options.EnableSensitiveDataLogging(false); // Set to true in development only
                });

                // Register main window
                services.AddTransient<MainWindow>();

                // Logging configuration
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            });
}

