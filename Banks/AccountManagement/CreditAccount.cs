using System;
using Banks.ClientManagement;

namespace Banks.AccountManagement
{
    public class CreditAccount : IAccount
    {
        private double _commissionPercent;
        private double _withdrawalCounter = 0;
        private double _balance;
        private Client _client;
        private double _maxWithdrawalSum;

        public CreditAccount(Client client, int limit, double commissionPercent, double maxWithdrawalSum)
        {
            Type = "Deposit";
            Id = Guid.NewGuid();
            _commissionPercent = commissionPercent;
            _balance = limit;
            _client = client;
            _maxWithdrawalSum = maxWithdrawalSum;
        }

        public string Type { get; }

        public double Balance => _balance;

        public Client Client => _client;
        public Guid Id { get; }
        public bool IsWithdrawable(double sum)
        {
            if (!_client.IsTrustable())
            {
                if (_withdrawalCounter + sum > _maxWithdrawalSum)
                    return false;
            }
            else
            {
                _withdrawalCounter = 0;
            }

            if (_balance <= 0)
                CalculateCommissionFor(sum);
            return true;
        }

        public void IncreaseBalance(double sum)
        {
            _balance += sum;
        }

        public void DecreaseBalance(double sum)
        {
            CalculateCommissionFor(sum);
            _balance -= sum;
        }

        public void DoUniqueAction(DateTime lastCheckout, DateTime currentDate)
        {
        }

        private void CalculateCommissionFor(double sum)
        {
            if (_balance < 0)
                _balance -= sum * (_commissionPercent / 100);
        }
    }
}