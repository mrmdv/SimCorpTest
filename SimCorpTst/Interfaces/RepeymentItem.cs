using System;

namespace SimCorpTst.Interfaces
{
    public class RepaymentItem
    {
        public RepaymentItem(DateTime paymentDate, double interestAmount, double refundAmount, double restOfInvestmentSum)
        {
            PaymentDate = paymentDate;
            InterestAmount = interestAmount;
            RefundAmount = refundAmount;
            RestOfInvestmentSum = restOfInvestmentSum;
        }

        public DateTime PaymentDate { get; }
        public double InterestAmount { get; }
        public double RefundAmount { get; }
        public double RestOfInvestmentSum { get; }
    }
}
