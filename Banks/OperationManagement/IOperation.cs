namespace Banks.OperationManagenment
{
    public interface IOperation
    {
        public void Execute();
        public void Undo();
    }
}