using Test.Programs;
using Test.Tests;

namespace Test;

internal class Test
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Select Option Number To Start");
        Console.WriteLine("1. Testing Changes");
        Console.WriteLine("2. String Programs");

        int input = Convert.ToInt32(Console.ReadLine());

        switch (input)
        {
            case 1:
                TestChanges.TestingChanges();
                break;
            case 2:
                StringProgram.StringPrograms();
                break;

            default:
                Console.WriteLine("Wrong Input Exit");
                break;
        }

        string exitcode = Console.ReadLine();
    }
}