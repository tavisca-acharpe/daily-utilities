using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Cancel : IStep
    {
        public Task<bool> ExecuteAsync()
        {
            Console.WriteLine("Executing Cancel");
            return Task.FromResult(true);
        }
    }
}
