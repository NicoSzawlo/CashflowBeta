using CashflowBeta.Models;
using Google.Protobuf.Reflection;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace CashflowBeta.Services.StatementProcessing
{
    public class CurrencyTransactionCsvMap
    {
        public string DateTimeHeader { get; set; }
        public string DateTimeHeaderFormat { get; set; }
        public string AmountHeader { get; set; }
        public string CurrencyHeader { get; set; }
        public string InfoHeader { get; set; }
        public string ReferenceHeader { get; set; }
        public string PartnerNameHeader { get; set; }
        public string PartnerAccountIdendifierHeader { get; set; }
        public string PartnerBankIdentifierHeader { get; set; }
        public string PartnerBankCodeHeader { get; set; }

        // Constructor with initial values
        public CurrencyTransactionCsvMap()
        {
            DateTimeHeader = "Transaction Date";
            DateTimeHeaderFormat = "yyyy-MM-dd HH:mm:ss"; // Example format
            AmountHeader = "Amount";
            CurrencyHeader = "Currency";
            InfoHeader = "Transaction Info";
            ReferenceHeader = "Reference";
            PartnerNameHeader = "Partner Name";
            PartnerAccountIdendifierHeader = "Partner Account ID";
            PartnerBankIdentifierHeader = "Partner Bank ID";
            PartnerBankCodeHeader = "Partner Bank Code";
        }
    }
}
