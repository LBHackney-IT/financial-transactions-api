using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FinancialTransactionsApi.V1.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class TransactionFactory
    {
        public static TransactionDbEntity ToDatabase(this Transaction transaction)
        {
            return transaction == null ? null : new TransactionDbEntity
            {
                Id = transaction.Id,
                TargetId = transaction.TargetId,
                TargetType = transaction.TargetType,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                FinancialMonth = transaction.FinancialMonth,
                FinancialYear = transaction.FinancialYear,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                BankAccountNumber = transaction.BankAccountNumber,
                SuspenseResolutionInfo = transaction.SuspenseResolutionInfo,
                PeriodNo = transaction.PeriodNo,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = transaction.TransactionDate,
                TransactionType = transaction.TransactionType,
                TransactionSource = transaction.TransactionSource,
                Address = transaction.Address,
                Sender = transaction.Sender,
                Fund = transaction.Fund,
                SortCode = transaction.SortCode,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = transaction.CreatedBy,
                LastUpdatedBy = transaction.LastUpdatedBy,
                LastUpdatedAt = transaction.LastUpdatedAt
            };
        }

        public static Transaction ToDomain(this TransactionDbEntity transactionDbEntity)
        {
            return transactionDbEntity == null ? null : new Transaction
            {
                Id = transactionDbEntity.Id,
                TargetId = transactionDbEntity.TargetId,
                TargetType = transactionDbEntity.TargetType,
                BalanceAmount = transactionDbEntity.BalanceAmount,
                ChargedAmount = transactionDbEntity.ChargedAmount,
                FinancialMonth = transactionDbEntity.FinancialMonth,
                FinancialYear = transactionDbEntity.FinancialYear,
                HousingBenefitAmount = transactionDbEntity.HousingBenefitAmount,
                PaidAmount = transactionDbEntity.PaidAmount,
                PaymentReference = transactionDbEntity.PaymentReference,
                BankAccountNumber = transactionDbEntity.BankAccountNumber,
                SuspenseResolutionInfo = transactionDbEntity.SuspenseResolutionInfo,
                PeriodNo = transactionDbEntity.PeriodNo,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                TransactionDate = transactionDbEntity.TransactionDate,
                TransactionType = transactionDbEntity.TransactionType,
                TransactionSource = transactionDbEntity.TransactionSource,
                Address = transactionDbEntity.Address,
                Sender = transactionDbEntity.Sender,
                Fund = transactionDbEntity.Fund,
                SortCode = transactionDbEntity.SortCode,
                CreatedAt = transactionDbEntity.CreatedAt,
                CreatedBy = transactionDbEntity.CreatedBy,
                LastUpdatedAt = transactionDbEntity.LastUpdatedAt,
                LastUpdatedBy = transactionDbEntity.LastUpdatedBy
            };
        }

        public static TransactionLimitedModel ToResponse(this TransactionLimitedDbEntity transactionDbEntity)
        {
            return transactionDbEntity == null ? null : new TransactionLimitedModel
            {
                Id = transactionDbEntity.Id,
                TargetId = transactionDbEntity.TargetId,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                HousingBenefitAmount = transactionDbEntity.HousingBenefitAmount,
                PaidAmount = transactionDbEntity.PaidAmount,
                BalanceAmount = transactionDbEntity.BalanceAmount,
                ChargedAmount = transactionDbEntity.ChargedAmount,
            };
        }

        public static Transaction ToDomain(this AddTransactionRequest transactionRequest)
        {
            return transactionRequest == null ? null : new Transaction
            {
                TargetId = transactionRequest.TargetId,
                TargetType = transactionRequest.TargetType,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                BankAccountNumber = transactionRequest.BankAccountNumber,
                SuspenseResolutionInfo = null,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Sender = transactionRequest.Sender,
                Fund = transactionRequest.Fund,
                SortCode = transactionRequest.SortCode
            };
        }

        public static Transaction ToDomain(this UpdateTransactionRequest transactionRequest)
        {
            return transactionRequest == null ? null : new Transaction
            {
                TargetId = transactionRequest.TargetId,
                TargetType = transactionRequest.TargetType,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                BankAccountNumber = transactionRequest.BankAccountNumber,
                SuspenseResolutionInfo = transactionRequest.SuspenseResolutionInfo,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Sender = transactionRequest.Sender,
                Fund = transactionRequest.Fund,
                SortCode = transactionRequest.SortCode
            };
        }

        public static Transaction ResponseToDomain(this TransactionResponse transactionRequest, SuspenseConfirmationRequest transaction, string lastUpdatedBy)
        {
            return transactionRequest == null ? null : new Transaction
            {
                Id = transactionRequest.Id,
                TargetId = transaction.TargetId,
                TargetType = transactionRequest.TargetType,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                BankAccountNumber = transactionRequest.BankAccountNumber,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = EnumHelper.GetValueFromDescription<TransactionType>(transactionRequest.TransactionType),
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Sender = transactionRequest.Sender,
                Fund = transactionRequest.Fund,
                SortCode = transactionRequest.SortCode,
                SuspenseResolutionInfo = new SuspenseResolutionInfo
                {
                    IsConfirmed = true,
                    IsApproved = true,
                    Note = transaction.Note,
                    ResolutionDate = DateTime.UtcNow
                },
                CreatedAt = transactionRequest.CreatedAt,
                CreatedBy = transactionRequest.CreatedBy,
                LastUpdatedBy = lastUpdatedBy
            };
        }

        public static List<Transaction> ToDomain(this IEnumerable<TransactionDbEntity> databaseEntity)
        {
            return databaseEntity.Select(p => p.ToDomain())
                                 .OrderBy(x => x.TransactionDate)
                                 .ToList();
        }

        public static IEnumerable<Transaction> ToDomain(this IEnumerable<AddTransactionRequest> transactionRequests)
        {
            return transactionRequests == null ?
                new List<Transaction>() : transactionRequests.Select(t => t.ToDomain());
        }

        public static List<TransactionDbEntity> ToDatabaseList(this List<Transaction> transactions)
        {
            return transactions.Select(item => item.ToDatabase()).ToList();
        }


        public static Transaction ToDomain(this TransactionPGEntity transactionPGEntity)
        {
            return transactionPGEntity == null ? null : new Transaction
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.Parse(transactionPGEntity.TargetId),
                TargetType = (TargetType) Enum.Parse(typeof(TargetType), transactionPGEntity.TargetType),
                BalanceAmount = transactionPGEntity.BalanceAmount,
                ChargedAmount = transactionPGEntity.ChargedAmount,
                FinancialMonth = (short)transactionPGEntity.FinancialMonth,
                FinancialYear = (short)transactionPGEntity.FinancialYear,
                HousingBenefitAmount = transactionPGEntity.HousingBenefitAmount ?? 0,
                PaidAmount = transactionPGEntity.PaidAmount ?? 0,
                PaymentReference = transactionPGEntity.PaymentReference,
                BankAccountNumber = transactionPGEntity.BankAccountNumber,
                //SuspenseResolutionInfo = transactionPGEntity.SuspenseResolutionInfo,
                PeriodNo = (short)transactionPGEntity.PeriodNumber,
                TransactionAmount = transactionPGEntity.TransactionAmount,
                TransactionDate = transactionPGEntity.TransactionDate,
                //TransactionType = transactionPGEntity.TransactionType,
                TransactionSource = transactionPGEntity.TransactionSource,
                Address = transactionPGEntity.Address,
                Sender = new Sender() { Id = Guid.NewGuid(), FullName = transactionPGEntity.Sender },
                Fund = transactionPGEntity.Fund ?? String.Empty,
                SortCode = transactionPGEntity.SortCode,
                CreatedAt = transactionPGEntity.CreatedAt,
                CreatedBy = transactionPGEntity.CreatedBy,
                LastUpdatedAt = transactionPGEntity.LastUpdatedAt.Value,
                LastUpdatedBy = transactionPGEntity.LastUpdatedBy
            };
        }
        public static List<Transaction> ToDomain(this IEnumerable<TransactionPGEntity> databaseEntity)
        {
            return databaseEntity.Select(p => p.ToDomain()).OrderBy(x => x.TransactionDate).ToList();
        }
    }
}
