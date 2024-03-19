namespace AcmeBank;

internal class CustomerUtilities
{
    //This method allows for the creation of a customer object when it is called in the menu
    public static Customer CreateCustomer()
    {
        Console.WriteLine("""
                Hello and welcome to a new and Exciting Journey with us.
                I will start by asking for your name.

                """);
        string firstName = InputUtilities.StringInputHandling("What is your first name", true);
        string lastName = InputUtilities.StringInputHandling("what is your last name", true);
        string otherName = InputUtilities.StringInputHandling("what is your middle name/s", true, true);

        DateOnly dob = CreateDOB();

        string securityQUestion = SelectSecurityQuestion();
        string securityAnswer = InputUtilities.StringInputHandling("What would be the answer to your security question?");

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
}
