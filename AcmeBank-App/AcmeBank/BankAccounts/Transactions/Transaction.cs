namespace AcmeBank.BankAccounts.Transactions;

public struct Transaction
{
    private decimal _amount;
    private decimal _balance;
    private TransactionType _type;
    private DateTime _date;

    public Transaction(decimal amount, decimal balance, TransactionType type, DateTime date)
    {
        _amount = amount;
        _balance = balance;
        _type = type;
        _date = date;
    }

    public decimal Amount { get { return _amount; } }
    public decimal Balance { get { return _balance; } }
    public TransactionType Type { get { return _type; } }
    public DateTime Date { get { return _date; } }

}
