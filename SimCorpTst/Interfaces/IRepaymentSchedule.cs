using System;
using System.Collections.Generic;

namespace SimCorpTst.Interfaces
{
    public interface IRepaymentSchedule
    {
        IEnumerable<RepaymentItem> GetRepaymentSchedule();
    }
}
