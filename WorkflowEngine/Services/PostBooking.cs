using WorkflowEngine.Contracts;

namespace WorkflowEngine.Services
{
    public class PostBooking : IStep
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Executing PostBooking");
            return Task.CompletedTask;
        }
    }
}
