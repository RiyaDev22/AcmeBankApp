using AcmeBank.BankAccounts.Transactions;

namespace AcmeBank.BankAccounts.RegularPayments
{
    internal struct RegularPayment
    {
        private string _payeeAccountNumber;
        private decimal _amount;
        private DateTime _date;

        public RegularPayment(string payeeAccountNumber, decimal amount, DateTime date)
        {
            _payeeAccountNumber = payeeAccountNumber;
            _amount = amount;
            _date = date;
        }

        public string PayeeAccountNumber { get { return _payeeAccountNumber; } }
        public decimal Amount { get { return _amount; } }
        public DateTime Date { get { return _date; } }
    }
}
