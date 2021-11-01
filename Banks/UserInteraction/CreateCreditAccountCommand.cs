using System;
using System.Linq;
using Banks.AccountCreation;
using Banks.BanksManagement;
using Banks.ClientManagement;
using Banks.Tools;

namespace Banks.UserInteraction
{
    public class CreateCreditAccountCommand : ICommand
    {
        public CreateCreditAccountCommand(string name)
        {
            Text = name;
        }

        public string Text { get; }

        public void Execute(BanksManager banksManager)
        {
            Console.Write("Client firstname? ");
            string firstname = Console.ReadLine();

            Console.Write("Client surname? ");
            string surname = Console.ReadLine();

            Client client = banksManager.GetClients.FirstOrDefault(client =>
                client.Firstname == firstname && client.Surname == surname);
            if (client is null)
                throw new BanksException("Client not found");

            Console.Write("Bank? ");
            string response = Console.ReadLine();
            Bank bank = banksManager.GetBanks.FirstOrDefault(bank => bank.BankInfo.Name == response);
            if (bank is null)
                throw new BanksException("Bank not found");

            Console.WriteLine("What sum? ");
            double balance = double.Parse(Console.ReadLine() ?? string.Empty);
            var creditAccountCreator = new CreditAccountCreator(client, bank.BankInfo.LimitForCredit, bank.BankInfo.CommissionPercent, bank.BankInfo.MaxOperationSumForUntrustedClients);
            var creditAccount = creditAccountCreator.CreateAccount();
            bank.RegisterAccount(creditAccount, client);
            Console.WriteLine();
        }
    }
}