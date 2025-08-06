using Microsoft.Extensions.Configuration;
using WorkflowEngine.Contracts;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace WorkflowEngine.Services;

public class WorkflowExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public WorkflowExecutor(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task ExecuteWorkflowAsync(string workflowName)
    {
        //Steps : Get Workflow Configuration for consul/programservice
        var steps = _configuration.GetSection($"Workflows:{workflowName}").Get<List<string>>();

        //excecute workflow 
        foreach (var stepName in steps)
        {
            var step = GetRequiredService(stepName);
            var result = await step.ExecuteAsync();

            if (result == false)
            {
                break;
            }
        }
    }

    private IStep GetRequiredService(string stepName)
    {
        if (string.Equals(stepName, "FraudCheck", StringComparison.OrdinalIgnoreCase))
        {
            return new FraudCheck();
        }
        if (string.Equals(stepName, "Book", StringComparison.OrdinalIgnoreCase))
        {
            return new Book();
        }
        if (string.Equals(stepName, "Payment", StringComparison.OrdinalIgnoreCase))
        {
            return new Payment();
        }
        if (string.Equals(stepName, "Cancel", StringComparison.OrdinalIgnoreCase))
        {
            return new Cancel();
        }
        if (string.Equals(stepName, "PostBooking", StringComparison.OrdinalIgnoreCase))
        {
            return new PostBooking();
        }
        return null;
    }
}