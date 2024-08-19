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

        //Map to create or load a map for a currency transaction csv statement for specific account
        public static CurrencyTransactionCsvMap LoadMapForAccount(int accId)
        {
            string path = GenerateFilePath(accId);
            CurrencyTransactionCsvMap map = new();
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                map = JsonSerializer.Deserialize<CurrencyTransactionCsvMap>(jsonString) ?? new CurrencyTransactionCsvMap();
            }
            return map;
        }
        //Map to create or save a map for a currency transaction csv statement for specific account
        public static void SaveMapForAccount(int accId, CurrencyTransactionCsvMap map)
        {
            string path = GenerateFilePath(accId);
            string jsonString = JsonSerializer.Serialize(map);
            GenerateFilePath(accId);
            if (File.Exists(path))
            {
                File.WriteAllText(path, jsonString);
            }
            else 
            {
                File.WriteAllText(path, jsonString);
            }
        }
        //Method to generate the filepath for the selected account currency transaction csv statement map
        private static string GenerateFilePath(int accId)
        {
            string path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(),
                                       "CashFlow",
                                       "Maps",
                                       $"account{accId.ToString()}map.json");
            return path;
        }
    }
}
