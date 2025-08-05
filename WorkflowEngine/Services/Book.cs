using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Book : IStep
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Executing Book");
            return Task.CompletedTask;
        }
    }
}
