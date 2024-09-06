using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Services;

namespace CashflowBeta.ViewModels.Templates
{
    public class MainMenuItemTemplate
    {
        public MainMenuItemTemplate(Type type)
        {
            ModelType = type;
            Label = type.Name.Replace("ViewModel", "");

            ImageFromBinding = ImageHelper.LoadFromResource(new Uri("avares://CashflowBeta/Assets/" + type.Name.Replace("ViewModel", "") + ".png"));
    }
        public string Label { get; }
        public Type ModelType { get; }
        public Bitmap? ImageFromBinding { get; } 

    }
}
