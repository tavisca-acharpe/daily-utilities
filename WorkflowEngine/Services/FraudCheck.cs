using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class FraudCheck : IStep
    {
        public Task<bool> ExecuteAsync()
        {
            Console.WriteLine("Executing FraudCheck");
            return Task.FromResult(true);
        }
    }
}
