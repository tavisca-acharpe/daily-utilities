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
        var steps = _configuration.GetSection($"Workflows:{workflowName}").Get<List<string>>();

        foreach (var stepName in steps)
        {
            var stepType = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .FirstOrDefault(t => typeof(IStep).IsAssignableFrom(t) && t.Name == stepName);


            var step = (IStep)_serviceProvider.GetRequiredService(stepType);
            await step.ExecuteAsync();
        }
    }
}