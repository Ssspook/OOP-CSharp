using System;
using Banks.ClientManagement;

namespace Banks.AccountManagement
{
    public interface IAccount
    {
        Guid Id { get; }
        Client Client { get; }

        string Type { get; }

        double Balance { get; }
        bool IsWithdrawable(double sum);

        void IncreaseBalance(double sum);

        void DecreaseBalance(double sum);

        void DoUniqueAction(DateTime lastCheckout, DateTime currentDate);
    }
}