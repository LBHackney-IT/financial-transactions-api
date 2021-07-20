using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class TransactionFactory
    {
        public static TransactionDbEntity ToDatabase(this Transaction transaction)
        {
            return new TransactionDbEntity
            {
                Id = transaction.Id,
                TargetId = transaction.TargetId,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                FinancialMonth = transaction.FinancialMonth,
                FinancialYear = transaction.FinancialYear,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                IsSuspense = transaction.IsSuspense,
                SuspenseInfo = new SuspenseInfoDbEntity()
                {
                    ResolutionDate = transaction.SuspenseInfo?.ResolutionDate,
                    IsResolve = transaction.SuspenseInfo != null && transaction.SuspenseInfo.IsResolve,
                    Note = transaction.SuspenseInfo == null ? string.Empty : transaction.SuspenseInfo.Note
                },
                PeriodNo = transaction.PeriodNo,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = transaction.TransactionDate,
                TransactionType = transaction.TransactionType,
                TransactionSource = transaction.TransactionSource,
                Address = transaction.Address,
                Person = transaction.Person == null ? null :
                new PersonDbEntity()
                {
                    Id = transaction.Person.Id,
                    FullName = transaction.Person?.FullName
                },
                Fund = transaction.Fund
            };
        }

        public static Transaction ToDomain(this TransactionDbEntity transactionDbEntity)
        {
            return new Transaction
            {
                Id = transactionDbEntity.Id,
                TargetId = transactionDbEntity.TargetId,
                BalanceAmount = transactionDbEntity.BalanceAmount,
                ChargedAmount = transactionDbEntity.ChargedAmount,
                FinancialMonth = transactionDbEntity.FinancialMonth,
                FinancialYear = transactionDbEntity.FinancialYear,
                HousingBenefitAmount = transactionDbEntity.HousingBenefitAmount,
                PaidAmount = transactionDbEntity.PaidAmount,
                PaymentReference = transactionDbEntity.PaymentReference,
                IsSuspense = transactionDbEntity.IsSuspense,
                SuspenseInfo = new SuspenseInfo()
                {
                    ResolutionDate = transactionDbEntity.SuspenseInfo.ResolutionDate,
                    IsResolve = transactionDbEntity.SuspenseInfo.IsResolve,
                    Note = transactionDbEntity.SuspenseInfo.Note
                },
                PeriodNo = transactionDbEntity.PeriodNo,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                TransactionDate = transactionDbEntity.TransactionDate,
                TransactionType = transactionDbEntity.TransactionType,
                TransactionSource = transactionDbEntity.TransactionSource,
                Address = transactionDbEntity.Address,
                Person = transactionDbEntity.Person == null ? null :
                new Person()
                {
                    Id = transactionDbEntity.Person.Id,
                    FullName = transactionDbEntity.Person.FullName
                },
                Fund = transactionDbEntity.Fund
            };
        }

        public static Transaction ToDomain(this AddTransactionRequest transactionRequest)
        {
            return transactionRequest == null ? null : new Transaction
            {
                TargetId = transactionRequest.TargetId,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                FinancialMonth = transactionRequest.FinancialMonth,
                FinancialYear = transactionRequest.FinancialYear,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                IsSuspense = transactionRequest.IsSuspense,
                SuspenseInfo = new SuspenseInfo()
                {
                    ResolutionDate = null,
                    IsResolve = false,
                    Note = string.Empty
                },
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Person = transactionRequest.Person == null ? null :
                new Person()
                {
                    Id = transactionRequest.Person.Id,
                    FullName = transactionRequest.Person?.FullName
                },
                Fund = transactionRequest.Fund
            };
        }

        public static Transaction ToDomain(this UpdateTransactionRequest transactionRequest)
        {
            return transactionRequest == null ? null : new Transaction
            {
                TargetId = transactionRequest.TargetId,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                FinancialMonth = transactionRequest.FinancialMonth,
                FinancialYear = transactionRequest.FinancialYear,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                IsSuspense = transactionRequest.IsSuspense,
                SuspenseInfo = new SuspenseInfo()
                {
                    ResolutionDate = transactionRequest.SuspenseInfo.ResolutionDate,
                    IsResolve = transactionRequest.SuspenseInfo.IsResolve,
                    Note = transactionRequest.SuspenseInfo.Note
                },
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Person = new Person()
                {
                    Id = transactionRequest.Person.Id,
                    FullName = transactionRequest.Person.FullName
                },
                Fund = transactionRequest.Fund
            };
        }
    }
}
