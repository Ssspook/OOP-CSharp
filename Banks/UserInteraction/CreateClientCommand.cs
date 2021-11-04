using System;
using Banks.BanksManagement;
using Banks.ClientManagement;
using static System.String;

namespace Banks.UserInteraction
{
    public class CreateClientCommand : ICommand
    {
        public CreateClientCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }

        public void Execute(BanksManager banksManager)
        {
            var clientBuilder = new ClientBuilder();

            Console.Write("Name? ");
            string name = Console.ReadLine();
            Client client = clientBuilder.SetFirstname(name).Build();

            Console.Write("Surname? ");
            string surName = Console.ReadLine();
            client = clientBuilder.SetSurname(surName).Build();

            Console.Write("Address? (optional, Enter to skip) ");
            string address = Console.ReadLine();
            if (!IsNullOrEmpty(address))
                client = clientBuilder.SetAddress(address).Build();

            Console.Write("Passport Data? (optional, Enter to skip) ");
            string passportData = Console.ReadLine();
            if (!IsNullOrEmpty(passportData))
                client = clientBuilder.SetPassportData(passportData).Build();
            banksManager.AddClient(client);
            Console.WriteLine();
        }
    }
}