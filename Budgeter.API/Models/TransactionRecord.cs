using Budgeter.FileParsers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Models
{
    public class TransactionRecord
    {
        [CsvColumn(columnName: "TranDate", displayName: "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [CsvColumn(columnName: "PostDate", displayName: "Post Date")]
        public DateTime PostDate { get; set; }

        [CsvColumn(columnName: "Reference", displayName: "Reference ID")]
        public string ReferenceId { get; set; }

        [CsvColumn(columnName: "Description")]
        public string Description { get; set; }

        [CsvColumn(columnName: "Amount")]
        public decimal Amount { get; set; }

        [CsvColumn(columnName: "AccountNumber", displayName: "Account Number")]
        public string AccountNumber { get; set; }

        [CsvColumn(columnName: "CardNumber", displayName: "Card Number")]
        public string CardNumber { get; set; }

        [CsvColumn(columnName: "CardholderName", displayName: "Cardholder Name")]
        public string CardholderName { get; set; }

        [CsvColumn(columnName: "MCC")]
        public string Mcc { get; set; }

        [CsvColumn(columnName: "MCC Description")]
        public string MccDescription { get; set; }

        [CsvColumn(columnName: "MCC Group")]
        public string MccGroup { get; set; }
    }
}
