using AcmeBank.Accounts;

namespace AcmeBank;

internal class Program
{
    static void Main(string[] args)
    {
        //Create a new Teller object which displays the login screen once the application starts
        Teller oTeller = new Teller();        

        // Instantiate three Account objects
        PersonalAccount account1 = new PersonalAccount("Personal account", "12-34-56", "12345678"); //no value for construction as £1 is needed
        ISAAccount account2 = new ISAAccount("ISA", "98-76-54", "87654321", 500.25m);
        BusinessAccount account3 = new BusinessAccount("Business Account", "45-67-89", "98765432", 750.75m,2000m);

        Console.Clear();
        // Display details of each account
        account1.DisplayAccountDetails();
        account2.DisplayAccountDetails();
        account3.DisplayAccountDetails();
    }
}
