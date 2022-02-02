using System;
using System.Collections.Generic;
using Banks.BanksManagement;
using Banks.ClientManagement;
using Banks.CommandManagement;
using Banks.Tools;
using Banks.UserInteraction;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var banksManager = new BanksManager();
            var commandManager = new CommandManager();
            commandManager.AddCommand(new AddClientToBankCommand("/AddClientToBank"));
            commandManager.AddCommand(new CreateClientCommand("/CreateClient"));
            commandManager.AddCommand(new CreateCreditAccountCommand("/CreateCreditAccount"));
            commandManager.AddCommand(new CreateDebitAccountCommand("/CreateDebitAccount"));
            commandManager.AddCommand(new CreateDepositAccountCommand("/CreateDepositAccount"));
            commandManager.AddCommand(new DisplayBankInfoCommand("/DisplayBankInfo"));
            commandManager.AddCommand(new DisplayClientCommand("/DisplayClient"));
            commandManager.AddCommand(new ForwardTimeCommand("/ForwardTime"));

            commandManager.AddCommand(new ClearCommand("/clear"));

            var sberBankPercentages = new Dictionary<Range, double>()
            {
                { new Range(1, 50000), 3 },
                { new Range(50000, 100000), 5 },
                { new Range(100000, 200000), 7 },
                { new Range(200000, int.MaxValue), 10 },
            };
            var tinkoffPercentages = new Dictionary<Range, double>()
            {
                { new Range(1, 50000), 6 },
                { new Range(50000, 100000), 8 },
                { new Range(100000, 200000), 10 },
                { new Range(200000, int.MaxValue), 12 },
            };
            var sberBank = new Bank("Sberbank", 3.65, 3, 10000, sberBankPercentages, 100000);
            var tinkoffBank = new Bank("Tinkoff", 8, 6, 100000, tinkoffPercentages, 200000);
            banksManager.AddObserver(sberBank);
            banksManager.AddObserver(tinkoffBank);

            banksManager.RegisterBank(sberBank);
            banksManager.RegisterBank(tinkoffBank);
            commandManager.DisplayCommands();

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "/quit")
                    break;

                if (command == "/help")
                {
                    commandManager.DisplayCommands();
                    continue;
                }

                try
                {
                    commandManager.HandleCommand(command, banksManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
