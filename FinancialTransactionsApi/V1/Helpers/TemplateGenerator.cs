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


        public static string GetHTMLReportString(ExportResponse report)
        {
            var sb = new StringBuilder();
            sb.Append(@"<html>
   <head>
   </head>
    <body>
      <div>
         <p style='text-align:justify'><img src='~/images/Aspose.Words.f6aa76e6-3b31-4a27-b85a-52ed3b73ad65.001.png' width='277' height='69' alt='' style='-aw-left-pos:0pt; -aw-rel-hpos:column; -aw-rel-vpos:paragraph; -aw-top-pos:0pt; -aw-wrap-type:inline' /></p>
         <p style='margin-top:6.45pt; margin-right:137.2pt; margin-left:3pt; line-height:13.5pt'><span style='font-family:Arial; font-size:10pt; font-weight:bold; color:#222222; background-color:#ffffff'>Income Services, Hackney Service Centre, 1 Hillman Street, London E8 1DY Tel: 0208 356 3100 24 hour payment line: 0208 356 5050 </span><a href='http://www.hackney.gov.uk/your-rent' style='text-decoration:none'><span style='font-size:10pt; text-decoration:underline; color:#000000; background-color:#ffffff'>www.hackney.gov.uk/your-rent</span></a><a href='http://www.hackney.gov.uk/your-rent' target='_blank' style='text-decoration:none'><span style='height:0pt; margin-top:-6.45pt; display:block; position:absolute; z-index:-2'><img src='images/Aspose.Words.f6aa76e6-3b31-4a27-b85a-52ed3b73ad65.002.png' width='346' height='19' alt='' style='margin-top:0.46pt; margin-left:-1.45pt; border-style:none; -aw-left-pos:41pt; -aw-rel-hpos:page; -aw-rel-vpos:paragraph; -aw-top-pos:33.91pt; -aw-wrap-type:none; position:absolute' /></span></a></p>
         <p style='margin-top:0.8pt; margin-left:381.45pt; text-align:justify; line-height:11.15pt'><span style='font-family:Arial; font-size:10pt; color:#222222; background-color:#ffffff'>Date: 17 November 2021</span></p>
         <p style='margin-top:2.35pt; margin-left:383.65pt; text-align:justify; line-height:11.15pt'><span style='font-family:Arial; font-size:10pt; color:#222222; background-color:#ffffff'>Payment : xxxxxxxxx</span></p>
         <p style='margin-top:6pt; margin-right:444.15pt; margin-left:12.75pt; line-height:13.5pt'><span style='height:0pt; margin-top:-6pt; display:block; position:absolute; z-index:0'><img src='images/Aspose.Words.f6aa76e6-3b31-4a27-b85a-52ed3b73ad65.003.png' width='282' height='111' alt='' style='margin-top:-60.56pt; margin-left:-7.2pt; -aw-left-pos:45pt; -aw-rel-hpos:page; -aw-rel-vpos:paragraph; -aw-top-pos:-0.56pt; -aw-wrap-type:none; position:absolute' /></span><span style='font-family:Arial; font-size:10pt; background-color:#ffffff'>Name Address1 Address2 Address3 Postcode</span></p>
         <p style='margin-top:20.25pt; margin-left:78.4pt; text-align:justify; line-height:24.55pt'><span style='font-family:Arial; font-size:22pt; font-weight:bold; background-color:#ffffff'>STATEMENT OF YOUR ACCOUNT</span></p>
         <p style='margin-top:4.85pt; margin-left:111.95pt; text-align:justify; line-height:12.25pt'><span style='font-family:Arial; font-size:11pt; font-weight:bold; background-color:#ffffff'>for the period 27 September 2021 to 17 November 2021</span></p>
         <p style='margin-top:10.7pt; text-align:justify; font-size:1pt'><span style='font-family:Arial; background-color:#ffffff; -aw-import:spaces'>&#xa0;</span></p>
         <table cellspacing='0' cellpadding='0' style='width:502.7pt; margin-left:7.25pt; border:0.75pt solid #000000; -aw-border:0.5pt single; -aw-border-insideh:0.5pt single #000000; -aw-border-insidev:0.5pt single #000000; border-collapse:collapse'>
            <tr style='height:18pt; -aw-height-rule:exactly'>
               <td style='width:24.45pt; border-right-style:solid; border-right-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-right:27.18pt; padding-left:32.58pt; vertical-align:middle; background-color:#cccccc; -aw-border-bottom:0.5pt single; -aw-border-right:0.5pt single'>
                  <p style='margin-top:0.05pt; text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#cccccc'>Date</span></p>
               </td>
               <td style='width:87.5pt; border-right-style:solid; border-right-width:0.75pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-right:31.28pt; padding-left:36.48pt; vertical-align:middle; background-color:#cccccc; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single; -aw-border-right:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#cccccc'>Transaction Details</span></p>
               </td>
               <td style='width:27.5pt; border-right-style:solid; border-right-width:0.75pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-right:29.32pt; padding-left:34.22pt; vertical-align:middle; background-color:#cccccc; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single; -aw-border-right:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#cccccc'>Debit</span></p>
               </td>
               <td style='width:31pt; border-right-style:solid; border-right-width:0.75pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-right:21.28pt; padding-left:26.18pt; vertical-align:middle; background-color:#cccccc; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single; -aw-border-right:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#cccccc'>Credit</span></p>
               </td>
               <td style='width:39.5pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-right:22.22pt; padding-left:27.48pt; vertical-align:middle; background-color:#cccccc; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#cccccc'>Balance</span></p>
               </td>
            </tr>
            <tr style='height:18pt; -aw-height-rule:exactly'>
               <td style='width:84.2pt; border-top-style:solid; border-top-width:0.75pt; border-right-style:solid; border-right-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; vertical-align:middle; background-color:#ffffff; -aw-border-bottom:0.5pt single; -aw-border-right:0.5pt single; -aw-border-top:0.5pt single'>
                  <p><span style='-aw-import:ignore'>&#xa0;</span></p>
               </td>
               <td style='width:103.05pt; border-style:solid; border-width:0.75pt; padding-right:51.72pt; padding-left:0.48pt; vertical-align:middle; background-color:#ffffff; -aw-border:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; background-color:#ffffff'>Balance brought forward</span></p>
               </td>
               <td style='width:91.05pt; border-style:solid; border-width:0.75pt; vertical-align:middle; background-color:#ffffff; -aw-border:0.5pt single'>
                  <p><span style='-aw-import:ignore'>&#xa0;</span></p>
               </td>
               <td style='width:78.45pt; border-style:solid; border-width:0.75pt; vertical-align:middle; background-color:#ffffff; -aw-border:0.5pt single'>
                  <p><span style='-aw-import:ignore'>&#xa0;</span></p>
               </td>
               <td style='width:41.28pt; border-top-style:solid; border-top-width:0.75pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-left:47.92pt; vertical-align:middle; background-color:#ffffff; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single; -aw-border-top:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#ffffff'>£1,113.31</span></p>
               </td>
            </tr>");
            foreach (var item in report.Data)
            {
                sb.AppendFormat(@"
                <tr style='height:18pt; -aw-height-rule:exactly'>
               <td style='width:52.82pt; border-top-style:solid; border-top-width:0.75pt; border-right-style:solid; border-right-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-left:31.38pt; vertical-align:middle; background-color:#d9d9d9; -aw-border-bottom:0.5pt single; -aw-border-right:0.5pt single; -aw-border-top:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; background-color:#d9d9d9'>{0}</span></p>
               </td>
               <td style='width:66.35pt; border-style:solid; border-width:0.75pt; padding-right:86.92pt; padding-left:1.98pt; vertical-align:middle; background-color:#d9d9d9; -aw-border:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; background-color:#d9d9d9'>{1}</span></p>
               </td>
               <td style='width:29.62pt; border-style:solid; border-width:0.75pt; padding-left:61.42pt; vertical-align:middle; background-color:#d9d9d9; -aw-border:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; background-color:#d9d9d9'>{2}</span></p>
               </td>
               <td style='width:78.45pt; border-style:solid; border-width:0.75pt; vertical-align:middle; background-color:#d9d9d9; -aw-border:0.5pt single'>
                  <p><span style='-aw-import:ignore'>{3}</span></p>
               </td>
               <td style='width:41.28pt; border-top-style:solid; border-top-width:0.75pt; border-left-style:solid; border-left-width:0.75pt; border-bottom-style:solid; border-bottom-width:0.75pt; padding-left:47.92pt; vertical-align:middle; background-color:#d9d9d9; -aw-border-bottom:0.5pt single; -aw-border-left:0.5pt single; -aw-border-top:0.5pt single'>
                  <p style='text-align:justify; line-height:10.05pt'><span style='font-family:Arial; font-size:9pt; font-weight:bold; background-color:#d9d9d9'>{4}</span></p>
               </td>
            </tr>", item.TransactionDate,
                                  item.TransactionType,
                                  item.PaidAmount,
                                  item.HousingBenefitAmount,
                                  item.BalanceAmount);
            }
            sb.Append(@"</table>
         <p style='margin-top:3.7pt; margin-left:45.65pt; text-align:justify; line-height:14.5pt'><span style='font-family:Arial; font-size:13pt; font-weight:bold; background-color:#ffffff'>As of 17 November 2021 your account balance was £1008.31 in arrears.</span></p>
         <p style='margin-top:27.9pt; margin-left:14.25pt; line-height:10.5pt'><span style='height:0pt; margin-top:-27.9pt; display:block; position:absolute; z-index:-1'><img src='images/Aspose.Words.f6aa76e6-3b31-4a27-b85a-52ed3b73ad65.004.png' width='679' height='64' alt='' style='margin-top:-1.74pt; margin-left:-7.7pt; -aw-left-pos:46pt; -aw-rel-hpos:page; -aw-rel-vpos:paragraph; -aw-top-pos:19.26pt; -aw-wrap-type:none; position:absolute' /></span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222'>As your landlord, the council has a duty to make sure all charges are</span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222; -aw-import:spaces'>&#xa0; </span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222'>paid up to date. This is because the housing income goes toward the upkeep of council housing and providing services for residents.</span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222; -aw-import:spaces'>&#xa0; </span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222'>You must make weekly charges payment</span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222; -aw-import:spaces'>&#xa0; </span><span style='font-family:Arial; font-size:9pt; font-weight:bold; color:#ffffff; background-color:#222222'>a priority. If you don’t pay, you risk losing your home.</span></p>
      </div>
   </body>
</html>");
            return sb.ToString();
        }
    }
}
