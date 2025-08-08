using WorkflowEngine.Contracts;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services
{
    public class Cancel : IStep
    {
        public Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload)
        {
            Console.WriteLine("Executing Cancel");
            return Task.FromResult((orderPayload, true));
        }
    }
}
