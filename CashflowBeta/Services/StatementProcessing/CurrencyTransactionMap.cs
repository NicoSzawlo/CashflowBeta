using CashflowBeta.Models;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CashflowBeta.Services.StatementProcessing;

public class CurrencyTransactionMap : ClassMap<CurrencyTransaction>
{
    public CurrencyTransactionMap(int accId)
    {
        //Load map for this account
        var map = FileService.LoadMapForAccount(accId);
        //Mapping transaction details
        Map(m => m.DateTime).Name(map.DateTimeHeader);
        Map(m => m.DateTime).TypeConverter<DateTimeConverter>()
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