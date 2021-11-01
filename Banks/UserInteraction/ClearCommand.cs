using System;
using Banks.BanksManagement;

namespace Banks.UserInteraction
{
    public class ClearCommand : ICommand
    {
        public ClearCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }
        public void Execute(BanksManager banksManager)
        {
            Console.Clear();
        }
    }
}