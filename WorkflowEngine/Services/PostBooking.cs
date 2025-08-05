using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class PostBooking : IStep
    {
        public Task<bool> ExecuteAsync()
        {
            Console.WriteLine("Executing PostBooking");
            return Task.FromResult(true);
        }
    }
}
