using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FinancialTransactionsApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Tenure
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        [Description("Section 20 Rebate")]
        Section20Rebate,
        [Description("Section 125 Rebate")]
        Section125Rebate,
        [Description("Assignment SC Trans")]
        AssignmentSCTrans,
        [Description("Basic Rent (No VAT)")]
        BasicRentNoVAT,
        [Description("MW Balance Transfer")]
        MWBalanceTransfer,
        [Description("C Preliminaries")]
        CPreliminaries,
        [Description("C Provisional Sums")]
        CProvisionalSums,
        [Description("C Contingency Sums")]
        CContingencySums,
        [Description("C Professional Fees")]
        CProfessionalFees,
        [Description("C Administration")]
        CAdministration,
        [Description("Cleaning (Block)")]
        CleaningBlock,
        [Description("Court Costs")]
        CourtCosts,
        [Description("Cleaning (Estate)")]
        CleaningEstate,
        [Description("Contents Insurance")]
        ContentsInsurance,
        [Description("Concierge")]
        Concierge,
        [Description("Car Port")]
        CarPort,
        [Description("Communal Digital TV")]
        CommunalDigitalTV,
        [Description("Garage (Attached)")]
        GarageAttached,
        [Description("Grounds Maintenance")]
        GroundsMaintenance,
        [Description("Ground Rent")]
        GroundRent,
        [Description("Host Amenity")]
        HostAmenity,
        [Description("Heating")]
        Heating,
        [Description("Heating Maintenance")]
        HeatingMaintenance,
        [Description("Hot Water")]
        HotWater,
        [Description("Interest")]
        Interest,
        [Description("Arrangement Interest")]
        ArrangementInterest,
        [Description("Lost Key Fobs")]
        LostKeyFobs,
        [Description(@"\Legacy Debit")]
        LegacyDebit,
        [Description("Lost Key Charge")]
        LostKeyCharge,
        [Description("Landlord Lighting")]
        LandlordLighting,
        [Description("Late Payment Charge")]
        LatePaymentCharge,
        [Description("Major Works Capital")]
        MajorWorksCapital,
        [Description("TA Management Fee")]
        TAManagementFee,
        [Description("MW Judgement Trans")]
        MWJudgementTrans,
        [Description("Major Works Loan")]
        MajorWorksLoan,
        [Description("Major Works Revenue")]
        MajorWorksRevenue,
        [Description("Parking Permits")]
        ParkingPermits,
        [Description("Parking Annual Chg")]
        ParkingAnnualChg,
        [Description("R Preliminaries")]
        RPreliminaries,
        [Description("R Provisional Sums")]
        RProvisionalSums,
        [Description("R Contingency Sums")]
        RContingencySums,
        [Description("R Professional Fees")]
        RProfessionalFees,
        [Description("R Administration Fee")]
        RAdministrationFee,
        [Description("Rechg Repairs no VAT")]
        RechgRepairsnoVAT,
        [Description("Rechargeable Repairs")]
        RechargeableRepairs,
        [Description("SC Adjustment")]
        SCAdjustment,
        [Description("SC Balancing Charge")]
        SCBalancingCharge,
        [Description("Service Charges")]
        ServiceCharges,
        [Description("SC Judgement Debit")]
        SCJudgementDebit,
        [Description("Shared Owners Rent")]
        SharedOwnersRent,
        [Description("Reserve Fund")]
        ReserveFund,
        [Description("Storage")]
        Storage,
        [Description("Basic Rent Temp Acc")]
        BasicRentTempAcc,
        [Description("Travellers Charge")]
        TravellersCharge,
        [Description("Tenants Levy")]
        TenantsLevy,
        [Description("Television License")]
        TelevisionLicense,
        [Description("VAT Charge")]
        VATCharge,
        [Description("Water Rates")]
        WaterRates,
        [Description("Water Standing Chrg.")]
        WaterStandingChrg,
        [Description("Watersure Reduction")]
        WatersureReduction,
        [Description("Bailiff Payment")]
        BailiffPayment,
        [Description("Bank Payment")]
        BankPayment,
        [Description("PayPoint/Post Office")]
        PayPointPostOffice,
        [Description("Rep. Cash Incentive")]
        RepCashIncentive,
        [Description("Cash Office Payments")]
        CashOfficePayments,
        [Description("Debit / Credit Card")]
        DebitOrCreditCard,
        [Description("MW Credit Transfer")]
        MWCreditTransfer,
        [Description("Direct Debit")]
        DirectDebit,
        [Description("Direct Debit Unpaid")]
        DirectDebitUnpaid,
        [Description("Deduction (Work & P)")]
        DeductionWorkAndP,
        [Description("BACS Refund")]
        BACSRefund,
        [Description("Deduction (Salary)")]
        DeductionSalary,
        [Description("DSS Transfer")]
        DSSTransfer,
        [Description("Tenant Refund")]
        TenantRefund,
        [Description("HB Adjustment")]
        HBAdjustment,
        [Description("Housing Benefit")]
        HousingBenefit,
        [Description("Internal Transfer")]
        InternalTransfer,
        [Description("MW Loan Payment")]
        MWLoanPayment,
        [Description(@"\Opening Balance")]
        OpeningBalance,
        [Description("Prompt Pay. Discount")]
        PromptPayDiscount,
        [Description("Postal Order")]
        PostalOrder,
        [Description("PayPoint/Post Office (civicapay)")]
        PayPointPostOfficecivicapay,
        [Description("Cheque Payments")]
        ChequePayments,
        [Description("Returned Cheque")]
        ReturnedCheque,
        [Description("Recharge Rep. Credit")]
        RechargeRepCredit,
        [Description("SC Judgement Trans")]
        SCJudgementTrans,
        [Description("Standing Order")]
        StandingOrder,
        [Description("TMO Reversal")]
        TMOReversal,
        [Description("Universal Credit Rec")]
        UniversalCreditRec,
        [Description("Rent waiver")]
        Rentwaiver,
        [Description("Write Off")]
        WriteOff,
        [Description("Write On")]
        WriteOn
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatementType
    {
        Quaterly,
        Yearly
    }
}
