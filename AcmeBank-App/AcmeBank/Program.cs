using AcmeBank.Accounts;

namespace AcmeBank;

internal class Program
{
    static void Main(string[] args)
    {
        // Instantiate three Account objects
        PersonalAccount account1 = new PersonalAccount("Personal account", "12-34-56", "12345678");
        ISAAccount account2 = new ISAAccount("ISA", "98-76-54", "87654321", 500.25m);
        BuisnessAccount account3 = new BuisnessAccount("Buisness Account", "45-67-89", "98765432", 750.75m,2000m);

        // Display details of each account
        account1.DisplayAccountDetails();
        account2.DisplayAccountDetails();
        account3.DisplayAccountDetails();
    }
}
