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

        //Map to create or load a map for a currency transaction csv statement for specific account
        public CurrencyTransactionCsvMap LoadMapForAccount(int accId)
        {
            string jsonString = File.ReadAllText(GenerateFilePath(accId));
            CurrencyTransactionCsvMap map = JsonSerializer.Deserialize<CurrencyTransactionCsvMap>(jsonString);
            return map;
        }
        //Map to create or save a map for a currency transaction csv statement for specific account
        public void SaveMapForAccount(int accId, string[] strings)
        {
            CurrencyTransactionCsvMap map = new()
            {
                DateTimeHeader = strings[0],
                DateTimeHeaderFormat = strings[1],
                AmountHeader = strings[2],
                CurrencyHeader = strings[3],
                InfoHeader = strings[4],
                ReferenceHeader = strings[5],
                PartnerNameHeader = strings[6],
                PartnerAccountIdendifierHeader = strings[7],
                PartnerBankIdentifierHeader = strings[8],
                PartnerBankCodeHeader = strings[9]
            };
            string jsonString = JsonSerializer.Serialize(map);
            GenerateFilePath(accId);
            File.WriteAllText(GenerateFilePath(accId), jsonString);
        }
        //Method to generate the filepath for the selected account currency transaction csv statement map
        private string GenerateFilePath(int accId)
        {
            string path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(),
                                       "CashFlow",
                                       "Maps",
                                       $"account{accId.ToString()}map.json)");
            return path;
        }
    }
}
