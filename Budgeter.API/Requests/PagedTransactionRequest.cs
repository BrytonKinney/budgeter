using System;

namespace Budgeter.API.Requests
{
    public class PagedTransactionRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IgnorePayments { get; set; }
    }
}
