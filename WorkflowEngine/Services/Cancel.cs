using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Cancel : IStep
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Executing Cancel");
            return Task.CompletedTask;
        }
    }
}
