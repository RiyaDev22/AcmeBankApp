namespace AcmeBank.BankAccounts.AccountInterfaces;

internal interface IDepositLimitedAccount
{
    decimal DepositLimit { get; }
    decimal RemainingDepositLimit { get; set; } // Property to track and modify remaining deposit limit

    bool UpdateDepositLimit(decimal amount);
    void ResetDepositLimit(); // Method to reset deposit limit

}
