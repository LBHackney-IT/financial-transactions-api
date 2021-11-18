using FinancialTransactionsApi.V1.Boundary.Response;
using System.Text;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(ExportResponse report, string capture)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>{0} Statement Report</h1></div>
                                <table align='left'id='prt'>
                                <tr>
                                <th>Name</th>
                                <th>{1}</th>        
                               </tr>
                                <tr>
                                <th>Statement Period</th>
                                <th>{2}</th>        
                               </tr>
                                  <tr>
                                <th>Total Charge</th>
                                <th>{3}</th>        
                               </tr>
                                  <tr>
                                <th>Total Housing Benefit</th>
                                <th>{4}</th>        
                               </tr>
                                <tr>
                                <th>Total Paid</th>
                                <th>{5}</th>        
                               </tr>
                                </table>
                                <table align='center'>
                                    <tr>
                                        <th>Date</th>
                                        <th>Rent Account No</th>
                                        <th>Type</th>
                                        <th>Charge</th>
                                        <th>Paid</th>
                                        <th>HB Cont.</th>
                                        <th>Balance</th>
                                    </tr>", capture, report.FullName, report.StatementPeriod,
                                            report.TotalCharge, report.TotalHousingBenefit, report.TotalPaid);
            foreach (var item in report.Data)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>", item.TransactionDate,
                                  item.PaymentReference,
                                  item.TransactionType,
                                  item.ChargedAmount,
                                  item.PaidAmount,
                                  item.HousingBenefitAmount,
                                  item.BalanceAmount);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
