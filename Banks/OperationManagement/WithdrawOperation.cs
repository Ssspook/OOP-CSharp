using System;
using Banks.AccountManagement;
using Banks.BanksManagement;
using Banks.OperationManagenment;
using Banks.Tools;

namespace Banks.OperationManagement
{
    public class WithdrawOperation : IOperation
    {
        private IAccount _account;
        private double _sum;

        public WithdrawOperation(IAccount account, double sum)
        {
            _account = account;
            _sum = sum;
        }

        public void Execute()
        {
            if (!_account.IsWithdrawable(_sum))
                throw new BanksException("Unable to execute");

            _account.DecreaseBalance(_sum);
        }

        public void Undo()
        {
           _account.IncreaseBalance(_sum);
        }
    }
}