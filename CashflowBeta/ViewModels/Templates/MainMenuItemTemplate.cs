using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.ViewModels.Templates
{
    public class MainMenuItemTemplate
    {
        public MainMenuItemTemplate(Type type)
        {
            ModelType = type;
            Label = type.Name.Replace("ViewModel", "");
        }
        public string Label { get; }
        public Type ModelType { get; }
    }
}
