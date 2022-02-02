using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountManagement;
using Banks.ClientManagement;
using Banks.Observing;
using Banks.Tools;

namespace Banks.BanksManagement
{
    public class BanksManager : IObservable
    {
        private List<Bank> _banks;
        private List<IObserver> _observers;
        private DateTime _newDate = DateTime.Now;
        private List<Client> _clients;

        public BanksManager()
        {
            _observers = new List<IObserver>();
            _banks = new List<Bank>();
            _clients = new List<Client>();
        }

        public DateTime NewDate => _newDate;

        public IReadOnlyList<Bank> GetBanks => _banks;
        public IReadOnlyList<Client> GetClients => _clients;

        public void RegisterBank(Bank bank)
        {
            _banks.Add(bank);
        }

        public void AddClientToBank(Client client, Bank bank)
        {
            if (!_banks.Contains(bank))
                throw new BanksException("Bank doesn't exist");

            if (bank.ClientAccountsDictionary.Keys.Contains(client))
                throw new BanksException("This client is already added");

            bank.AddClient(client);
        }

        public void AddClient(Client client)
            => _clients.Add(client);
        public void ForwardTimeToNewDate(DateTime newDate)
        {
            _newDate = newDate;
            Notify();
        }

        public Bank GetBankByAccount(IAccount account)
            => _banks.FirstOrDefault(bank => bank.Accounts.Contains(account));

        public void AddObserver(IObserver observer)
            => _observers.Add(observer);

        public void Notify()
        {
            _observers.ForEach(observer =>
            {
                observer.Modify(this);
            });
        }
    }
}