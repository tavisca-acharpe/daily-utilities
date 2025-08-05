using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class Book : IStep
    {
        public Task<bool> ExecuteAsync()
        {
            Console.WriteLine("Executing Book");
            return Task.FromResult(true);
        }
    }
}
