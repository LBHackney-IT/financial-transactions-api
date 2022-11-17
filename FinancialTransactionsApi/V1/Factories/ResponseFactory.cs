using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;
using System.Linq;
using FinancialTransactionsApi.V1.Helpers;
using FTHelpers = FinancialTransactionsApi.V1.Helpers;
using Hackney.Shared.Finance.Pagination;
using Hackney.Shared.Finance.Boundary.Response;
using FTHGeneralModels = FinancialTransactionsApi.V1.Helpers.GeneralModels;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class ResponseFactory
    {
        #region NugetPackageClasses

        public static PaginatedResponse<TransactionResponse> ToResponse(this Paginated<Transaction> ptrDomain)
        {
            var metadata = new MetadataModel
            {
                Pagination = ToPaginationDataResponse(ptrDomain)
            };

            return new PaginatedResponse<TransactionResponse>
            {
                Metadata = metadata,
                Results = ptrDomain.Results.ToResponse()
            };
        }

        private static PagingDetails ToPaginationDataResponse<TItem>(Paginated<TItem> paginatedResult) where TItem : class
        => new PagingDetails
        {
            ResultCount = paginatedResult.ResultCount,
            CurrentPage = paginatedResult.CurrentPage,
            PageSize = paginatedResult.PageSize,
            TotalCount = paginatedResult.TotalResultCount,
            PageCount = paginatedResult.PageCount
        };

        #endregion

        #region OldClasses

        public static FTHGeneralModels.PaginatedResponse<TransactionResponse> ToResponse(this FTHGeneralModels.Paginated<Transaction> ptrDomain)
        {
            var metadata = new MetadataModel
            {
                Pagination = ToPaginationDataResponse(ptrDomain)
            };

            return new FTHGeneralModels.PaginatedResponse<TransactionResponse>
            {
                Metadata = metadata,
                Results = ptrDomain.Results.ToResponse()
            };
        }

        private static PagingDetails ToPaginationDataResponse<TItem>(FTHGeneralModels.Paginated<TItem> paginatedResult) where TItem : class
            => new PagingDetails
            {
                ResultCount = paginatedResult.ResultCount,
                CurrentPage = paginatedResult.CurrentPage,
                PageSize = paginatedResult.PageSize,
                TotalCount = paginatedResult.TotalResultCount,
                PageCount = paginatedResult.PageCount
            };

        #endregion
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
                BankAccountNumber = domain.BankAccountNumber,
                SortCode = domain.SortCode,
                SuspenseResolutionInfo = domain.SuspenseResolutionInfo,
                PeriodNo = domain.PeriodNo,
                TransactionAmount = domain.TransactionAmount,
                TransactionDate = domain.TransactionDate,
                TransactionType = domain.TransactionType.GetDescription(),
                TransactionSource = domain.TransactionSource,
                IsSuspense = domain.IsSuspense,
                Address = domain.Address,
                Sender = domain.Sender,
                Fund = domain.Fund,
                CreatedAt = domain.CreatedAt,
                CreatedBy = domain.CreatedBy,
                LastUpdatedAt = domain.LastUpdatedAt,
                LastUpdatedBy = domain.LastUpdatedBy
            };
        }

        public static FTHelpers.ResponseWrapper<TransactionResponse> ToResponseWrapper(this Transaction domainList)
        {
            return new FTHelpers.ResponseWrapper<TransactionResponse>(domainList.ToResponse());
        }

        public static List<TransactionResponse> ToResponse(this IEnumerable<Transaction> domainList)
        {
            return domainList == null ?
                new List<TransactionResponse>() :
                domainList.Select(domain => domain.ToResponse()).ToList();
        }

        public static FTHelpers.ResponseWrapper<IEnumerable<TransactionResponse>> ToResponseWrapper(this IEnumerable<Transaction> domainList)
        {
            return new FTHelpers.ResponseWrapper<IEnumerable<TransactionResponse>>(domainList.Select(domain => domain.ToResponse()));
        }

    }
}
