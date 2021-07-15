using System;
using TransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.Tests.V1.Helper
{
    public static class Constants
    {
        public static Guid ID { get; } = Guid.NewGuid();
        public static Guid TARGETID { get; } = Guid.NewGuid();
        public const decimal AMOUNT = 123.45M;
        public const short PERIODNO = 2;
        public const short YEAR = 2021;
        public const short MONTH = 06;
        public const string TYPE = "TYPE A";
        public static string REFERENCE { get; } = Guid.NewGuid().ToString();
        public static string ADDRESS { get; } = "Apartment 22, 18 G road, SW11";
        public static DateTime DATE { get; } = DateTime.UtcNow.AddDays(-40);


        public static Transaction ConstructTransactionFromConstants()
        {
            var entity = new Transaction
            {
                Id = ID,
                TargetId = TARGETID,
                PaymentReference = REFERENCE,
                PeriodNo = PERIODNO,
                TransactionAmount = AMOUNT,
                FinancialMonth = MONTH,
                FinancialYear = YEAR,
                TransactionType = TYPE,
                TransactionDate = DATE,
                PaidAmount = AMOUNT,
                BalanceAmount = AMOUNT,
                ChargedAmount = AMOUNT,
                HousingBenefitAmount= AMOUNT,
                AssetFullAddress = ADDRESS
            };

            return entity;
        }
    }
}
