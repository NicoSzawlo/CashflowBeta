using CashflowBeta.Models;
using CashflowBeta.Services.StatementProcessing;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CashflowBeta.Services.FileMapping
{
    public class CurrencyTransactionMap : ClassMap<CurrencyTransaction>
    {
        public CurrencyTransactionMap(int accId)
        {
            //Load map for this account
            CurrencyTransactionCsvMap map = CurrencyTransactionCsvMap.LoadMapForAccount(accId);
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

//Mapping transaction details
//Map(m => m.DateTime).Name("Booking Date");
//Map(m => m.DateTime).TypeConverter<CsvHelper
//    .TypeConversion.DateTimeConverter>()
//    .TypeConverterOption.Format("dd.MM.yyyy");
//Map(m => m.Amount).Name("Amount");
//Map(m => m.Currency).Name("Currency");
//Map(m => m.Info).Name("Booking details");
//Map(m => m.Reference).Name("Booking Reference");
////Mapping partner details
//Map(m => m.TransactionPartner.Name).Name("Partner Name");
//Map(m => m.TransactionPartner.AccountIdentifier).Name("Partner IBAN");
//Map(m => m.TransactionPartner.BankIdentifier).Name("BIC/SWIFT");
//Map(m => m.TransactionPartner.Bankcode).Name("Bank code");
