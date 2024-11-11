using System;
using Avalonia.Media.Imaging;
using CashflowBeta.Services;

namespace CashflowBeta.ViewModels.Templates;

public class MainMenuItemTemplate
{
    public MainMenuItemTemplate(Type type)
    {
        ModelType = type;
        Label = type.Name.Replace("ViewModel", "");

        ImageFromBinding =
            ImageHelper.LoadFromResource(new Uri("avares://CashflowBeta/Assets/" + type.Name.Replace("ViewModel", "") +
                                                 ".png"));
    }

    public string Label { get; }
    public Type ModelType { get; }
    public Bitmap? ImageFromBinding { get; }
}