using WorkflowEngine.Models;

namespace WorkflowEngine.Contracts
{
    public interface IBook
    {
        Task<(OrderPayload, bool)> ExecuteAsync(OrderPayload orderPayload);
    }
}
