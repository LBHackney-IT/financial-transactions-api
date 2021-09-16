using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class ValidationExtensions
    {
        public static bool HaveAllFieldsInAddTransactionModel(this AddTransactionRequest transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("AddTransactionRequest model cannot be null!");
            }

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

            if (transaction.Person == null)
            {
                return false;
            }

            if (transaction.Person.Id == Guid.Empty
                || string.IsNullOrWhiteSpace(transaction.Person.FullName))
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
            if (transaction == null)
            {
                throw new ArgumentNullException("UpdateTransactionRequest model cannot be null!");
            }

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

            if (transaction.Person == null)
            {
                return false;
            }

            if (transaction.Person.Id == Guid.Empty
                || string.IsNullOrWhiteSpace(transaction.Person.FullName))
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

            if (transaction.SuspenseResolutionInfo.ResolutionDate == null
                || transaction.SuspenseResolutionInfo.IsResolve == false)
            {
                return false;
            }

            return true;
        }

        public static bool HaveAllFieldsInAddWeeklyChargeModel(this AddTransactionRequest transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("AddTransactionRequest model cannot be null!");
            }

            if (transaction.TargetId == Guid.Empty)
            {
                return false;
            }

            if (transaction.TransactionSource == null)
            {
                return false;
            }

            if (transaction.ChargedAmount == decimal.Zero)
            {
                return false;
            }

            if (transaction.PeriodNo == 0)
            {
                return false;
            }

            if (!Enum.IsDefined( typeof(TransactionType), transaction.TransactionType) )
            {
                return false;
            }

            return true;
        }
    }
}
