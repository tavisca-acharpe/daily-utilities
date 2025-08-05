namespace WorkflowEngine.Contracts
{
    public interface IStep
    {
        Task<bool> ExecuteAsync();
    }
}
