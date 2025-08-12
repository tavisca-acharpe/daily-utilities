using WorkflowEngine.Models;

namespace WorkflowEngine.Contracts
{
    public interface IAuth
    {
        Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload);
    }
}
