using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CsvHelper;
using CsvHelper.Configuration;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace CashflowBeta.Services.StatementProcessing
{
    public class StatementProcessing
    {
        public static void ProcessStatementFile(string path,Account account)
        {
            //Read in Csv File
            var reader = new StreamReader(path);

            //Configure CsvHelper
            var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            var csv = new CsvReader(reader, csvconfig);

            //Load csv map for account
            csv.Context.RegisterClassMap(new CurrencyTransactionMap(account.ID));

            //Read records from csv file
            var records = csv.GetRecords<CurrencyTransaction>();

            //Convert records to transaction list
            List<CurrencyTransaction> transactions = new();
            foreach (var record in records)
            {
                transactions.Add(record);
            }

            //Get partners from transactionlist and add to db
            List<TransactionPartner> partners = TransactionPartnerService.GetDistinctPartners(transactions);
            TransactionPartnerService.AddTransactionPartners(partners);

            //Add transactions to db
            CurrencyTransactionService.AddCurrencyTransactions(transactions, account);
            NetworthService.AddNetworth(account);
            NetworthService.AddNetworth();
        }

        public static List<string> GetCsvHeaders(string path)
        {
            //Read in Csv File
            var reader = new StreamReader(path);

            //Configure CsvHelper
            var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            var csv = new CsvReader(reader, csvconfig);
            csv.Read();
            List<string> headers = new();
            foreach (var record in csv.Parser.Record)
            {
                headers.Add(record.ToString());
            };
            return headers;
        }
    }
    
}
