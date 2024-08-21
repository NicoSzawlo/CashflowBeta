using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CashflowBeta.Services.FileMapping;
using CsvHelper;
using CsvHelper.Configuration;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace CashflowBeta.Services.StatementProcessing
{
    public class CsvProcessing
    {
        public static void ProcessStatementFile(int accId)
        {
            string path = "C:\\Privat\\ftxcsv\\AT332026702001334800_2022-01-01_2023-01-01.csv";
            var reader = new StreamReader(path);
            var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            var csv = new CsvReader(reader, csvconfig);
            csv.Context.RegisterClassMap(new CurrencyTransactionMap(accId));
            var records = csv.GetRecords<CurrencyTransaction>();
            List<CurrencyTransaction> transactions = new List<CurrencyTransaction>();

            foreach (var record in records)
            {
                transactions.Add(record);
            }
            List<TransactionPartner> partners = TransactionPartnerService.GetDistinctPartners(transactions);
        }
    }
}
