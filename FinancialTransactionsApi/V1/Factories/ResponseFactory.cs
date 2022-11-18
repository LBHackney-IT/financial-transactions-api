using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;
using System.Linq;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using HSFPagination = Hackney.Shared.Finance.Pagination;
using HSFBResponse = Hackney.Shared.Finance.Boundary.Response;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class ResponseFactory
    {
        
    #region NugetPackageClasses

        public static HSFPagination.PaginatedResponse<TransactionResponse> ToResponse(this HSFPagination.Paginated<Transaction> ptrDomain)
        {
            var metadata = new HSFBResponse.MetadataModel
            {
                Pagination = ToPaginationDataResponse(ptrDomain)
            };

            return new HSFPagination.PaginatedResponse<TransactionResponse>
            {
                Metadata = metadata,
                Results = ptrDomain.Results.ToResponse()
            };
        }

        private static HSFPagination.PagingDetails ToPaginationDataResponse<TItem>(HSFPagination.Paginated<TItem> paginatedResult) where TItem : class
        => new HSFPagination.PagingDetails
        {
            ResultCount = paginatedResult.ResultCount,
            CurrentPage = paginatedResult.CurrentPage,
            PageSize = paginatedResult.PageSize,
            TotalCount = paginatedResult.TotalResultCount,
            PageCount = paginatedResult.PageCount
        };

    #endregion

        private static HSFPagination.PagingDetails ToPaginationDataResponse<TItem>(Paginated<TItem> paginatedResult) where TItem : class
        => new HSFPagination.PagingDetails
        {
            ResultCount = paginatedResult.ResultCount,
            CurrentPage = paginatedResult.CurrentPage,
            PageSize = paginatedResult.PageSize,
            TotalCount = paginatedResult.TotalResultCount,
            PageCount = paginatedResult.PageCount
        };

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
                PaymentReference = domain.PaymentReference,
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
                Fund = domain.Fund,
                CreatedAt = domain.CreatedAt,
                CreatedBy = domain.CreatedBy,
                LastUpdatedAt = domain.LastUpdatedAt,
                LastUpdatedBy = domain.LastUpdatedBy
            };
        }

        public static List<TransactionResponse> ToResponse(this IEnumerable<Transaction> domainList)
        {
            return domainList == null ?
                new List<TransactionResponse>() :
                domainList.Select(domain => domain.ToResponse()).ToList();
        }

        public static PaginatedResponse<TransactionResponse> ToResponse(this Paginated<Transaction> ptrDomain)
        {
            var metadata = new HSFBResponse.MetadataModel
            {
                Pagination = ToPaginationDataResponse(ptrDomain)
            };

            return new PaginatedResponse<TransactionResponse>
            {
                Metadata = metadata,
                Results = ptrDomain.Results.ToResponse()
            };
        }

        public static ResponseWrapper<TransactionResponse> ToResponseWrapper(this Transaction domainList)
        {
            return new ResponseWrapper<TransactionResponse>(domainList.ToResponse());
        }

        public static ResponseWrapper<IEnumerable<TransactionResponse>> ToResponseWrapper(this IEnumerable<Transaction> domainList)
        {
            return new ResponseWrapper<IEnumerable<TransactionResponse>>(domainList.Select(domain => domain.ToResponse()));
        }

    }
}