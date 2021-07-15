using System.Collections.Generic;
using System.Linq;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Domain;

namespace TransactionsApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static TransactionResponseObject ToResponse(this Transaction domain)
        {
            if (domain == null) return null;
            return new TransactionResponseObject() {
                FinancialMonth = domain.FinancialMonth,
                Id = domain.Id,
                PaymentReference = domain.PaymentReference,
                PeriodNo = domain.PeriodNo,
                TargetId = domain.TargetId,
                TransactionAmount = domain.TransactionAmount,
                TransactionDate = domain.TransactionDate,
                TransactionType = domain.TransactionType,
                FinancialYear = domain.FinancialYear,
                PaidAmount = domain.PaidAmount,
                ChargedAmount = domain.ChargedAmount,
                BalanceAmount = domain.BalanceAmount,
                HousingBenefitAmount = domain.HousingBenefitAmount,
                AssetFullAddress = domain.AssetFullAddress
            };
        }

        public static List<TransactionResponseObject> ToResponse(this IEnumerable<Transaction> domainList)
        {
            if (null == domainList) return new List<TransactionResponseObject>();
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
