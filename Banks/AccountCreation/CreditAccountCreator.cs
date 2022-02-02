using Banks.AccountManagement;
using Banks.ClientManagement;

namespace Banks.AccountCreation
{
    public class CreditAccountCreator : IAccountCreator
    {
        private Client _client;
        private double _limit;
        private double _commission;
        private double _maxWithdrawalSum;

        public CreditAccountCreator(Client client, double limit, double commission, double maxWithdrawalSum)
        {
            _limit = limit;
            _client = client;
            _commission = commission;
            _maxWithdrawalSum = maxWithdrawalSum;
        }

        public IAccount CreateAccount()
            => new CreditAccount(_client, _limit, _commission, _maxWithdrawalSum);
    }
}