using System;
using System.Collections.Generic;
using Banks.AccountManagement;
using Banks.ClientManagement;

namespace Banks.AccountCreation
{
    public class DepositAccountCreator : IAccountCreator
    {
        private double _balance;
        private DateTime _endOfPeriodDate;
        private double _maxWithdrawalSum;
        private Client _client;
        private IReadOnlyDictionary<Range, double> _sumsPercentages;
        private DateTime _currentDate;

        public DepositAccountCreator(Client client, DateTime endOfPeriodDate, DateTime currentDate, double maxWithdrawalSum, double balance, IReadOnlyDictionary<Range, double> sumsPercentages)
        {
            _client = client;
            _endOfPeriodDate = endOfPeriodDate;
            _maxWithdrawalSum = maxWithdrawalSum;
            _balance = balance;
            _sumsPercentages = sumsPercentages;
            _currentDate = currentDate;
        }

        public IAccount CreateAccount()
            => new DepositAccount(_client, _endOfPeriodDate, _currentDate, _maxWithdrawalSum, _balance, _sumsPercentages);
    }
}