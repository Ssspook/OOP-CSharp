using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountManagement;
using Banks.ClientManagement;
using Banks.Observing;
using Banks.OperationManagement;
using Banks.OperationManagenment;
using Banks.Tools;

namespace Banks.BanksManagement
{
    public class Bank : IObserver, IObservable
    {
        private readonly Dictionary<Client, List<IAccount>> _clientAccountsDictionary = new Dictionary<Client, List<IAccount>>();
        private readonly List<IAccount> _accounts;
        private readonly List<IOperation> _operations = new List<IOperation>();
        private readonly List<IOperation> _undoneOperations;
        private readonly List<IObserver> _observers;
        private DateTime _lastUpdate = DateTime.Now;

        public Bank(string name, double yearlyPercent, double commissionPercent, double maxOperationSumForUntrustedClients, Dictionary<Range, double> sumsPercents, int limitForCredit)
        {
            _observers = new List<IObserver>();
            _accounts = new List<IAccount>();
            _undoneOperations = new List<IOperation>();
            BankInfo = new BankInfo(yearlyPercent, sumsPercents, commissionPercent, maxOperationSumForUntrustedClients, name, limitForCredit);
        }

        public BankInfo BankInfo { get; }

        public DateTime NewDate => _lastUpdate;

        public IReadOnlyDictionary<Client, List<IAccount>> ClientAccountsDictionary => _clientAccountsDictionary;

        public IReadOnlyList<IAccount> Accounts => _accounts;
        public void AddClient(Client client)
        {
            _clientAccountsDictionary[client] = new List<IAccount>();
        }

        public void RegisterAccount(IAccount account, Client client)
        {
            if (_clientAccountsDictionary.Keys.Contains(client))
            {
                _clientAccountsDictionary[client].Add(account);
            }
            else
            {
                AddClient(client);
                _clientAccountsDictionary[client].Add(account);
            }

            _accounts.Add(account);
        }

        public void Modify(IObservable subject)
        {
            foreach (var keyValuePair in _clientAccountsDictionary)
            {
                keyValuePair.Value.ForEach(account =>
                {
                    account.DoUniqueAction(_lastUpdate, subject.NewDate);
                });
            }

            _lastUpdate = subject.NewDate;
        }

        public void ChangePercentagesForDeposit(Dictionary<Range, double> newSumsPercentages)
        {
            BankInfo.SumsPercentages = newSumsPercentages;
            Notify();
        }

        public void ChangeCreditLimit(int newCreditLimit)
        {
            BankInfo.LimitForCredit = newCreditLimit;
            Notify();
        }

        public void AddObserver(IObserver observer)
        {
           _observers.Add(observer);
        }

        public void Notify()
        {
            _observers.ForEach(observer =>
            {
                observer.Modify(this);
            });
        }

        public void AddMoney(IAccount account, double sum)
        {
            if (account is null)
                throw new BanksException("Account cannot be null");

            IOperation addOperation = new AddOperation(account, sum);
            addOperation.Execute();
            RegisterOperation(addOperation);
        }

        public void WithdrawMoney(IAccount account, double sum)
        {
            if (account is null)
                throw new BanksException("Account cannot be null");
            IOperation withdrawal = new WithdrawOperation(account, sum);
            withdrawal.Execute();
            RegisterOperation(withdrawal);
        }

        public void TransferMoney(BanksManager banksManager, IAccount sender, IAccount receiver, double sum)
        {
            if (sender is null)
                throw new BanksException("Sender cannot be null");
            if (receiver is null)
                throw new BanksException("Receiver cannot be null");
            if (!_accounts.Contains(sender))
                throw new BanksException("Sender's account is not an account of this bank");

            IOperation transfer = new TransferOperation(sender, receiver, sum);
            transfer.Execute();
            var receiversBank = banksManager.GetBankByAccount(receiver);
            if (receiversBank == this)
            {
                RegisterOperation(transfer);
                return;
            }

            RegisterOperation(transfer);
            receiversBank.RegisterOperation(transfer);
        }

        public void UndoOperation(IOperation operation)
        {
            if (operation is null)
                throw new BanksException("Operation cannot be null");
            if (!_operations.Contains(operation))
                throw new BanksException("There was no such an operation");
            if (_undoneOperations.Contains(operation))
                throw new BanksException("Undone operation cannot be undone again!");

            operation.Undo();
            _undoneOperations.Add(operation);
        }

        private void RegisterOperation(IOperation operation)
        {
            _operations.Add(operation);
        }
    }
}