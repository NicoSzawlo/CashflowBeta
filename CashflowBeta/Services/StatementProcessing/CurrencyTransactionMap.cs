using CashflowBeta.Models;
using CashflowBeta.Services.StatementProcessing;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CashflowBeta.Services.StatementProcessing
{
    public class CurrencyTransactionMap : ClassMap<CurrencyTransaction>
    {
        public CurrencyTransactionMap(int accId)
        {
            //Load map for this account
            CurrencyTransactionCsvMap map = FileService.LoadMapForAccount(accId);
            //Mapping transaction details
            Map(m => m.DateTime).Name(map.DateTimeHeader);
            Map(m => m.DateTime).TypeConverter<CsvHelper
                .TypeConversion.DateTimeConverter>()
                .TypeConverterOption.Format(map.DateTimeHeaderFormat);
            Map(m => m.Amount).Name(map.AmountHeader);
            Map(m => m.Currency).Name(map.CurrencyHeader);
            Map(m => m.Reference).Name(map.InfoHeader);
            //Mapping partner details
            Map(m => m.TransactionPartner.Name).Name(map.PartnerNameHeader);
            Map(m => m.TransactionPartner.AccountIdentifier).Name(map.PartnerAccountIdendifierHeader);
            Map(m => m.TransactionPartner.BankIdentifier).Name(map.PartnerBankIdentifierHeader);
            Map(m => m.TransactionPartner.Bankcode).Name(map.PartnerBankCodeHeader);
        }
    }
}