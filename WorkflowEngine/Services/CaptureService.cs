using WorkflowEngine.Contracts;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services
{
    public class CaptureService : ICapture, IStep
    {
        public Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload)
        {
            Console.WriteLine("Executing Capture Service");
            return Task.FromResult((orderPayload, true));
        }
    }
}
