using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CashflowBeta.ViewModels;
using System.IO;
using Avalonia.Markup.Xaml;
using CashflowBeta.Converters;

namespace CashflowBeta.Views
{
    public partial class AddAccountView : Window
    {
        public AddAccountView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Resources["BoolToColorconverter"] = new BoolToColorConverter();
        }
    }
}
