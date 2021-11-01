using System;
using Banks.ClientManagement;
using Banks.Tools;

namespace Banks.AccountManagement
{
    public class DebitAccount : IAccount
    {
        private double _profit = 0;
        private double _percentage;
        private double _balance;
        private Client _client;
        private double _withdrawalCounter = 0;
        private double _maxWithdrawalSum;
        public DebitAccount(Client client, double balance, double percentage, double maxWithdrawalSum)
        {
            Type = "Deposit";
            Id = Guid.NewGuid();
            _balance = balance;
            _percentage = percentage;
            _client = client;
            _maxWithdrawalSum = maxWithdrawalSum;
        }

        public Client Client => _client;
        public Guid Id { get; }

        public double Balance => _balance;
        public string Type { get; }
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

            return _balance >= sum;
        }

        public void IncreaseBalance(double sum)
        {
            _balance += sum;
        }

        public void DecreaseBalance(double sum)
        {
            _balance -= sum;
        }

        public void DoUniqueAction(DateTime lastCheckout, DateTime currentDate)
        {
            if (currentDate < lastCheckout)
                throw new BanksException("Travelling back in time is invalid!");
            double monthDiff = currentDate.Month - lastCheckout.Month + (12 * (currentDate.Year - lastCheckout.Year));
            double daysDiff = (currentDate - lastCheckout).TotalDays;

            if (daysDiff >= 1)
                CalculateProfitForDays((int)daysDiff);
            if (monthDiff >= 1)
                PayProfitForMonths((int)monthDiff);
        }

        private void CalculateProfitForDays(int days)
        {
            for (int i = 0; i < days; i++)
            {
                _profit += _balance * (_percentage / 365);
            }
        }

        private void PayProfitForMonths(int months)
        {
            _balance += _profit * months;
        }
    }
}