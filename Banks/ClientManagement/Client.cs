using System;
using System.Collections.Generic;
using Banks.AccountManagement;
using Banks.Observing;
using Banks.Tools;

namespace Banks.ClientManagement
{
    public class Client : IObserver
    {
        private List<IAccount> _accounts = new List<IAccount>();
        private bool _isNotified = false;
        public Client(string firstname, string surname, string address, string passport)
        {
            Id = Guid.NewGuid();
            Firstname = firstname;
            Surname = surname;
            Address = address;
            PassportData = passport;
        }

        public string Firstname { get; }
        public string Surname { get; }
        public string Address { get; private set; }
        public string PassportData { get; private set; }
        public Guid Id { get; }

        public bool IsNotified => _isNotified;
        public List<IAccount> GetAccounts => new List<IAccount>(_accounts);

        public void AddAccount(IAccount account)
        {
            if (account is null)
                throw new BanksException("Account cannot be null");

            _accounts.Add(account);
        }

        public void Modify(IObservable subject)
        {
            _isNotified = true;
        }

        public bool IsTrustable()
            => !(Address is null) && !(PassportData is null);

        public void AddOrSwitchAddress(string address)
        {
            if (address is null)
                throw new BanksException("Address cannot be null");

            Address = address;
        }

        public void AddOrSwitchPassportData(string passportData)
        {
            if (passportData is null)
                throw new BanksException("Passport Data cannot be null");

            PassportData = passportData;
        }
    }
}