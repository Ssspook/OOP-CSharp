using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountCreation;
using Banks.AccountManagement;
using Banks.BanksManagement;
using Banks.ClientManagement;
using Banks.OperationManagement;
using Banks.Tools;
using Banks.UserInteraction;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class BanksTests
    {
        private BanksManager _banksManager;
        private Bank _sberBank;
        private Bank _tinkoffBank;
        [SetUp]
        public void Setup()
        {
            _banksManager = new BanksManager();
            var sberBankPercentages = new Dictionary<Range, double>()
            {
                { new Range(1, 50000), 3 },
                { new Range(50000, 100000), 5 },
                { new Range(100000, 200000), 7 },
                { new Range(200000, int.MaxValue), 10 },
            };
            var tinkoffPercentages = new Dictionary<Range, double>()
            {
                { new Range(1, 50000), 6 },
                { new Range(50000, 100000), 8 },
                { new Range(100000, 200000), 10 },
                { new Range(200000, int.MaxValue), 12 },
            };

            _sberBank = new Bank("Sberbank", 3.65, 3, 10000, sberBankPercentages, 100000);
            _tinkoffBank = new Bank("Tinkoff", 8, 6, 100000, tinkoffPercentages, 200000);
            _banksManager.RegisterBank(_sberBank);
            _banksManager.RegisterBank(_tinkoffBank);
        }
        [Test]
        public void AddClientToBank_ClientAdded()
        {
            var clientBuilder = new ClientBuilder();
             var client = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").SetAddress("My address").SetPassportData("Passport data").Build();

            _banksManager.AddClientToBank(client, _sberBank);

            Assert.True(_sberBank.ClientAccountsDictionary.Keys.Contains(client));
        }

        [Test]
        public void UntrustedClientWithdrawsMoreThanAllowed_ThrowException()
        {
            var clientBuilder = new ClientBuilder();
            var client = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").Build();

            _banksManager.AddClientToBank(client, _sberBank);
            var accountCreator = new DebitAccountCreator(client, 50000,_sberBank.BankInfo.YearlyPercent, _sberBank.BankInfo.MaxOperationSumForUntrustedClients);
            var account = accountCreator.CreateAccount();
            _sberBank.RegisterAccount(account, client);

            var withdrawOperation = new WithdrawOperation(account, _sberBank.BankInfo.MaxOperationSumForUntrustedClients + 1);
            Assert.Catch<BanksException>(() =>
            {
                withdrawOperation.Execute();
            });
        }

        [Test]
        public void TimeForwarded_BalancesUpdated()
        {
            var clientBuilder = new ClientBuilder();
            _banksManager.AddObserver(_sberBank);
            _banksManager.AddObserver(_tinkoffBank);
            Client sberClient = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").SetAddress("My address").SetPassportData("Passport data").Build();
            Client tinkClient = clientBuilder.SetFirstname("John").SetSurname("Doe").Build();

            IAccount sbeAccount = new DebitAccountCreator(sberClient, 50000, _sberBank.BankInfo.YearlyPercent, _sberBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            IAccount tinkAccount = new DebitAccountCreator(tinkClient, 60000, _tinkoffBank.BankInfo.YearlyPercent, _tinkoffBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            int initialSberAccBalance = 50000;
            int initialTinkAccBalance = 60000;

            _banksManager.AddClientToBank(sberClient, _sberBank);
            _banksManager.AddClientToBank(tinkClient, _tinkoffBank);

            _sberBank.RegisterAccount(sbeAccount, sberClient);
            _tinkoffBank.RegisterAccount(tinkAccount, tinkClient);

            _banksManager.ForwardTimeToNewDate(DateTime.Now.AddMonths(2));
            Assert.True(sbeAccount.Balance > initialSberAccBalance && tinkAccount.Balance > initialTinkAccBalance);
        }

        [Test]
        public void TransferMoney_MoneyTransferred()
        {
            var clientBuilder = new ClientBuilder();
            Client sberClient = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").SetAddress("My address").SetPassportData("Passport data").Build();
            Client tinkClient = clientBuilder.SetFirstname("John").SetSurname("Doe").Build();

            IAccount sbeAccount = new DebitAccountCreator(sberClient, 50000, _sberBank.BankInfo.YearlyPercent, _sberBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            IAccount tinkAccount = new DebitAccountCreator(tinkClient, 60000, _tinkoffBank.BankInfo.YearlyPercent, _tinkoffBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            double initialSberAccBalance = 50000;
            double initialTinkAccBalance = 60000;

            _banksManager.AddClientToBank(sberClient, _sberBank);
            _banksManager.AddClientToBank(tinkClient, _tinkoffBank);

            _sberBank.RegisterAccount(sbeAccount, sberClient);
            _tinkoffBank.RegisterAccount(tinkAccount, tinkClient);

            var transferOperation = new TransferOperation(sbeAccount, tinkAccount, 60);
            transferOperation.Execute();
            Assert.True(sbeAccount.Balance == initialSberAccBalance - 60 && tinkAccount.Balance == initialTinkAccBalance + 60);
        }

        [Test]
        public void UndoOperation_OperationUndone()
        {
            var clientBuilder = new ClientBuilder();
            Client sberClient = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").SetAddress("My address").SetPassportData("Passport data").Build();
            Client tinkClient = clientBuilder.SetFirstname("John").SetSurname("Doe").Build();

            IAccount sbeAccount = new DebitAccountCreator(sberClient, 50000, _sberBank.BankInfo.YearlyPercent, _sberBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            IAccount tinkAccount = new DebitAccountCreator(tinkClient, 60000, _tinkoffBank.BankInfo.YearlyPercent, _tinkoffBank.BankInfo.MaxOperationSumForUntrustedClients).CreateAccount();
            double initialSberAccBalance = 50000;
            double initialTinkAccBalance = 60000;

            _banksManager.AddClientToBank(sberClient, _sberBank);
            _banksManager.AddClientToBank(tinkClient, _tinkoffBank);

            _sberBank.RegisterAccount(sbeAccount, sberClient);
            _tinkoffBank.RegisterAccount(tinkAccount, tinkClient);

            var transferOperation = new TransferOperation(sbeAccount, tinkAccount, 60);
            transferOperation.Execute();

            transferOperation.Undo();
            Assert.True(sbeAccount.Balance == initialSberAccBalance && tinkAccount.Balance == initialTinkAccBalance);
        }

        [Test]
        public void TryToWithdrawFromDepositBeforeTimeElapses_ThrowException()
        {
            var clientBuilder = new ClientBuilder();
            Client sberClient = clientBuilder.SetFirstname("Mike").SetSurname("Andrew").SetAddress("My address").SetPassportData("Passport data").Build();
            IAccount sbeAccount = new DepositAccountCreator(
                                  sberClient,
                    DateTime.Now.AddMonths(5),
                        DateTime.Now,
                    _sberBank.BankInfo.MaxOperationSumForUntrustedClients,
                                  70000,
                                  _sberBank.BankInfo.SumsPercentages
                                  ).CreateAccount();
            var withdrawOperation = new WithdrawOperation(sbeAccount, 590);
            Assert.Catch<BanksException>(() =>
            {
                withdrawOperation.Execute();
            });

        }
    }
}