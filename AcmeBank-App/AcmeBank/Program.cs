
using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;
using System.Collections.Generic;
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


            //Customer newCustomer = CustomerUtilities.CreateCustomer();

            //loads customer and then presents options
            //Account account = AccountUtilities.LoadAccountDetails("23455432");
            //account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options

            List<Account> accounts = new List<Account>();

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

            //Initialise a string list which will contains customer's details
            List<string> slCustomersCsv = new List<string>();
            List<Customer> clCustomers = new List<Customer>();
            /****************************************************************************************************************************************
            * TODO!!!: Populate slCustomersCsv with content from the csv file                                                                       *
            /****************************************************************************************************************************************/
            //Populate string list with data
            slCustomersCsv.Add("Kawsar,Hussain,EMPTY,17/04/2001,E15 5DP,17/04/2018,What is the manufacturer of the first car you owned or drove?,Toyota,11112222,33334444");
            slCustomersCsv.Add("Tom,Scott,EMPTY,15/06/2001,E13 3FJ,17/06/2018,What is the manufacturer of the first car you owned or drove?,Audi,12344321");


            //public Customer(string firstName, string lastName, string otherName, DateOnly dateOfBirth, string postCode, string securityQuestion, string securityAnswer)

            //Populate customer list with data from the string list
            foreach (string sCustomerDetails in slCustomersCsv)
            {
                string[] saCustomerDetails = sCustomerDetails.Split(',');
                DateOnly doDate = DateOnly.Parse(saCustomerDetails[3]);

                clCustomers.Add(new Customer (saCustomerDetails[0].ToString(),
                                              saCustomerDetails[1].ToString(),
                                              saCustomerDetails[2].ToString(),
                                              doDate,
                                              saCustomerDetails[4].ToString(),
                                              saCustomerDetails[6].ToString(),
                                              saCustomerDetails[7].ToString()));
            }

            //Declare object which will display the customer validation screen
            CustomerValidation oCustomerValidation;
            Customer oCustomer;

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
                string? sUserInput = Console.ReadLine();

                switch (sUserInput)
                {
                    case "1":
                        //Create a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();
                        //Retrieve customer's details using the object
                        oCustomer = oCustomerValidation.ValidateCustomer(clCustomers);
                        /****************************************************************************************************************************************
                        * TODO!!!: Display the correct customer's details                                                                                       *
                        /****************************************************************************************************************************************/

                        //loads customer and then presents options
                        Account account = AccountUtilities.LoadAccountDetails("11112222");
                        account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options
                        break;
                    case "2":
                        //Clear the console
                        Console.Clear();
                        //Invoke function in the Customer Utilities class to create a new customer account
                        Customer oNewCustomer = CustomerUtilities.CreateCustomer();
                        /****************************************************************************************************************************************
                        * TODO!!!: Append the new customer details to the list/csv file                                                                         *
                        /****************************************************************************************************************************************/
                        break;
                    case "3":
                        //Retrieve customer's details by creating a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();
                        //Retrieve customer's details using the object
                        oCustomer = oCustomerValidation.ValidateCustomer(clCustomers);
                        //Invoke function in the Customer Utilities class to remove the customer account
                        CustomerUtilities.RemoveCustomerDetails(oCustomer);
                        /****************************************************************************************************************************************
                        * TODO!!!: Remove the customer details from the list/csv file                                                                           *
                        /****************************************************************************************************************************************/
                        break;
                    case "*":
                        //Log teller out
                        oTeller.logout();
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Clear the console
                        Console.Clear();
                        //Prompt the teller to log back in
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
        }

        //DateOnly dob = new DateOnly(2001, 4, 17);
        //Customer kawsar = CustomerUtilities.LoadCustomerDetails("Kawsar", "Hussain", "", dob, "E15 5DP");

        //CustomerUtilities.RemoveCustomerDetails(kawsar);
    }
}