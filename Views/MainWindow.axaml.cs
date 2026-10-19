using Avalonia.Controls;
using InvestmentCalculator.ViewModels;
namespace InvestmentCalculator.Views;

public partial class MainWindow : Window {
  public MainWindow() {
    InitializeComponent();
    DataContext = new MainWindowViewModel();
  }
}
