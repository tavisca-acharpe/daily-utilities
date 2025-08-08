using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WorkflowEngine.Models;
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

        while (true)
        { 
            Console.Write("\n\nEnter workflow name  :  ");
            var workflowName = Console.ReadLine();
            Console.Write("\n");
            var orderPaylaod = new OrderPayload() { WorkflowName = workflowName };
            // Run workflow
            await executor.ProcessFunctionalityAsync(orderPaylaod);
            Console.Write("\nExecution Completed for " + workflowName);
        }
        Console.ReadLine();
    }
}
