using System;
using System.Linq;
using Banks.BanksManagement;
using Banks.ClientManagement;
using Banks.Tools;

namespace Banks.UserInteraction
{
    public class AddClientToBankCommand : ICommand
    {
        public AddClientToBankCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }

        public void Execute(BanksManager banksManager)
        {
            Console.Write("Bank? ");
            string bankName = Console.ReadLine();
            Bank bank = banksManager.GetBanks.FirstOrDefault(bank => bank.BankInfo.Name == bankName);

            if (bank is null)
                throw new BanksException("Bank not found");

            Console.WriteLine("Client Name? ");
            string clientName = Console.ReadLine();

            Console.WriteLine("Client Surname? ");
            string clientSurname = Console.ReadLine();
            Client client = banksManager.GetClients.FirstOrDefault(client =>
                client.Firstname == clientName && client.Surname == clientSurname);

            if (client is null)
                throw new BanksException("Client not found");
            bank.AddClient(client);
            Console.WriteLine();
        }
    }
}