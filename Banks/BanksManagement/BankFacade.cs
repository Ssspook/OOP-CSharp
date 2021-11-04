using System;
using Banks.AccountCreation;
using Banks.ClientManagement;

namespace Banks.BanksManagement
{
    public class BankFacade
    {
        private Bank _bank;

        public BankFacade(Bank bank)
        {
            _bank = bank;
        }

        public void CreateAndRegisterDebitAccount(double sum, Client client)
        {
            var debitAccountCreator = new DebitAccountCreator(client, sum, _bank.BankInfo.YearlyPercent, _bank.BankInfo.MaxOperationSumForUntrustedClients);
            _bank.RegisterAccount(debitAccountCreator.CreateAccount(), client);
        }

        public void CreateAndRegisterCreditAccount(double limit, Client client)
        {
            var creditAccountCreator = new CreditAccountCreator(client, limit, _bank.BankInfo.CommissionPercent, _bank.BankInfo.MaxOperationSumForUntrustedClients);
            _bank.RegisterAccount(creditAccountCreator.CreateAccount(), client);
        }

        public void CreateAndRegisterDepositAccount(int months, Client client, double sum)
        {
            var depositAccountCreator = new DepositAccountCreator(client, DateTime.Now.AddMonths(months), DateTime.Now, _bank.BankInfo.MaxOperationSumForUntrustedClients, sum, _bank.BankInfo.SumsPercentages);
            _bank.RegisterAccount(depositAccountCreator.CreateAccount(), client);
        }
    }
}