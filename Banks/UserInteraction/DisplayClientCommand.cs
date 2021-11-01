using System;
using System.Linq;
using Banks.BanksManagement;
using Banks.ClientManagement;
using Banks.Tools;

namespace Banks.UserInteraction
{
    public class DisplayClientCommand : ICommand
    {
        public DisplayClientCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }
        public void Execute(BanksManager banksManager)
        {
            Console.Write("Client Name? ");
            string clientName = Console.ReadLine();

            Console.Write("Client Surname? ");
            string clientSurname = Console.ReadLine();
            Client client = banksManager.GetClients.FirstOrDefault(client =>
                client.Firstname == clientName && client.Surname == clientSurname);

            if (client is null)
                throw new BanksException("Client not found");

            Console.WriteLine($"Id: {client.Id}");
            Console.WriteLine($"Address: {client.Address}");
            Console.WriteLine($"Passport Data: {client.PassportData}");
            client.GetAccounts.ForEach(account =>
            {
                Console.WriteLine($"Id: {account.Id}");
                Console.WriteLine($"Type: {account.Type}");
            });
            Console.WriteLine();
        }
    }
}