using CashflowBeta.Services;
using CashflowBeta.ViewModels;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        //Global app data storage
        collection.AddSingleton<AppDataStore>();
        collection.AddSingleton<AppStatusService>();
        //DbContext
        collection.AddDbContext<CashflowContext>();
        //Add services
        collection.AddScoped<AccountService>();
        collection.AddScoped<CurrencyTransactionService>();
        collection.AddScoped<BudgetService>();
        collection.AddScoped<NetworthService>();
        collection.AddScoped<TransactionPartnerService>();
        //Add Viewmodels
        collection.AddTransient<MainWindowViewModel>();
        collection.AddTransient<TransactionsViewModel>();
        collection.AddTransient<AccountViewModel>();
        collection.AddTransient<BudgetsViewModel>();
        collection.AddTransient<HomeViewModel>();
        collection.AddTransient<PartnersViewModel>();
        collection.AddTransient<AddAccountViewModel>();
        collection.AddTransient<StatementMapViewModel>();
    }
}