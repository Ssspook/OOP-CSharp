using Banks.AccountManagement;
using Banks.BanksManagement;
using Banks.OperationManagenment;
using Banks.Tools;

namespace Banks.OperationManagement
{
    public class AddOperation : IOperation
    {
        private IAccount _account;
        private double _sum;

        public AddOperation(IAccount account, double sum)
        {
            _account = account;
            _sum = sum;
        }

        public void Execute()
        {
            _account.IncreaseBalance(_sum);
        }

        public void Undo()
        {
            _account.DecreaseBalance(_sum);
        }
    }
}