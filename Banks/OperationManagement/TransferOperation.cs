using Banks.AccountManagement;
using Banks.BanksManagement;
using Banks.OperationManagenment;
using Banks.Tools;

namespace Banks.OperationManagement
{
    public class TransferOperation : IOperation
    {
        private IAccount _sender;
        private IAccount _receiver;

        private double _sum;

        public TransferOperation(IAccount sender, IAccount receiver, double sum)
        {
            _sender = sender;
            _receiver = receiver;
            _sum = sum;
        }

        public void Execute()
        {
            if (!_sender.IsWithdrawable(_sum))
                throw new BanksException("Sender cannot transfer money!");

            _sender.DecreaseBalance(_sum);
            _receiver.IncreaseBalance(_sum);
        }

        public void Undo()
        {
            _sender.IncreaseBalance(_sum);
            _receiver.DecreaseBalance(_sum);
        }
    }
}