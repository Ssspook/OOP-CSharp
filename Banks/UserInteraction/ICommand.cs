using Banks.BanksManagement;

namespace Banks.UserInteraction
{
    public interface ICommand
    {
        string Text { get; }
        void Execute(BanksManager banksManager);
    }
}