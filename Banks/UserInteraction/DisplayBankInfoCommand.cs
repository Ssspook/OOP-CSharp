using System;
using System.Linq;
using Banks.BanksManagement;
using Banks.Tools;

namespace Banks.UserInteraction
{
    public class DisplayBankInfoCommand : ICommand
    {
        public DisplayBankInfoCommand(string name)
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
            Console.WriteLine($"Id: {bank.BankInfo.UniqueId}");
            Console.WriteLine($"Credit limit: {bank.BankInfo.LimitForCredit}");
            Console.WriteLine($"Yearly percent: {bank.BankInfo.YearlyPercent}");
            Console.WriteLine($"Max sum for withdraw and transfer for untrusted clients: {bank.BankInfo.MaxOperationSumForUntrustedClients}");
            foreach (var pair in bank.BankInfo.SumsPercentages)
            {
                Console.WriteLine($"Range: {pair.Key}");
                Console.WriteLine($"Percentage: {pair.Value}");
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}