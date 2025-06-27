namespace Test.Programs
{
    public static class StringProgram
    {
        public static void StringPrograms()
        {
            Console.WriteLine("Select Option Number To Select Program");
            Console.WriteLine("1. PalindromeString");
            Console.WriteLine("2. ReverseString");


            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    PalindromeString();
                    break;
                case 2:
                    ReverseWordString();
                    break;

                case 3:

                    break;

                default:
                    Console.WriteLine("Wrong Program Selected Exit");
                    break;
            }
        }

        private static void ReverseWordString()
        {
            string exit = "no";
            do
            {
                Console.WriteLine("Enter String");
                string string1 = Console.ReadLine();
                var stringCharArray = string1.ToCharArray();
                char[] reverseStringArray = new char[stringCharArray.Length];

                var j = 0;
                for (int i = string1.Length - 1 ; i >= 0; i--)
                {
                    reverseStringArray[j++] = stringCharArray[i];
                }

                var reverseString = new string(reverseStringArray);
                Console.WriteLine("Reverse String {0}", reverseString);

                Console.WriteLine("Do you want to continue.............yes or no ?");
                exit = Console.ReadLine();
            } while (string.Equals(exit, "yes", StringComparison.OrdinalIgnoreCase));
        }

        private static void PalindromeString()
        {
            string exit = "no";
            do
            {
                Console.WriteLine("Enter String");
                string string1 = Console.ReadLine();

                var str = string1.ToCharArray();
                var len = string1.Length;
                bool pal = true;

                for (int i = 0; i < string1.Length / 2; i++)
                {
                    if (str[i] != str[len-1])
                    {
                        Console.WriteLine("Not Palindrome String");
                        pal = false;
                        break;
                    }
                    len --;
                }

                if (pal)
                {
                    Console.WriteLine("Palindrome String");
                }

                Console.WriteLine("Do you want to continue.............yes or no ?`");
                exit = Console.ReadLine();
            } while (string.Equals(exit, "yes", StringComparison.OrdinalIgnoreCase));
        }
    }
}
