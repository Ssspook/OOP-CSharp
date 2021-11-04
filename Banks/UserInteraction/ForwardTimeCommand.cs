using System;
using Banks.BanksManagement;

namespace Banks.UserInteraction
{
    public class ForwardTimeCommand : ICommand
    {
        public ForwardTimeCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }
        public void Execute(BanksManager banksManager)
        {
            Console.Write("How much to forward in months? ");
            int months = int.Parse(Console.ReadLine() ?? string.Empty);

            banksManager.ForwardTimeToNewDate(banksManager.NewDate.AddMonths(months));
            Console.WriteLine();
        }
    }
}