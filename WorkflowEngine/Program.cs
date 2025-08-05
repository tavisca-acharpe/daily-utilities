using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WorkflowEngine.Services;

public class Program
{
    public static async Task Main(string[] args)
    {

        using IHost host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                configBuilder.AddJsonFile($"appsettings.json", optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                // Register step implementations
                services.AddTransient<FraudCheck>();
                services.AddTransient<Payment>();
                services.AddTransient<Cancel>();
                services.AddTransient<Book>();
                services.AddTransient<PostBooking>();

                // Register the executor
                services.AddSingleton<WorkflowExecutor>();
            })
            .Build();

        var executor = host.Services.GetRequiredService<WorkflowExecutor>();

        // Run workflow
        await executor.ExecuteWorkflowAsync("Workflow1");
    }
}
