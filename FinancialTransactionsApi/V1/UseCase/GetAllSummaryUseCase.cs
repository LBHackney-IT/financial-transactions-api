using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.UseCase.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllSummaryUseCase : IGetAllSummaryUseCase
    {
        private readonly ITransactionGateway _gateway;
        public GetAllSummaryUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<TransactionSummaryResponseObjectList> ExecuteAsync(Guid targetId, string startDate, string endDate)
        {
            var transactions = await _gateway.GetAllTransactionsSummaryAsync(targetId, startDate.CheckAndConvertDateTime(), endDate.CheckAndConvertDateTime()).ConfigureAwait(false);

            var result = transactions.Select(p => p.ToResponse()).ToList();

            return GetSummarisedResult(result, targetId);           
        }
        private static TransactionSummaryResponseObjectList GetSummarisedResult(List<TransactionResponseObject> filteredResult, Guid targetId)
        {
            var result = new TransactionSummaryResponseObjectList();
            result.ResponseObjects = new List<TransactionSummaryResponseObject>();

            var sortedResult = filteredResult.GroupBy(x => new { x.PeriodNo, x.FinancialMonth, x.FinancialYear })
                .OrderByDescending(a => a.Key.FinancialYear)
                .ThenByDescending(b => b.Key.FinancialMonth)
                .ThenByDescending(c => c.Key.PeriodNo)
                .Select(x => new
                {
                    TransactionDetails =  x.ToList(),
                    WeekNumber = x.Key.PeriodNo,
                    Month = x.Key.FinancialMonth,
                    Year = x.Key.FinancialYear
                }).ToList();

            sortedResult.ForEach(item =>
            {                
                decimal totalCharged = 0;
                decimal totalPaid = 0;
                decimal currentBalance = 0;
                decimal housingBenefit = 0;
                
                item.TransactionDetails.ForEach(detail =>
                {                   
                    if (detail.TransactionType == TransactionType.rent.ToString())
                    {
                        totalCharged += detail.TransactionAmount;
                    }
                    if (detail.TransactionType == TransactionType.payment.ToString())
                    {
                        totalPaid += detail.TransactionAmount;
                    }
                    if (detail.TransactionType == TransactionType.housingbenefit.ToString())
                    {
                        housingBenefit += detail.TransactionAmount;
                    }
                });
                currentBalance = totalCharged - (totalPaid + housingBenefit);               
                
                var summaryByWeek = new TransactionSummaryResponseObject
                {
                    HousingBenefitAmount = housingBenefit,
                    ChargedAmount = totalCharged,
                    BalanceAmount = currentBalance,
                    FinancialMonth = item.Month,
                    FinancialYear = item.Year,
                    PeriodNo = item.WeekNumber,
                    PaidAmount = totalPaid,
                    TargetId = targetId,
                    WeekStartDate = DateTimeHelper.GetWeekStartDate(item.Year, item.Month, item.WeekNumber)
                };
                result.ResponseObjects.Add(summaryByWeek);
            });
            return result;
        }
        
    }
    
}
