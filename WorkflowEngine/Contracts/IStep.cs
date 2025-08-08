using WorkflowEngine.Models;

namespace WorkflowEngine.Contracts
{
    public interface IStep
    {
        Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload);
    }
}
