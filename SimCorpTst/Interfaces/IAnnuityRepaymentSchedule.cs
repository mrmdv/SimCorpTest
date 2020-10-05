using System;

namespace SimCorpTst.Interfaces
{
    public interface IAnnuityRepaymentSchedule : IRepaymentSchedule
    {
        DateTime AgreementDate { get; }
        DateTime FirstPaymentDate { get; }
        double InvestmentSum { get; }
        double InterestRate { get; }
        int PeriodYear { get; }
        double MonthlyPaymentAmount { get; }
    }
}
