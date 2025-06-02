using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CashflowBeta.Services.StatementProcessing;

public class StatementProcessingService
{
    private readonly CurrencyTransactionService _currencyTransactionService;
    public StatementProcessingService(CurrencyTransactionService currencyTransactionService)
    {
        _currencyTransactionService = currencyTransactionService;
    }
    public async Task ProcessStatementFile(string path, Account account)
    {

        //Testfilepath: 
        // /home/nico/Documents/Statements/AT332026702001334800_2024-10-01_2024-11-09_€214_65.csv
        //Read in Csv File
        var reader = new StreamReader(path);

        //Configure CsvHelper
        var csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        var csv = new CsvReader(reader, csvconfig);
        List<CurrencyTransaction> transactions = new();
        //Load csv map for account
        csv.Context.RegisterClassMap(new CurrencyTransactionMap(account.ID));
        try
        {
            //Read records from csv file
            var records = csv.GetRecords<CurrencyTransaction>();
            
            foreach (var record in records) transactions.Add(record);

            //Get partners from transactionlist and add to db
            var partners = TransactionPartnerService.GetDistinctPartners(transactions);
            TransactionPartnerService.AddTransactionPartners(partners);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        //Add transactions to db
        await _currencyTransactionService.AddCurrencyTransactions(transactions, account);
    }

    public static List<string> GetCsvHeaders(string? path)
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
        foreach (var record in csv.Parser.Record) headers.Add(record);
        ;
        return headers;
    }
}