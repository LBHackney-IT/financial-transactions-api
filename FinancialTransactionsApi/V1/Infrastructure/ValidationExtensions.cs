using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class ValidationExtensions
    {
        public static bool HaveAllFieldsInAddTransactionModel(this AddTransactionRequest transaction)
        {
            if (transaction.TransactionSource == null)
            {
                return false;
            }

            if (transaction.PaymentReference == null)
            {
                return false;
            }

            if (transaction.Fund == null)
            {
                return false;
            }

            return true;
        }

        public static bool HaveAllFieldsInUpdateTransactionModel(this UpdateTransactionRequest transaction)
        {
            if (transaction.TargetId == Guid.Empty)
            {
                return false;
            }

            if (transaction.TransactionSource == null)
            {
                return false;
            }

            if (transaction.PaymentReference == null)
            {
                return false;
            }

            if (transaction.Fund == null)
            {
                return false;
            }

            if (transaction.SuspenseResolutionInfo == null)
            {
                return false;
            }

            if (transaction.SuspenseResolutionInfo.ResolutionDate == null || !transaction.SuspenseResolutionInfo.IsResolve)
            {
                return false;
            }

            return true;
        }

        public static bool HaveAllFieldsInBatchProcessingModel(this Transaction transaction)
        {
            if (transaction.TargetId == Guid.Empty)
            {
                return false;
            }

            if (transaction.TransactionSource == null)
            {
                return false;
            }

            if (transaction.PeriodNo == 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(TransactionType), transaction.TransactionType))
            {
                return false;
            }

            return true;
        }
    }
}
