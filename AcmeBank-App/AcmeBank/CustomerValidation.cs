namespace AcmeBank
{
    internal class CustomerValidation
    {
        #region Global Variables

        /*These global variables are used to store the user input values for customer validation*/
        private string? _sFirstName;
        private string? _sLastName;
        private string? _sDob;
        private string? _sPostcode;
        /*These global variables are used to store the current menu and the title*/
        private string? _sMenu;
        private string? _sMenuTitle = "---Customer Validation---";

        #endregion

        #region Methods
        /*This function retrieves and validates customer's details, the security question and answer*/
        public Customer? validateCustomer(List<Customer> clCustomers)
        {
            //Invoke function to retrieve details from user input and store them into the global variables
            retrieveAndStoreDetails();
          
            //Loop through the customer list
            foreach (Customer oCustomer in clCustomers)
            {
                //Check if customer exists by validating details
                if (_sFirstName.ToLower().CompareTo(oCustomer.FirstName.ToLower()) == 0 &&
                    _sLastName.ToLower().CompareTo(oCustomer.LastName.ToLower()) == 0 &&
                    (DateOnly.TryParse(_sDob, out DateOnly doDob) && doDob.CompareTo(oCustomer.DateOfBirth) == 0) &&
                    _sPostcode.ToLower().Replace(" ", "").CompareTo(oCustomer.PostCode.ToLower().Replace(" ", "")) == 0)
                {
                    //Clear console
                    Console.Clear();
                    //Display chosen security question
                    Console.Write($"{_sMenu}\n\nPlease answer this security question\n{oCustomer.SecurityQuestion}: ");

                    //Prompt teller to enter customer's security answer
                    string? sAnswerInput = InputUtilities.GetInputWithinTimeLimit().Trim();
                    //If the answer is correct
                    if (oCustomer.SecurityAnswer.ToLower().CompareTo(sAnswerInput.ToLower()) == 0)
                    {
                        //Clear the console
                        Console.Clear();
                        //Display message
                        Console.Write("Customer Found\nLoading...");
                        //Pause the application for 2 seconds
                        Thread.Sleep(2000);
                        //Clear the console
                        Console.Clear();
                        //Return customer
                        return oCustomer;
                    }
                    //If the answer is incorrect
                    else
                    {
                        //Return null
                        return null;
                    }
                }
            }
            //Return null if customer does not exist
            return null;
        }

        /*This function displays the menu and stores data that meets the basic requirements*/
        private void retrieveAndStoreDetails()
        {
            //Clear the console
            Console.Clear();
            //Store the Customer Validation menu into a string
            string sMenu = """                            
                            Please enter the customer's details in the following fields.

                            First Name: 
                            """;
            //Prompt teller to enter customer's first name
            _sFirstName = askForInputAndCheck(sMenu);

            //Append the string to the menu
            sMenu += $"{_sFirstName}\nLast Name: ";
            //Prompt teller to enter customer's last name
            _sLastName = askForInputAndCheck(sMenu);

            //Append the string to the menu
            sMenu += $"{_sLastName}\nDate Of Birth (DD/MM/YYYY): ";
            //Prompt teller to enter customer's date of birth
            _sDob = askForInputAndCheck(sMenu);

            //Append the string to the menu
            sMenu += $"{_sDob}\nPostcode: ";
            //Prompt teller to enter customer's postcode
            _sPostcode = askForInputAndCheck(sMenu);

            //Append the string to the menu
            sMenu += _sPostcode;
            //Print the menu
            Console.WriteLine(sMenu);

            //Store the menu title and the current menu into the global variable
            _sMenu = _sMenuTitle + "\n" + sMenu;
        }

        /*This function prompts the teller to input data and checks if the input meets the basic requirements. This is specifically for the Customer Validation class*/
        private string askForInputAndCheck(string sMenu)
        {
            //Boolean which determines if the input is valid
            bool bInputValid = false;
            //String which stores the user input
            string? sInput;

            //Print the menu title
            Console.WriteLine(_sMenuTitle);

            do
            {
                //Display the Customer Validation menu
                Console.Write(sMenu);
                //Prompt teller to enter information
                sInput = InputUtilities.GetInputWithinTimeLimit().Trim();

                //Check if input meets the basic requirements (length > 1 && does NOT contain only whitespaces)
                if (sInput.Length > 1 && sInput.Trim().Length > 0)
                {
                    //Set boolean to true
                    bInputValid = true;
                }
                else
                {
                    //Clear console
                    Console.Clear();
                    //Display message
                    Console.WriteLine($"{_sMenuTitle}\nInvalid Input. The input must contain at least 2 characters. Please try again.\n");
                }
            } while (!bInputValid); //Keep executing until the input meets the basic requirements
            //Clear console
            Console.Clear();
            //Return a string that meets the basic requirements
            return sInput;
        }
        #endregion
    }
}
