using Banks.AccountManagement;
using Banks.ClientManagement;

namespace Banks.AccountCreation
{
    public class DebitAccountCreator : IAccountCreator
    {
        private Client _client;
        private double _balance;
        private double _percentage;
        private double _maxWithdrawalSum;
        public DebitAccountCreator(Client client, double balance, double percentage, double maxWithdrawalSum)
        {
            _balance = balance;
            _client = client;
            _percentage = percentage;
            _maxWithdrawalSum = maxWithdrawalSum;
        }

        public IAccount CreateAccount()
            => new DebitAccount(_client, _balance, _percentage, _maxWithdrawalSum);
    }
}