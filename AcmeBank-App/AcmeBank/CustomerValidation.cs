using AcmeBank.BankAccounts;

namespace AcmeBank
{
    internal class CustomerValidation
    {
        #region Global Variables        
        #endregion

        #region Getters and Setters
        #endregion

        #region Methods
        public Customer? ValidateCustomer(List<Customer> clCustomers)
        {
            //Clear the console
            Console.Clear();
            //Display the Customer Validation menu
            Console.Write("""
                            --- Customer Validation ---
                            Please enter the customer's details in the following fields.

                            First Name: 
                            """);

            //Prompt teller to enter customer's first name
            string? sFirstName = Console.ReadLine();
            //Display message
            Console.Write("Last Name: ");
            //Prompt teller to enter customer's last name
            string? sLastName = Console.ReadLine();
            //Display message
            Console.Write("Postcode: ");
            //Prompt teller to enter customer's postcode
            string? sPostcode = Console.ReadLine();
            //Display message
            Console.Write("Date Of Birth (YYYY-MM-DD): ");
            //Prompt teller to enter customer's date of birth
            string? sDob = Console.ReadLine();

            //Check if user input is valid
            if(DateOnly.TryParse(sDob, out DateOnly doDob))
            {             
                //Loop through the customer list
                foreach (Customer oCustomer in clCustomers)
                {
                    //Check if customer exists
                    if (oCustomer.FirstName.CompareTo(sFirstName) == 0 &&
                       oCustomer.LastName.CompareTo(sLastName) == 0 &&
                       oCustomer.DateOfBirth.CompareTo(doDob) == 0 &&
                       oCustomer.PostCode.CompareTo(sPostcode) == 0)
                    {
                        //Display chosen security question
                        Console.Write($"\nPlease answer this security question\n{oCustomer.SecurityQuestion}: ");
                        //Prompt teller to enter customer's security answer
                        string? sAnswerInput = Console.ReadLine();
                        //If the answer is correct
                        if (oCustomer.SecurityAnswer.CompareTo(sAnswerInput) == 0)
                        {
                            //Clear the console
                            Console.Clear();
                            //Display message
                            Console.Write("Valid Customer\nLoading...");
                            //Pause the application for 2 seconds
                            Thread.Sleep(2000);
                            //Return customer
                            return oCustomer;
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
