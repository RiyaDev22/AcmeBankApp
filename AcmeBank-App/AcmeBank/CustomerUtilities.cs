using AcmeBank.BankAccounts;

namespace AcmeBank;

internal class CustomerUtilities
{
    private static string accountFile = $@"{AppDomain.CurrentDomain.BaseDirectory}\Customers.csv";

    //This method allows for the creation of a customer object when it is called in the menu
    internal static Customer CreateCustomer()
    {
        Console.WriteLine("""
                Hello and welcome to a new and Exciting Journey with us.
                I will start by asking for your name.

                """);
        string firstName = InputUtilities.StringInputHandling("What is your first name", true);
        string lastName = InputUtilities.StringInputHandling("what is your last name", true);
        string otherName = InputUtilities.StringInputHandling("what is your middle name/s", true, true);

        DateOnly dob = CreateDOB();

        string postCode = InputUtilities.StringInputHandling("What is your postcode?");

        string securityQUestion = SelectSecurityQuestion();
        string securityAnswer = InputUtilities.StringInputHandling("What would be the answer to your security question?");

        Customer customer = new Customer(firstName, lastName, otherName, dob, postCode, securityQUestion, securityAnswer);
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

        int question = InputUtilities.IntegerInputHandling(prompt, "To select a question, input a number", 1, 6, 1);

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
        int day = -1, month = -1, year = -1;
        bool loop = true;
        DateOnly dob = new DateOnly();
        while (loop)
        {
            try
            {
                day = InputUtilities.IntegerInputHandling("What day were you born?", "Input in the format DD", 1, 31, 2);
                month = InputUtilities.IntegerInputHandling("What month were you born?", "Input in the format MM", 1, 12, 2);
                year = InputUtilities.IntegerInputHandling("What year were you born?", "Input in the format YYYY", 1900, DateTime.Now.Year, 4);
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

    //Method to load the customer details
    internal static Customer LoadCustomerDetails(string firstName, string lastName, string otherName, DateOnly dob, string postcode)
    {
        Customer customer = null;

        try
        {
            if (File.Exists(accountFile))
            {
                using (StreamReader sr = File.OpenText(accountFile))
                {
                    string customerDetailsString;
                    string[] customerSplit;
                    while ((customerDetailsString = sr.ReadLine()) != null)
                    {
                        //Splits the string and inserts into array
                        customerSplit = customerDetailsString.Split(',');
                        if (customerSplit[2] == "EMPTY") customerSplit[2] = "";
                        
                        //Condition for making 
                        if (string.Equals(customerSplit[0], firstName, StringComparison.OrdinalIgnoreCase) && 
                            string.Equals(customerSplit[1], lastName, StringComparison.OrdinalIgnoreCase)  && 
                            string.Equals(customerSplit[2], otherName, StringComparison.OrdinalIgnoreCase) && 
                            string.Equals(customerSplit[3], dob.ToString("dd/MM/yyyy"), StringComparison.OrdinalIgnoreCase) && 
                            string.Equals(customerSplit[4], postcode, StringComparison.OrdinalIgnoreCase)) 
                        { 
                            //Creating an array for the date so that it can be parsed into the return statement
                            string[] date = customerSplit[5].Split('/');

                            List<string> listOfAccounts = new List<string>();
                            int i = 8; //In Customer.csv, the account numbers are from the 8th index onwards
                            while (i < customerSplit.Length)
                            {
                                listOfAccounts.Add(customerSplit[i]);
                            }
                            
                            return new Customer(customerSplit[0], 
                                customerSplit[1], 
                                customerSplit[2], 
                                dob, 
                                customerSplit[4], 
                                customerSplit[6], 
                                customerSplit[7],
                                new DateOnly(Int32.Parse(date[0]), Int32.Parse(date[1]), Int32.Parse(date[2])), 
                                listOfAccounts);
                        }
                    }
                }

            }
        }
        catch (IndexOutOfRangeException)
        {
            // Handle parsing errors
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong parsing the file, please check the data!");
        }
        catch (FileNotFoundException)
        {
            // Handle file not found error
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The file couldn't be found!");
        }
        catch (Exception)
        {
            // Handle other exceptions
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong while loading the file!");
        }
        finally
        {
            // Reset console color and provide a delay for user to see the message
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        return customer;
    }
}
