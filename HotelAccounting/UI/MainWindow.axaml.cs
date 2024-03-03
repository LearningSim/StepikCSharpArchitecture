using System;
using Avalonia.Controls;

namespace HotelAccounting.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        var accountingModel = new AccountingModel();
        accountingModel.PropertyChanged += (_, args) =>
        {
            var val = typeof(AccountingModel).GetProperty(args.PropertyName!)?
                .GetValue(accountingModel, null);
            Console.WriteLine($"{args.PropertyName} {val}");
        };
        accountingModel.Price = 1000;
        accountingModel.NightsCount = 3;
        accountingModel.Discount = 10;
        DataContext = accountingModel;
        InitializeComponent();
    }
}