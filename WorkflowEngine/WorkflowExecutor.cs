using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkflowEngine.Contracts;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IAuth _authService;
    private readonly IBook _bookService;
    private readonly ICapture _captureService;

    public WorkflowExecutor(IServiceProvider serviceProvider, IConfiguration configuration, IAuth auth, IBook book, ICapture capture)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _authService = auth;
        _bookService = book;
        _captureService = capture;
    }

    public async Task ProcessFunctionalityAsync(OrderPayload orderPayload)
    {
        var enabledWorkflow = true;

        if (enabledWorkflow)
        {
            //Steps : Get Workflow Configuration for consul/programservice
            var steps = _configuration.GetSection($"Workflows:{orderPayload.WorkflowName}").Get<List<string>>();

            //excecute workflow 
            foreach (var stepName in steps)
            {
                var step = GetRequiredService(stepName);
                var (payload, result) = await step.ExecuteAsync(new Models.OrderPayload());

                if (result == false)
                {
                    break;
                }
                orderPayload = payload;
            }
        }
        else
        {
            var (payload, authResult) = await _authService.ExecuteAsync(orderPayload);
            if (authResult == true)
            {
                var (bookPayload, bookResult) = await _bookService.ExecuteAsync(orderPayload);
                var (capturePayload, captureResult) = await _captureService.ExecuteAsync(orderPayload);
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
            return new BookService();
        }
        if (string.Equals(stepName, "Payment", StringComparison.OrdinalIgnoreCase))
        {
            return new AuthService();
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