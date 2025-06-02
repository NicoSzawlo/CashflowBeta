using System.Collections.ObjectModel;
using CashflowBeta.Models;
using CashflowBeta.Services;

namespace CashflowBeta.ViewModels;

public class PartnersViewModel : ViewModelBase
{
    private readonly AppDataStore _appDataStore;

    public PartnersViewModel(AppDataStore appDataStore)
    {
        _appDataStore = appDataStore;
        TransactionPartners = _appDataStore.TransactionPartners;
    }

    public ObservableCollection<TransactionPartner> TransactionPartners { get; set; }
}