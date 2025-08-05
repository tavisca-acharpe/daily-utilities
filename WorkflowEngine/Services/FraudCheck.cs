using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class FraudCheck : IStep
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Executing FraudCheck");
            return Task.CompletedTask;
        }
    }
}
