using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Text;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class TemplateGenerator
    {
        //public static string GetHTMLString(ExportResponse report, string capture)
        //{
        //	var sb = new StringBuilder();
        //	sb.AppendFormat(@"
        //				<html>
        //					<head>
        //					</head>
        //					<body>
        //						<div class='header'><h1>{0} Statement Report</h1></div>
        //						<table align='left'id='prt'>
        //						<tr>
        //						<th>Name</th>
        //						<th>{1}</th>        
        //					   </tr>
        //						<tr>
        //						<th>Statement Period</th>
        //						<th>{2}</th>        
        //					   </tr>
        //						  <tr>
        //						<th>Total Charge</th>
        //						<th>{3}</th>        
        //					   </tr>
        //						  <tr>
        //						<th>Total Housing Benefit</th>
        //						<th>{4}</th>        
        //					   </tr>
        //						<tr>
        //						<th>Total Paid</th>
        //						<th>{5}</th>        
        //					   </tr>
        //						</table>
        //						<table align='center'>
        //							<tr>
        //								<th>Date</th>
        //								<th>Rent Account No</th>
        //								<th>Type</th>
        //								<th>Charge</th>
        //								<th>Paid</th>
        //								<th>HB Cont.</th>
        //								<th>Balance</th>
        //							</tr>", capture, report.FullName, report.StatementPeriod,
        //									report.TotalCharge, report.TotalHousingBenefit, report.TotalPaid);
        //	foreach (var item in report.Data)
        //	{
        //		sb.AppendFormat(@"<tr>
        //							<td>{0}</td>
        //							<td>{1}</td>
        //							<td>{2}</td>
        //							<td>{3}</td>
        //							<td>{4}</td>
        //							<td>{5}</td>
        //							<td>{6}</td>
        //						  </tr>", item.TransactionDate,
        //						  item.PaymentReference,
        //						  item.TransactionType,
        //						  item.ChargedAmount,
        //						  item.PaidAmount,
        //						  item.HousingBenefitAmount,
        //						  item.BalanceAmount);
        //	}
        //	sb.Append(@"
        //						</table>
        //					</body>
        //				</html>");
        //	return sb.ToString();
        //}


        public static string GetHTMLReportString(ExportResponse report)
        {
            var date = DateTime.Today.ToString("D");
            var sb = new StringBuilder();
            sb.Clear();
            sb.Append(@"<html><body>
<div id='page_1'>
<div id='p1dimg1'>
<img src='data:image/png;base64,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wLTx2Yvan3XmWxdPUWx9XKHVKJs3x6jFoO71waoA16nJG4+51bN18+ZpETC+w27UWDe3ejc+1q9as2bVmrV1adLiaX9mSxz98/eo9erExvKueSyOeutl+kfs5GaD49FhtcSvPP/JX7nsohjMjBbHiy3tO/blKxqWZvwdjxBcXJcpYzAj5nubGliTjn708cpMGa02vnywDXYnylpEb632OOSJj1z6zLUwJq5EtNIWm2z6drT33qNN/SmMK62vvEhy6fPL/1nqVpdQlui9X3z5paUMapTn/PYjxk+z3VtXhrNrogw+89XP91Hb/uRF7f3G++yzsDfutwmwh00+HjDo5Z63u+PHvv7ViXu3rURttbWFS1765t171h7ltf/y/uPf+6bxf0zW2uPufb+4BlO47OBAepl6NHrLY8cPL4+5X6xeF1POkowPa9daWqtl01Vr51ad9sdP/sPTXhIRfQdbkVGrdbDdQMKIiKj1R1dtvNGarXbNx/cmGpWttvXLlyOUUSmDuv1FdLtkPANPjYtrucnxf/SDU942cSjideSQmxz4he9/v0078bPQautt2wscdqJHXLXpyoU2GtbBSff7g+d86F2bW4vetpqhubdRqSe+6+/6mnUTfwv6aOG8Z71quP2VhwA3OJM7YFT68x9y1C+97BkxcTh9WbyycPcOmfbeS/Q+s/xHdsIB77p4bfqUL1/Xli4Ve+zpr4x162Nh6qn3WqJftfGnDjzwcfe639N+66Hj49YPe8NLe4npt05otY/alIlsex187cILbnTLw1d+chARpVx4yeXf/tEPJyyuxLDU1ftMnk9wpaW7963YIpYSvffWIuqPy+gl//6BE3/1ATtdzrXl1251h3/63HnThoqMTn3XqLWr1SW9jxZqHfbFE0Klx+aNl8fsmm1OANVee28v+fC76mAwedLD0cLB6/a7mq8G4HppcgeUUo44/DaLU7uXGtFrL1v2X1u0nZ1Z34Gli+6WH7/NH/oetbQWcdWmtjCqwx3cSe8am7aNqbX22qOd9okPx8xslLryXjqL8+ZGrSXucMBNzn71qcv341kcU1ZKLxNmu1teetu8KQbDic9eS3ntJ/7pLrc8psbyNIVR+qj1wf/396/8p8+eO+GygNIiBqcfdezkl7J4u78etfQNGwZr1/XR1pf2L94nsC+0/pS3vG5lB2x/2cK16zfv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' id='p1img1'></div>


<div class='dclr'></div>
<div id='id1_1'>
<p class='p0 ft0'>Income Services, Hackney Service Centre, 1 Hillman Street, London E8 1DY</p>
<p class='p1 ft0'>Tel: 0208 356 3100 24 hour payment line: 0208 356 5050</p>
<p class='p0 ft1'><nobr><a href='http://www.hackney.gov.uk/your-rent'>www.hackney.gov.uk/your-rent</a></nobr></p>");
            sb.AppendFormat(@"
<p class='p2 ft2'>Date: {0}</p>
<p class='p3 ft2'>Payment : xxxxxxxxx</p>
<p class='p4 ft3'>Name</p>
<p class='p5 ft3'>Address1</p>
<p class='p5 ft3'>Address2</p>
<p class='p5 ft3'>Address3</p>
<p class='p5 ft3'>Postcode</p>
<p class='p6 ft4'>STATEMENT OF YOUR ACCOUNT</p>
<p class='p7 ft5'>for the period 27 September 2021 to 17 November 2021</p>
<table cellpadding='0' cellspacing='0' class='t0'>
<tbody>
<tr>
	<td colspan='2' class='tr0 td0'><p class='p8 ft6'>Date</p></td>
	<td class='tr0 td1'><p class='p9 ft6'>Transaction Details</p></td>
	<td class='tr0 td2'><p class='p10 ft6'>Debit</p></td>
	<td class='tr0 td4'><p class='p12 ft6'>Credit</p></td>
	<td class='tr0 td5'><p class='p13 ft6'>Balance</p></td>
</tr>
<tr>
	<td colspan='2' class='tr1 td6'><p class='p16 ft8'>&nbsp;</p></td>
	<td class='tr1 td8'><p class='p17 ft8'>Balance brought forward</p></td>
	<td class='tr1 td9'><p class='p11 ft7'>&nbsp;</p></td>
	<td class='tr1 td11'><p class='p15 ft8'>&nbsp;</p></td>
	<td class='tr1 td12'><p class='p15 ft6'>£1,113.31</p></td>
</tr>", date);
            foreach (var item in report.Data)
            {
                sb.AppendFormat(@"
				<tr>
	<td colspan='2' class='tr1 td25'><p class='p16 ft8'>{0}</p></td>
	<td class='tr1 td8'><p class='p17 ft8'>{1}</p></td>
	<td class='tr1 td10'><p class='p11 ft8'>{2}</p></td>
	<td class='tr1 td11'><p class='p15 ft8'><nobr>{3}</nobr></p></td>
	<td class='tr1 td12'><p class='p15 ft6'>{4}</p></td>
</tr>", item.Date, item.TransactionDetail, item.Debit, item.Credit, item.Balance);
            };

            sb.AppendFormat(@"
</tbody>
</table>
<p class='p20 ft10 clearfix'>As of 17 November 2021 your account balance was £1008.31 in arrears.</p>
</div>", 2);
            sb.Append(@"
<div class='clearfix'></div>
<div id='id1_2'>
<p class='p0 ft11'>As your landlord, the council has a duty to make sure all charges are paid up to date. This is because the housing income goes toward the upkeep of council housing and providing services for residents. You must make weekly charges payment a priority. If you don’t pay, you risk losing your home.</p>
</div>
</div>

</body></html>");


            //sb.Append(@"</TABLE>
            //        <P class='p20 ft10'>As of 17 November 2021 your account balance was £1008.31 in arrears.</P>
            //        </DIV>");
            // sb.Append(@"<DIV id='id1_2'>
            //  <P class='p0 ft11'>As your landlord, the council has a duty to make sure all charges are paid up to date. This is because the housing income goes toward the upkeep of council housing and providing services for residents. You must make weekly charges payment a priority. If you don’t pay, you risk losing your home.</P>
            //        </DIV>
            //        </DIV>
            //        <DIV id='page_2'>


            //        </DIV>
            //        </body>
            //        </html>");

            return sb.ToString();
        }
    }
}
