using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Payment : IStep
    {
        public Task<bool> ExecuteAsync()
        {
            Console.WriteLine("Executing Payment");
            return Task.FromResult(true);
        }
    }
}
