using System;
using System.Linq;
using SimCorpTst.Implementations;

namespace SimCorpTst
{
    class Program
    {
        static void Main(string[] args)
        {
            var annuityRepaymentCalculator = new AnnuityRepaymentSchedule(new DateTime(2020, 01, 01), new DateTime(2020, 01, 15), 10000.0, 0.15, 5);

            int i = 1;
            Console.WriteLine($"Agreement Date:{annuityRepaymentCalculator.AgreementDate.ToString("yyyy-MM-dd")}  Monthly refund: {annuityRepaymentCalculator.MonthlyPaymentAmount.ToString("0.00")}");
            Console.WriteLine("Refund Schedule:");
            foreach (var repaymentItem in annuityRepaymentCalculator.GetRepaymentSchedule())
            {
                Console.WriteLine($"{i++}) Payment Date:{repaymentItem.PaymentDate.ToString("yyyy-MM-dd")}   Interest Amount:{repaymentItem.InterestAmount.ToString("0.00")}   Refund Amount:{repaymentItem.RefundAmount.ToString("0.00")}    Rest Of Investment Sum:{repaymentItem.RestOfInvestmentSum.ToString("0.00")}");
            }

            Console.WriteLine($"Sum of all future interest payments: {annuityRepaymentCalculator.GetRepaymentSchedule().Sum(s => s.InterestAmount)}");

            Console.ReadKey();
        }
    }
}
