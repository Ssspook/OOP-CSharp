using System;
using System.Collections.Generic;
using System.Linq;
using Banks.ClientManagement;
using Banks.Extensions;
using Banks.Tools;

namespace Banks.AccountManagement
{
    public class DepositAccount : IAccount
    {
        private DateTime _currentDate;
        private DateTime _endOfPeriodDate;
        private Client _client;

        private double _balance;
        private double _percentage;
        private double _profit = 0;
        private double _maxWithdrawalSum;
        private double _withdrawalCounter = 0;
        public DepositAccount(Client client, DateTime endOfPeriodDate, DateTime currentDate, double maxWithdrawalSum, double balance, IReadOnlyDictionary<Range, double> sumsPercentages)
        {
            if (balance <= 0)
                throw new BanksException("Balance has to be a positive number");
            var tariff = sumsPercentages.FirstOrDefault(percentage
                => percentage.Key.IsInRange(balance));
            Id = Guid.NewGuid();
            _percentage = tariff.Value;
            _endOfPeriodDate = endOfPeriodDate;
            _client = client;
            _maxWithdrawalSum = maxWithdrawalSum;
            _balance = balance;
            _currentDate = currentDate;
        }

        public Client Client => _client;
        public Guid Id { get; }

        public double Balance => _balance;
        public void DecreaseBalance(double sum)
        {
            _balance -= sum;
            _withdrawalCounter += sum;
        }

        public void IncreaseBalance(double sum)
            => _balance += sum;

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

            return _currentDate >= _endOfPeriodDate && _balance - sum <= 0;
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
            _currentDate = currentDate;
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