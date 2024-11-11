namespace CashflowBeta.Services.StatementProcessing;

public class CurrencyTransactionCsvMap
{
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
}