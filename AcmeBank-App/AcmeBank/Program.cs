
using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;
using System.Text.RegularExpressions;

namespace AcmeBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create a new Teller object which displays the login screen once the application starts
            //Teller oTeller = new Teller();

            //Customer newCustomer = CustomerUtilities.CreateCustomer();

            //loads customer and then presents options
            Account account = AccountUtilities.LoadAccountDetails("23455432");
            account.AccountOptionsLoop(); // this is a place holder for now and just holds the basic shared options
        }        

    }
}
