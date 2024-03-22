namespace AcmeBank
{
    internal class CustomerValidation
    {
        #region Global Variables

        private string? _sFirstName;
        private string? _sLastName;
        private string? _sPostcode;
        private string? _sDob;

        #endregion

        #region Methods
        public Customer? ValidateCustomer(List<Customer> clCustomers)
        {
            //Invoke function to retrieve details from user input and store them into the global variables
            retrieveAndStoreDetails();
            
            /*//Check if first name, last name, postcode, and date of birth is valid
            if(checkIfDetailsValid(_sFirstName, _sLastName, _sPostcode) && DateOnly.TryParse(_sDob, out DateOnly doDob))
            {
            }*/
          
            //Loop through the customer list
            foreach (Customer oCustomer in clCustomers)
            {
                //Check if customer exists
                if (checkNameValid(oCustomer) && checkPostcodeValid(oCustomer) && checkDobValid(oCustomer))
                {                    
                    //Display chosen security question
                    Console.Write($"\nPlease answer this security question\n{oCustomer.SecurityQuestion}: ");
                    //Prompt teller to enter customer's security answer
                    string? sAnswerInput = InputUtilities.GetInputWithinTimeLimit();
                    //If the answer is correct
                    if (oCustomer.SecurityAnswer.CompareTo(sAnswerInput) == 0)
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
            sMenu += $"{_sLastName}\nPostcode: ";
            //Prompt teller to enter customer's postcode
            _sPostcode = askForInputAndCheck(sMenu);

            //Append the string to the menu
            sMenu += $"{_sPostcode}\nDate Of Birth (DD/MM/YYYY): ";
            //Prompt teller to enter customer's date of birth
            _sDob = askForInputAndCheck(sMenu);

            //Append the date of birth to the string
            sMenu += _sDob;
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
                sInput = InputUtilities.GetInputWithinTimeLimit();;

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
            //Return a string that meets the basic requirements
            return sInput;
        }

        private bool checkNameValid(Customer oCustomer)
        {            
            return true;
        }

        private bool checkPostcodeValid(Customer oCustomer)
        {
            return true;
        }

        private bool checkDobValid(Customer oCustomer)
        {
            return true;
        }
        #endregion
    }
}
