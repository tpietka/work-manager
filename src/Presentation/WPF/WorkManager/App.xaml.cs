using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Windows;
using WorkManager.Infrastructure.Data;

namespace WorkManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = CreateHostBuilder().Build();

        // Ensure database is created and up to date
        using (var scope = _host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            context.Database.EnsureCreated();
            // For production, use: context.Database.Migrate();
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
                services.AddDbContext<TodoDbContext>(options =>
                {
                    var connectionString = TodoDbContext.GetConnectionString();
                    options.UseSqlite(connectionString, sqliteOptions =>
                    {
                        sqliteOptions.CommandTimeout(30);
                    });

                    // Performance settings
                    options.EnableServiceProviderCaching();
                    options.EnableSensitiveDataLogging(false); // Set to false in production
                });

                // Views and ViewModels will be added later
                services.AddTransient<MainWindow>();

                // Logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            });
}
