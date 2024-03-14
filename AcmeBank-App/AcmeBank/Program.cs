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
            //string firstName = StringInputHandling("What is your first name", true);
            //string lastName = StringInputHandling("what is your last name", true);
            //string otherName = StringInputHandling("what is your middle name / names", true, true);

            int day = DateInputHandling("What day were you born?", "Input in the format DD", 1, 31, 2);
            int month = DateInputHandling("What month were you born?", "Input in the format MM", 1, 12, 2);

        }

        //Method to facilitate inputs of integers

        private static int DateInputHandling(string prompt, string helpPrompt, int minValue, int maxValue, uint digits)
        {
            string input;
            int inputNumber = -1;
            bool helpConfirm = false;
            bool correctInputType;

            if (digits == 0) return -1; // If the digits of input is 0 then it returns a -1, indicating that digits values wasn't put in
            
            do
            {
                //If the user got the input wrong
                if (helpConfirm) 
                    Console.WriteLine($"""
                        {helpPrompt} AND
                        Make sure the value is between {minValue}-{maxValue}
                        """);
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                //Checks to see if it can convert input to an integer
                try
                {
                    correctInputType = true;
                    inputNumber = Int32.Parse(input);
                }
                catch (FormatException) //If the input cannot be parsed into string
                {
                    Console.Clear();
                    correctInputType = false;
                    Console.WriteLine("please input an integer value");
                    helpConfirm = true;
                    input = "";
                }

                if (correctInputType)
                {
                    if (input.Length == digits && (inputNumber >= minValue && inputNumber <= maxValue))
                        Console.Clear(); //Breaking Condition
                    else
                    {
                        input = "";
                        helpConfirm = true;
                        Console.Clear();
                    }
                }
            } while (input == "");

            return inputNumber;
        }



        //Method to facilitate inputting of string inputs
        private static string StringInputHandling(string prompt, bool specialCharacterCheck = false, bool isNullable = false)
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
