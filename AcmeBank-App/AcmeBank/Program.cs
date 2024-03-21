
using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AcmeBank;

internal class Program
{
    static void Main(string[] args)
    {
        //Create a new Teller object which displays the login screen once the application starts
        Teller oTeller = new Teller();            


        //Customer newCustomer = CustomerUtilities.CreateCustomer();
        //CustomerUtilities.AddCustomerDetials(newCustomer);
        //CustomerUtilities.EditCustomerDetails(new Customer("Jon", "Jones", "", new DateOnly(1970, 1, 1), "E1 3MX", "What is your pets name?", "What pet?", new DateOnly(2024, 3, 21), new List<string>()));

        //loads customer and then presents options
        //Account account = AccountUtilities.LoadAccountDetails("23455432");
        //account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options

        List < Account > accounts = new List<Account>();

        // Adding ISAAccount, BusinessAccount, and PersonalAccount objects to the list
        accounts.Add(new ISAAccount("12345678", "111111", 2000.00m, "ABC 123"));
        accounts.Add(new BusinessAccount("23456789", "222222", 2500.00m, "DEF 456"));
        accounts.Add(new PersonalAccount("34567890", "333333", 3000.00m, "GHI 789"));
        accounts.Add(new ISAAccount("45678901", "444444", 1500.00m, "JKL 012"));
        accounts.Add(new BusinessAccount("56789012", "555555", 1800.00m, "MNO 345"));
        accounts.Add(new PersonalAccount("67890123", "666666", 2200.00m, "PQR 678"));
        accounts.Add(new ISAAccount("78901234", "777777", 1700.00m, "STU 901"));
        accounts.Add(new BusinessAccount("89012345", "888888", 1900.00m, "VWX 234"));
        accounts.Add(new PersonalAccount("90123456", "999999", 2100.00m, "YZAB 567"));
        accounts.Add(new ISAAccount("11112222", "101010", 2700.00m, "CDE 890"));
        accounts.Add(new BusinessAccount("22223333", "202020", 3000.00m, "FGH 123"));
        accounts.Add(new PersonalAccount("33334444", "303030", 3200.00m, "IJK 456"));
        accounts.Add(new ISAAccount("44445555", "404040", 1900.00m, "LMN 789"));
        accounts.Add(new BusinessAccount("55556666", "505050", 2200.00m, "OPQ 012"));
        accounts.Add(new PersonalAccount("66667777", "606060", 2600.00m, "RST 345"));
        accounts.Add(new ISAAccount("77778888", "707070", 1800.00m, "UVW 678"));
        accounts.Add(new BusinessAccount("88889999", "808080", 2100.00m, "XYZ 901"));
        accounts.Add(new PersonalAccount("99990000", "909090", 2300.00m, "ABC 234"));
        accounts.Add(new ISAAccount("12344321", "111122", 2400.00m, "DEF 567"));
        accounts.Add(new BusinessAccount("23455432", "222233", 2600.00m, "GHI 890"));

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

