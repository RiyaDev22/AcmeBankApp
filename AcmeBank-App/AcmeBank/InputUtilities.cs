using System.Text.RegularExpressions;

namespace AcmeBank;

internal class InputUtilities
{
    
    /*
    Method to facilitate inputting of string inputs
    Parameter specialCharacterCheck checks for the following values in a string "/*-+_@&$#%" and values between 0-9 when set to true
    Parameter isNullable allows the input to be empty when set to true
    */
    internal static string StringInputHandling(string prompt, bool specialCharacterCheck = false, bool isNullable = false)
    {
        string input;
        bool inputValidation = true;
        do
        {
            Console.WriteLine(prompt);

            //Case that the input is nullable
            if (isNullable)
                Console.WriteLine("you can press the RETURN key if this information is not available.");

            input = InputUtilities.GetInputWithinTimeLimit();

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
                Console.WriteLine("Please do not input an empty string. \n");
            }
        } while ((input == "" && !isNullable) ^ !inputValidation);

        Console.Clear();
        return input;

    }

    /*
    Method to facilitate inputs of integers
    Parameter minValue sets the minimum value the integer input can be
    Parameter maxValue sets the maximum value the integer input can be
    Parameter digits sets the amount of digits the integer input ha to be i.e. setting digits to two accepts inputs between 01 - 99
    */
    internal static int IntegerInputHandling(string prompt, string helpPrompt, int minValue, int maxValue, uint digits)
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
                        {helpPrompt}
                        Make sure the value is between {minValue}-{maxValue}

                        """);
            Console.WriteLine(prompt);
            input = InputUtilities.GetInputWithinTimeLimit();

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

            //Checks to see if input Follows the correct format
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

    //we need a method that replaces the usual readlines with one that ensures that input happens within an appropriate time limit
    public static string? GetInputWithinTimeLimit()
    {
        //initialise input
        string? input = null;
        //we create a task to read the input
        var task = Task.Run(() => input = Console.ReadLine());
        //if this task is completed in less than 2 minutes, we just return the input
        if (task.Wait(TimeSpan.FromMinutes(2)))
        {
            return input;
        }
        else
        {
            //otherwise, we will show an error message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("""
                    Input timed out.
                    You have been logged out for security.
                    Please press Return and start over.
                    """);
            Console.ResetColor();
            //since security is now a concern, we will exit the program
            Environment.Exit(1);
            //we still need a return statement for the method, even though it will never be reached
            return null;
        }
    }
}

