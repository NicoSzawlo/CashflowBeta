using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CashflowBeta.Converters;

namespace CashflowBeta.Views;

public partial class AddAccountView : Window
{
    public AddAccountView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        Resources["BoolToColorconverter"] = new BoolToColorConverter();
    }
}