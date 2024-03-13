namespace AcmeBank.Accounts;

public class BuisnessAccount : Account
{
    #region Attributes
    private decimal _overdraftLimit;
    #endregion

    #region Contructors
    public BuisnessAccount(string name, string sortCode, string accountNumber, decimal balance, decimal overdraftLimit) : base(name, sortCode, accountNumber, balance)
    {
        _overdraftLimit = overdraftLimit;
    }
    #endregion

    #region Getters/Setters
    public decimal OverdraftLimit
    {
        get { return _overdraftLimit; }
        set { _overdraftLimit = value; } //can realistically change but not sure for this context
    }
    #endregion

    #region Methods
    public void RequestChequeBook()
    {
        //displays the name and address of the customer the cheque has been sent to
        Console.WriteLine("""
            A cheque book has been sent to:
              name:
              address:

            press enter to continue.
            """);
        //allows for user to read the prompt
        Console.ReadLine();
    }

    public void LoanEligibilityCheck()
    {
        /* Actual example Lloyds
         * Customers have risk indicator 1 - 9, (1 being the best - 9 being the worst)
         * This displays how much they would be able to borrow #(over scope)#
         */

        //displays a prompt with contact details for the loan department
        Console.WriteLine("""
            Please refer the customer to:
              ---- Loan Department ---
              email: 
              phone:

            press enter to continue.
            """);
        //allows for user to read the prompt
        Console.ReadLine();
    }
    #endregion
}
