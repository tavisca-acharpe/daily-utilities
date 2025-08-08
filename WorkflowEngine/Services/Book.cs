using WorkflowEngine.Contracts;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services
{
    public class Book : IStep
    {
        public Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload)
        {
            Console.WriteLine("Executing Book");
            return Task.FromResult((orderPayload, true));
        }
    }
}
