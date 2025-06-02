using System.Linq;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class AppStatusService
    {
        public bool DataInitialized;

        private readonly AppDataStore _appDataStore;
        private readonly AccountService _accountService;
        private readonly CurrencyTransactionService _currencyTransactionService;
        private readonly BudgetService _budgetService;
        private readonly NetworthService _networthService;
        private readonly TransactionPartnerService _transactionPartnerService;
        public AppStatusService(
            AppDataStore appDataStore,
            AccountService accountService,
            CurrencyTransactionService currencyTransactionService,
            BudgetService budgetService,
            NetworthService networthService,
            TransactionPartnerService transactionPartnerService)
        {
            _appDataStore = appDataStore;
            _accountService = accountService;
            _currencyTransactionService = currencyTransactionService;
            _budgetService = budgetService;
            _networthService = networthService;
            _transactionPartnerService = transactionPartnerService;

            LoadAllData();
        }
        private async Task LoadAllData()
        {
            _appDataStore.ClearAll();
            //Load Account data
            var accounts = await _accountService.GetAllAsync();
            if (accounts.Count > 0)
                foreach (var a in accounts)
                    _appDataStore.Accounts.Add(a);
            //If data present load transactions
            if (_appDataStore.Accounts.Count > 0)
            {
                //Load Currency Transaction Data
                var currencyTransactions = await _currencyTransactionService.GetAllAsync();
                if (currencyTransactions.Count > 0)
                    foreach (var ct in currencyTransactions)
                        _appDataStore.CurrencyTransactions.Add(ct);
                //If currency transaction present load respective additional data
                if (_appDataStore.CurrencyTransactions.Count > 0)
                {
                    //Load transaction partners
                    var transactionPartners = await _transactionPartnerService.GetAllAsync();
                    if (transactionPartners.Count > 0)
                        foreach (var tp in transactionPartners)
                            _appDataStore.TransactionPartners.Add(tp);
                    //Load Budgets
                    var budgets = await _budgetService.GetAllAsync();
                    if (budgets.Count > 0)
                        foreach (var bdgt in budgets)
                            _appDataStore.Budgets.Add(bdgt);
                }
            }
            DataInitialized = true;
        }
    }
}