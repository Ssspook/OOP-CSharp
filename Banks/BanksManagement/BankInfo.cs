using System;
using System.Collections.Generic;

namespace Banks.BanksManagement
{
    public class BankInfo
    {
        public BankInfo(
            double yearlyPercent,
            Dictionary<Range, double> sumsPercentages,
            double commissionPercent,
            double maxOperationSumForUntrustedClients,
            string name,
            int limitForCredit)
        {
            UniqueId = Guid.NewGuid();
            YearlyPercent = yearlyPercent;
            CommissionPercent = commissionPercent;
            MaxOperationSumForUntrustedClients = maxOperationSumForUntrustedClients;
            Name = name;
            SumsPercentages = sumsPercentages;
            LimitForCredit = limitForCredit;
        }

        public Guid UniqueId { get; set; }
        public double YearlyPercent { get; set; }
        public double CommissionPercent { get; set; }
        public double MaxOperationSumForUntrustedClients { get; set; }
        public string Name { get; set; }

        public int LimitForCredit { get; set; }
        public IReadOnlyDictionary<Range, double> SumsPercentages { get; set; }
    }
}