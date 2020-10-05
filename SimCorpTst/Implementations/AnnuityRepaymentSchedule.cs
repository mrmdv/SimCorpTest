using System;
using System.Collections.Generic;
using SimCorpTst.Interfaces;

namespace SimCorpTst.Implementations
{
    public class AnnuityRepaymentSchedule : IAnnuityRepaymentSchedule
    {
        private const int _iterationCountLimit = 10000;
        private readonly DateTime _agreementDate;
        private readonly DateTime _firstPaymentDate;
        private readonly double _investmentSum;
        private readonly double _interestRate;
        private readonly int _periodYear;
        private readonly int _periodMonth;
        private readonly int _firstPeriodDay;
        private double? _monthlyPaymentAmount;

        private IEnumerable<RepaymentItem> _repaymentItems = null;

        public AnnuityRepaymentSchedule(DateTime agreementDate, DateTime calculationDate, double investmentSum, double interestRate, int periodYear)
        {
            _agreementDate = agreementDate;
            _firstPaymentDate = calculationDate;
            _investmentSum = investmentSum;
            _interestRate = interestRate;
            _periodYear = periodYear;

            _periodMonth = 12 * _periodYear;
            _firstPeriodDay = (_firstPaymentDate - _agreementDate).Days;
        }

        public DateTime AgreementDate => _agreementDate;
        public DateTime FirstPaymentDate => _firstPaymentDate;
        public double InvestmentSum => _investmentSum;
        public double InterestRate => _interestRate;
        public int PeriodYear => _periodYear;
        public double MonthlyPaymentAmount { 
            get 
            {
                if (!_monthlyPaymentAmount.HasValue)
                {
                    int iteration = 0;
                    double valueFx;
                    double monthlyPaymentAmount = 10;

                    // find monthly payment
                    do
                    {
                        valueFx = Fx(monthlyPaymentAmount);
                        monthlyPaymentAmount -= valueFx / (-_periodMonth);
                        iteration++;

                        if (double.IsNaN(valueFx))
                        {
                            throw new Exception("Can't find monthly payment.");
                        }

                        if (iteration > _iterationCountLimit)
                        {
                            throw new Exception("Iteration count limit exceeded.");
                        }
                    }
                    while (Math.Abs(valueFx) >= 0.3);

                    _monthlyPaymentAmount = Math.Round(monthlyPaymentAmount, 2, MidpointRounding.ToEven);
                }

                return _monthlyPaymentAmount.Value;
            }
        }

        public IEnumerable<RepaymentItem> GetRepaymentSchedule()
        {
            if (_repaymentItems == null)
            {
                var monthlyPaymentAmount = this.MonthlyPaymentAmount;

                var repaymentItems = new List<RepaymentItem>();
                var restOfInvestmentSum = _investmentSum;
                double refundAmount;

                // set first not standart period
                double interestAmount = Math.Round(restOfInvestmentSum * _interestRate * _firstPeriodDay / 360, 2, MidpointRounding.ToEven);
                restOfInvestmentSum -= (monthlyPaymentAmount - interestAmount);
                repaymentItems.Add(new RepaymentItem(_firstPaymentDate, interestAmount, monthlyPaymentAmount - interestAmount, restOfInvestmentSum));

                for (var i = 1; i < _periodMonth; i++)
                {
                    interestAmount = Math.Round(restOfInvestmentSum * _interestRate * 30 / 360, 2, MidpointRounding.ToEven);
                    if (i < _periodMonth - 1)
                    {
                        restOfInvestmentSum -= (monthlyPaymentAmount - interestAmount);
                        refundAmount = monthlyPaymentAmount - interestAmount;
                    }
                    else
                    {
                        // last payment usually different for some centes because of rounding
                        refundAmount = restOfInvestmentSum;
                        restOfInvestmentSum = 0;
                    }

                    repaymentItems.Add(new RepaymentItem(_firstPaymentDate.AddMonths(i), interestAmount, refundAmount, restOfInvestmentSum));
                }

                _repaymentItems = repaymentItems;
            }

            return _repaymentItems;
        }

        private double Fx(double monthlyPaymentAmount)
        {
            var result = _investmentSum;
            // first not full period days
            result += result * _interestRate * _firstPeriodDay / 360 - monthlyPaymentAmount;
            for (var i = 1; i < _periodMonth; i++)
            {
                result += result * _interestRate * 30 / 360 - monthlyPaymentAmount;
            }

            return result;
        }
    }
}
