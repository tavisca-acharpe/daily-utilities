using WorkflowEngine.Contracts;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services
{
    public class Payment : IStep
    {
        public Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload)
        {
            Console.WriteLine("Executing Payment");
            return Task.FromResult((orderPayload, true));
        }
    }
}
