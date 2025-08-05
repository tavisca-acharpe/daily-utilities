using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Payment : IStep
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Executing Payment");
            return Task.CompletedTask;
        }
    }
}
