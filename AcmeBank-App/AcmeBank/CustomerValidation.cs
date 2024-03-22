namespace AcmeBank
{
    internal class CustomerValidation
    {
        #region Global Variables

        private string? _sFirstName;
        private string? _sLastName;
        private string? _sDob;
        private string? _sPostcode;

        #endregion

        #region Methods
        public Customer? ValidateCustomer(List<Customer> clCustomers)
        {
            //Invoke function to retrieve details from user input and store them into the global variables
            retrieveAndStoreDetails();
          
            //Loop through the customer list
            foreach (Customer oCustomer in clCustomers)
            {
                //Check if customer exists by validating details
                if (validateDetails(oCustomer))
                {                    
                    //Display chosen security question
                    Console.Write($"\nPlease answer this security question\n{oCustomer.SecurityQuestion}: ");
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
                    //If the answer is incorrect, return null
                    else return null;
                }
            }
            //Return null if customer does not exist
            return null;
        }

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
        }

        private string askForInputAndCheck(string sMenu)
        {
            string sMenuTitle = "---Customer Validation---";
            //Boolean which determines if the input is valid
            bool bInputValid = false;
            //String which stores the user input
            string? sInput;

            //Print the menu title
            Console.WriteLine(sMenuTitle);

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
                    Console.WriteLine($"{sMenuTitle}\nInvalid Input. The input must contain at least 2 characters. Please try again.\n");
                }
            } while (!bInputValid); //Keep executing until the input meets the basic requirements
            //Clear console
            Console.Clear();
            //Trim leading and trailing whitespaces
            //Return a string that meets the basic requirements
            return sInput;
        }

        private bool validateDetails(Customer oCustomer)
        {
            if (_sFirstName.ToLower().CompareTo(oCustomer.FirstName.ToLower()) == 1 &&
                _sLastName.ToLower().CompareTo(oCustomer.LastName.ToLower()) == 1)
            {
                Console.WriteLine("First Name & Last Name does not exist");
                return false;
            }

            if (!DateOnly.TryParse(_sDob, out DateOnly doDob))
            {
                Console.WriteLine("The date must be in the correct format");
                return false;
            }

            if (_sPostcode.ToLower().Replace(" ", "").CompareTo(oCustomer.PostCode.ToLower().Replace(" ", "")) == 1)
            {
                return false;
            }
            
            return true;
        }
        #endregion
    }
}
