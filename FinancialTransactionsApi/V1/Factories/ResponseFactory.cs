using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;
using System.Linq;
using FinancialTransactionsApi.V1.Helpers;

using System;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static TransactionResponse ToResponse(this Transaction domain)
        {
            return domain == null ? null : new TransactionResponse()
            {
                Id = domain.Id,
                TargetId = domain.TargetId,
                AssetId = domain.AssetId,
                AssetType = domain.AssetType,
                TenancyAgreementRef = domain.TenancyAgreementRef,
                PropertyRef = domain.PropertyRef,
                PostDate = domain.PostDate,
                RealValue = domain.RealValue,
                TargetType = domain.TargetType,
                BalanceAmount = domain.BalanceAmount,
                ChargedAmount = domain.ChargedAmount,
                FinancialMonth = domain.FinancialMonth,
                FinancialYear = domain.FinancialYear,
                HousingBenefitAmount = domain.HousingBenefitAmount,
                PaidAmount = domain.PaidAmount,
                PaymentReference = domain.PaymentReference,
                PeriodNo = domain.PeriodNo,
                TransactionAmount = domain.TransactionAmount,
                TransactionDate = domain.TransactionDate,
                TransactionType = domain.TransactionType.GetDescription(),
                TransactionSource = domain.TransactionSource,
                Address = domain.Address,
                Fund = domain.Fund,
                CreatedAt = domain.CreatedAt,
                CreatedBy = domain.CreatedBy,
            };
        }

        public static List<TransactionResponse> ToResponse(this IEnumerable<Transaction> domainList)
        {
            return domainList == null ?
                new List<TransactionResponse>() :
                domainList.Select(domain => domain.ToResponse()).ToList();
        }

        public static ResponseWrapper<IEnumerable<TransactionResponse>> ToResponseWrapper(this IEnumerable<Transaction> domainList)
        {
            return new ResponseWrapper<IEnumerable<TransactionResponse>>(domainList.Select(domain => domain.ToResponse()));
        }
    }
}
