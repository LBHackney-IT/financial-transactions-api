using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class QueryResponseExtension
    {
        public static List<Transaction> ToTransactions(this QueryResponse response)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                SuspenseResolutionInfo suspenseResolutionInfo = null;
                Sender person = null;

                if (item.Keys.Any(p => p == "suspense_resolution_info"))
                {
                    var innerItem = item["suspense_resolution_info"].M;
                    suspenseResolutionInfo = new SuspenseResolutionInfo
                    {
                        IsConfirmed = innerItem["isConfirmed"].BOOL,
                        IsApproved = innerItem["isApproved"].BOOL,
                        Note = innerItem["note"].S,
                        ResolutionDate = DateTime.Parse(innerItem["resolutionDate"].S)
                    };
                }

                if (item.Keys.Any(p => p == "person"))
                {
                    var innerItem = item["person"].M;
                    person = new Sender()
                    {
                        Id = Guid.Parse(innerItem["id"].S),
                        FullName = innerItem["fullName"].S
                    };
                }

                transactions.Add(new Transaction
                {

                    Id = Guid.Parse(item["id"].S),
                    Address = item["address"].S,
                    BalanceAmount = decimal.Parse(item["balance_amount"].N),
                    BankAccountNumber = item["bank_account_number"].S,
                    ChargedAmount = decimal.Parse(item["charged_amount"].N),
                    FinancialMonth = short.Parse(item["financial_month"].N),
                    FinancialYear = short.Parse(item["financial_year"].N),
                    Fund = item["fund"].S,
                    HousingBenefitAmount = decimal.Parse(item["housing_benefit_amount"].N),
                    PaidAmount = decimal.Parse(item["paid_amount"].N),
                    PaymentReference = item["payment_reference"].S,
                    PeriodNo = short.Parse(item["period_no"].N),
                    Sender = person,
                    SuspenseResolutionInfo = suspenseResolutionInfo,
                    TargetId = Guid.Parse(item["target_id"].S),
                    TransactionAmount = decimal.Parse(item["transaction_amount"].N),
                    TransactionDate = DateTime.Parse(item["transaction_date"].S),
                    TransactionSource = item["transaction_source"].S,
                    TransactionType = Enum.Parse<TransactionType>(item["transaction_type"].S.Trim()),
                    LastUpdatedBy = item.ContainsKey("last_updated_by") ? item["last_updated_by"].S : null,
                    LastUpdatedAt = DateTime.Parse(item["last_updated_at"].S),
                    CreatedBy = item.ContainsKey("created_by") ? item["created_by"].S : null,
                    CreatedAt = DateTime.Parse(item["created_at"].S),
                });
            }

            return transactions;
        }
    }
}
