using WorkflowEngine.Models;

namespace WorkflowEngine.Contracts
{
    public interface ICapture
    {
        Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload);
    }
}
