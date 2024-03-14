using System.Text.RegularExpressions;

namespace AcmeBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateCustomer();
        }

        public static void CreateCustomer()
        {
            Console.WriteLine("""
                Hello and welcome to a new and Exciting Journey with us.
                I will start by asking for your name.
                """);
            string firstName = InputHandling("What is your first name", true);
            string lastName = InputHandling("what is your last name", true);
            string otherName = InputHandling("what is your middle name / names", true, true);

        }

        //Method to facilitate inputting of inputs
        private static string InputHandling(string prompt, bool specialCharacterCheck = false, bool isNullable = false)
        {
            string input;
            bool characterValidation = true;
            bool inputValidation = false;
            do
            {
                Console.WriteLine(prompt);
                
                //Case that the input is nullable
                if (isNullable)
                    Console.WriteLine("you can press the RETURN key if this information is not available.");
                
                input = Console.ReadLine();

                if (specialCharacterCheck)
                {
                    //Checks for special characters or numbers in the input
                    inputValidation = Regex.IsMatch(input, @"^[a-zA-Z]+$") ^ input == "";
                    if (!inputValidation)
                    {
                        Console.Clear();
                        Console.WriteLine("""
                            Please do not input any numbers or special characters in the input
                            These include 0-9 and the characters "/*-+_@&$#%"
                            """);
                    }
                }

                // This part handles the input validationit Tells the Teller what went wrong with the input
                if ((input == "" && !isNullable))
                {
                    Console.Clear();
                    Console.WriteLine("Please do not input an empty string.");
                }
            } while ((input == "" && !isNullable) || !inputValidation);

            Console.Clear();
            return input;

        }
    }

}
