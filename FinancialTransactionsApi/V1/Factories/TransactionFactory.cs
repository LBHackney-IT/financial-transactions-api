using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
                IsSuspense = transaction.IsSuspense,
                SuspenseResolutionInfo = transaction.SuspenseResolutionInfo,
                PeriodNo = transaction.PeriodNo,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = transaction.TransactionDate,
                TransactionType = transaction.TransactionType,
                TransactionSource = transaction.TransactionSource,
                Address = transaction.Address,
                Person = transaction.Person,
                Fund = transaction.Fund
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
                IsSuspense = transactionDbEntity.IsSuspense,
                SuspenseResolutionInfo = transactionDbEntity.SuspenseResolutionInfo,
                PeriodNo = transactionDbEntity.PeriodNo,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                TransactionDate = transactionDbEntity.TransactionDate,
                TransactionType = transactionDbEntity.TransactionType,
                TransactionSource = transactionDbEntity.TransactionSource,
                Address = transactionDbEntity.Address,
                Person = transactionDbEntity.Person,
                Fund = transactionDbEntity.Fund
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
                IsSuspense = transactionRequest.IsSuspense,
                SuspenseResolutionInfo = null,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Person = transactionRequest.Person,
                Fund = transactionRequest.Fund
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
                IsSuspense = transactionRequest.IsSuspense,
                SuspenseResolutionInfo = transactionRequest.SuspenseResolutionInfo,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType,
                TransactionSource = transactionRequest.TransactionSource,
                Address = transactionRequest.Address,
                Person = transactionRequest.Person,
                Fund = transactionRequest.Fund
            };
        }

        public static List<Transaction> ToDomain(this IEnumerable<TransactionDbEntity> databaseEntity)
        {
            //Func<DateTime, int> weekProjector = d => CultureInfo.CurrentCulture.Calendar
            //.GetDayOfWeek(d,CalendarWeekRule.FirstFourDayWeek,DayOfWeek.Sunday);

            var report = databaseEntity.Select(p => p.ToDomain()).ToList();
            var record = report.GroupBy(x => x.Person, (i, t) => new WeeklyReportResponse
            {
                Person = i.Person,
                Records = t.ToList()

            })
            return databaseEntity.Select(p => p.ToDomain())
                                 .OrderBy(x => x.TransactionDate)
                                 .ToList();
        }
    }
}
