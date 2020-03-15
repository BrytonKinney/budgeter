using Budgeter.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Responses
{
    public class TransactionsResponse
    {
        public IEnumerable<TransactionRecord> Data { get; set; }
        public long Total { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
