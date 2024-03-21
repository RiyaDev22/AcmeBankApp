
using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AcmeBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create a new Teller object which displays the login screen once the application starts
            Teller oTeller = new Teller();            

            List < Account > accounts = new List<Account>();

            // Adding ISAAccount, BusinessAccount, and PersonalAccount objects to the list
            accounts.Add(new ISAAccount("12345678", "111111", 2000.00m, "1-Main St-ABC123"));
            accounts.Add(new BusinessAccount("23456789", "222222", 2500.00m, "2-High St-DEF456"));
            accounts.Add(new PersonalAccount("34567890", "333333", 3000.00m, "3-Elm St-GHI789"));
            accounts.Add(new ISAAccount("45678901", "444444", 1500.00m, "4-Oak St-JKL012"));
            accounts.Add(new BusinessAccount("56789012", "555555", 1800.00m, "5-Pine St-MNO345"));
            accounts.Add(new PersonalAccount("67890123", "666666", 2200.00m, "6-Maple St-PQR678"));
            accounts.Add(new ISAAccount("78901234", "777777", 1700.00m, "7-Cedar St-STU901"));
            accounts.Add(new BusinessAccount("89012345", "888888", 1900.00m, "8-Birch St-VWX234"));
            accounts.Add(new PersonalAccount("90123456", "999999", 2100.00m, "9-Willow St-YZAB567"));
            accounts.Add(new ISAAccount("11112222", "101010", 2700.00m, "10-Market St-CDE890"));
            accounts.Add(new BusinessAccount("22223333", "202020", 3000.00m, "11-Meadow St-FGH123"));
            accounts.Add(new PersonalAccount("33334444", "303030", 3200.00m, "12-Grove St-IJK456"));
            accounts.Add(new ISAAccount("44445555", "404040", 1900.00m, "13-Orchard St-LMN789"));
            accounts.Add(new BusinessAccount("55556666", "505050", 2200.00m, "14-Park St-OPQ012"));
            accounts.Add(new PersonalAccount("66667777", "606060", 2600.00m, "15-Beach St-RST345"));
            accounts.Add(new ISAAccount("77778888", "707070", 1800.00m, "16-River St-UVW678"));
            accounts.Add(new BusinessAccount("88889999", "808080", 2100.00m, "17-Sunset St-XYZ901"));
            accounts.Add(new PersonalAccount("99990000", "909090", 2300.00m, "18-Sunrise St-ABC234"));
            accounts.Add(new ISAAccount("12344321", "111122", 2400.00m, "19-Hill St-DEF567"));
            accounts.Add(new BusinessAccount("23455432", "222233", 2600.00m, "20-Valley St-GHI890"));

            // Iterate through the list
            foreach (var i in accounts)
            {
                AccountUtilities.SaveAccountDetails(i);
            }

            //Declare object which will display the customer validation screen
            CustomerValidation oCustomerValidation;

            //This while loop will run indefinitely until the user press 'x' to quit - Look at the switch case statement
            while (true)
            {
                //Display main menu
                Console.Write("""
                            --- Main Menu ---
                            1. View a Customer Account
                            2. Create a Customer Account
                            3. Remove a Customer Account
                            *. Log Out
                            x. Log Out & Quit

                            Enter an option: 
                            """);

                //Prompt user input
                string? sUserInput = InputUtilities.GetInputWithinTimeLimit();

                switch (sUserInput)
                {
                    case "1":
                        //Retrieve customer's details by creating a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();

                        //loads customer and then presents options
                        Account account = AccountUtilities.LoadAccountDetails("23456789");
                        account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options
                        break;
                    case "2":
                        //Clear the console
                        Console.Clear();
                        //Invoke function to create a new customer account
                        CreateCustomer();
                        break;
                    case "3":
                        //Retrieve customer's details by creating a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();
                        //Invoke function in the Customer class to remove the customer account
                        break;
                    case "*":
                        //Logs teller out
                        oTeller.logout();
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Clear the console
                        Console.Clear();
                        //Promptes the teller to log back in
                        oTeller.login();
                        break;
                    case "x":
                        //Logs teller out
                        oTeller.logout();
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Print message
                        Console.Write("\nExiting...");
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Quit the application
                        Environment.Exit(0);
                        break;
                    default:
                        //Clear the console
                        Console.Clear();
                        //Print message
                        Console.WriteLine("Invalid Input. Please try again.\n");
                        break;
                }
            }

            /*MOVED THIS PART OF THE CODE TO THE SWITCH CASE STATEMENT ABOVE*/
            /*//loads customer and then presents options
            Account account = AccountUtilities.LoadAccountDetails("11112222");
            account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options*/
        }

        //This method allows for the creation of a customer object when it is called in the menu
        public static Customer CreateCustomer()
        {
            Console.WriteLine("""
                Hello and welcome to a new and Exciting Journey with us.
                I will start by asking for your name.

                """);
            string firstName = StringInputHandling("What is your first name", true);
            string lastName = StringInputHandling("what is your last name", true);
            string otherName = StringInputHandling("what is your middle name/s", true, true);

            DateOnly dob = CreateDOB();

            string securityQUestion = SelectSecurityQuestion();
            string securityAnswer = StringInputHandling("What would be the answer to your security question?");

            Customer customer = new Customer(firstName, lastName, otherName, dob, securityQUestion, securityAnswer);
            return customer;
        }

        
        //This method allows for the teller to select a security question that the user wants to use
        private static string SelectSecurityQuestion()
        {
            string prompt = """
                What would you like to select as your security question?
                Select from the options below
                1) What is your mother's maiden name?
                2) What school did you attend when you were 10 years old?
                3) What is your pets name?
                4) What was your dream as a child?
                5) What is your Grandfather's name?
                6) What is the manufacturer of the first car you owned or drove?
                """;

            int question = IntegerInputHandling(prompt, "To select a question, input a number", 1, 6, 1);

            switch (question)
            {
                case 1:
                    return "What is your mother's maiden name?";
                case 2:
                    return "What school did you attend when you were 10 years old?";
                case 3:
                    return "What is your pets name?";
                case 4:
                    return "What was your dream as a child?";
                case 5:
                    return "What is your Grandfather's name?";
                case 6:
                    return "What is the manufacturer of the first car you owned or drove?";
                default:
                    break;
            }
            return "";
        }

        
        //Method to facilitate the creation of DOB
        private static DateOnly CreateDOB()
        {
            int day = -1 , month = -1, year = -1;
            bool loop = true;
            DateOnly dob = new DateOnly();
            while (loop)
            {
                try
                {
                    day = IntegerInputHandling("What day were you born?", "Input in the format DD", 1, 31, 2);
                    month = IntegerInputHandling("What month were you born?", "Input in the format MM", 1, 12, 2);
                    year = IntegerInputHandling("What year were you born?", "Input in the format YYYY", 1900, DateTime.Now.Year, 4);
                    dob = new DateOnly(year, month, day);
                    loop = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Input a valid date");
                    day = -1;
                    month = -1;
                    year = -1;
                }
            }
            return dob;
        }

        /*
        Method to facilitate inputs of integers
        Parameter minValue sets the minimum value the integer input can be
        Parameter maxValue sets the maximum value the integer input can be
        Parameter digits sets the amount of digits the integer input ha to be i.e. setting digits to two accepts inputs between 01 - 99
        */
        private static int IntegerInputHandling(string prompt, string helpPrompt, int minValue, int maxValue, uint digits)
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


        /*
        Method to facilitate inputting of string inputs
        Parameter specialCharacterCheck checks for the following values in a string "/*-+_@&$#%" and values between 0-9 when set to true
        Parameter isNullable allows the input to be empty when set to true
        */
        private static string StringInputHandling(string prompt, bool specialCharacterCheck = false, bool isNullable = false)
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
    }

}
