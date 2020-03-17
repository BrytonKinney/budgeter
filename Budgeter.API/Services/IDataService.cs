using Budgeter.API.Models;
using Budgeter.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Services
{
    public interface IDataService
    {
        Task<IEnumerable<TransactionRecord>> GetTransactionsAsync(PagedTransactionRequest request);
        Task UploadTransactions(IEnumerable<TransactionRecord> transactions);
        Task<long> GetTotalTransactionCountAsync(bool ignorePayments = false);

        Task<IEnumerable<TransactionRecord>> GetAllExpensesAsync();
    }
}
