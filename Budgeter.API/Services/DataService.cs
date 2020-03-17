using Budgeter.API.Extensions;
using Budgeter.API.Models;
using Budgeter.API.Requests;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Services
{
    public class DataService : IDataService
    {
        private static string _connectionString;
        private const string INSERT_CMD = "INSERT INTO public.transactions(\"TransactionDate\", \"PostDate\", \"Reference\", \"Description\", \"Amount\", \"AccountNumber\", \"CardNumber\", \"CardholderName\", \"MCC\", \"MCCDescription\", \"MCCGroup\") VALUES(@trxDate, @postDate, @reference, @dsc, @amt, @accNum, @cardNum, @cardName, @mcc, @mccDesc, @mccGrp);";
        private const string SELECT_TRX_PAYMENTS = "SELECT * FROM public.transactions LIMIT @take OFFSET @skip_trx;";
        private const string SELECT_ALL_EXPENSES = "SELECT * FROM public.transactions WHERE \"Amount\" < '0.00'";
        private const string SELECT_TRX_NO_PAYMENTS = "SELECT * FROM public.transactions WHERE \"Amount\" < '0.00' LIMIT @take OFFSET @skip_trx;";
        private const string SELECT_COUNT_PAYMENTS = "SELECT COUNT(*) FROM public.transactions;";
        private const string SELECT_COUNT_NO_PAYMENTS = "SELECT COUNT(*) FROM public.transactions WHERE \"Amount\" < '0.00';";
        public DataService(string connectionString)
        {
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = connectionString;
        }
        public async Task<IEnumerable<TransactionRecord>> GetTransactionsAsync(PagedTransactionRequest request)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                List<TransactionRecord> results = new List<TransactionRecord>();
                try
                {
                    using (var trx = await connection.BeginTransactionAsync())
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            if (request.IgnorePayments)
                                cmd.CommandText = SELECT_TRX_NO_PAYMENTS;
                            else
                                cmd.CommandText = SELECT_TRX_PAYMENTS;
                            cmd.AddParam("take", request.Take);
                            cmd.AddParam("skip_trx", request.Skip);
                            cmd.Transaction = trx;
                            using (var dr = await cmd.ExecuteReaderAsync())
                            {
                                while (await dr.ReadAsync())
                                {
                                    results.Add(new TransactionRecord()
                                    {
                                        TransactionDate = dr.GetDateTime(1),
                                        PostDate = dr.GetDateTime(2),
                                        ReferenceId = dr.GetString(3),
                                        Description = dr.GetString(4),
                                        Amount = dr.GetDecimal(5),
                                        AccountNumber = dr.GetString(6),
                                        CardNumber = dr.GetString(7),
                                        CardholderName = dr.GetString(8),
                                        Mcc = dr.GetString(9),
                                        MccDescription = dr.GetString(10),
                                        MccGroup = dr.GetString(11)
                                    });
                                }
                            }
                        }
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
                return results;
            }
        }

        public async Task<long> GetTotalTransactionCountAsync(bool ignorePayments = false)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        if (ignorePayments)
                            cmd.CommandText = SELECT_COUNT_NO_PAYMENTS;
                        else
                            cmd.CommandText = SELECT_COUNT_PAYMENTS;
                        return (long)await cmd.ExecuteScalarAsync();
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
        public async Task UploadTransactions(IEnumerable<TransactionRecord> transactions)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    foreach (var trx in transactions)
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.AddParam("trxDate", trx.TransactionDate);
                            cmd.AddParam("postDate", trx.PostDate);
                            cmd.AddParam("reference", trx.ReferenceId);
                            cmd.AddParam("dsc", trx.Description);
                            cmd.AddParam("amt", trx.Amount);
                            cmd.AddParam("accNum", trx.AccountNumber);
                            cmd.AddParam("cardNum", trx.CardNumber);
                            cmd.AddParam("cardName", trx.CardholderName);
                            cmd.AddParam("mcc", trx.Mcc);
                            cmd.AddParam("mccDesc", trx.MccDescription);
                            cmd.AddParam("mccGrp", trx.MccGroup);
                            cmd.CommandText = INSERT_CMD;
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<IEnumerable<TransactionRecord>> GetAllExpensesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                List<TransactionRecord> results = new List<TransactionRecord>();
                try
                {
                    using (var trx = await connection.BeginTransactionAsync())
                    {
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = SELECT_ALL_EXPENSES;
                            using(var dr = await cmd.ExecuteReaderAsync())
                            {
                                while (await dr.ReadAsync())
                                {
                                    results.Add(new TransactionRecord()
                                    {
                                        TransactionDate = dr.GetDateTime(1),
                                        PostDate = dr.GetDateTime(2),
                                        ReferenceId = dr.GetString(3),
                                        Description = dr.GetString(4),
                                        Amount = dr.GetDecimal(5),
                                        AccountNumber = dr.GetString(6),
                                        CardNumber = dr.GetString(7),
                                        CardholderName = dr.GetString(8),
                                        Mcc = dr.GetString(9),
                                        MccDescription = dr.GetString(10),
                                        MccGroup = dr.GetString(11)
                                    });
                                }
                            }
                        }
                    }
                    return results;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
