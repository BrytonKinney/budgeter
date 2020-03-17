using Budgeter.API.Models;
using System;

namespace Budgeter.API.DataAccess
{
    public class TransactionMapping : DataMapper<TransactionRecord>
    {
        public TransactionMapping()
        {
            Column<DateTime>(trx => trx.TransactionDate, nameof(TransactionRecord.TransactionDate));
            Column<DateTime>(trx => trx.PostDate, nameof(TransactionRecord.PostDate));
            Column<string>(trx => trx.AccountNumber, nameof(TransactionRecord.AccountNumber));
            Column<decimal>(trx => trx.Amount, nameof(TransactionRecord.Amount));
            Column<string>(trx => trx.CardholderName, nameof(TransactionRecord.CardholderName));
            Column<string>(trx => trx.CardNumber, nameof(TransactionRecord.CardNumber));
            Column<string>(trx => trx.Description, nameof(TransactionRecord.Description));
            Column<string>(trx => trx.Mcc, nameof(TransactionRecord.Mcc));
            Column<string>(trx => trx.MccDescription, nameof(TransactionRecord.MccDescription));
            Column<string>(trx => trx.MccGroup, nameof(TransactionRecord.MccGroup));
            Column<string>(trx => trx.ReferenceId, nameof(TransactionRecord.ReferenceId));
        }
    }
}
