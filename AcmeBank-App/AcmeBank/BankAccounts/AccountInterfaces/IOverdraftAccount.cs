namespace AcmeBank.BankAccounts.AccountInterfaces;

internal interface IOverdraftAccount
{
    decimal OverdraftLimit { get; }
    decimal OverdraftRemaining { get; set; }

    bool UpdateRemainingOverdraft(decimal amount); // Checks if it can update the overdraft and then updates it if so
}
