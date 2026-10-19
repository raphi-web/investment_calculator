namespace InvestmentCalculator.ViewModels;

using ReactiveUI;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using InvestmentCalculator.Models;
using System.Collections.ObjectModel;
using System.Linq;

public partial class MainWindowViewModel : ViewModelBase {
  private string _startBalance = "0";
  private string _monthlyAmount = "300";
  private string _annualInterestRate = "6";
  private string _years = "20";

  public string StartBalance {
    get => _startBalance;
    set {
      this.RaiseAndSetIfChanged(ref _startBalance, value);
      Calculate();
    }
  }

  public string MonthlyAmount {
    get => _monthlyAmount;
    set {
      this.RaiseAndSetIfChanged(ref _monthlyAmount, value);
      Calculate();
    }
  }

  public string AnnualInterestRate {
    get => _annualInterestRate;
    set {
      this.RaiseAndSetIfChanged(ref _annualInterestRate, value);
      Calculate();
    }
  }

  public string Years {
    get => _years;
    set {
      this.RaiseAndSetIfChanged(ref _years, value);
      Calculate();
    }
  }

  public ObservableCollection<YearlyResult> YearlyData { get; } = new();

  private void Calculate() {
    if (!double.TryParse(StartBalance, out double startBalance) ||
        !double.TryParse(MonthlyAmount, out double amount) ||
        !double.TryParse(AnnualInterestRate, out double annualRate) ||
        !int.TryParse(Years, out int years)) {
      return;
    }

    double monthlyRate = (annualRate / 100.0) / 12.0;
    double balance = startBalance;

    YearlyData.Clear();

    for (int year = 1; year <= years; year++) {
      for (int month = 0; month < 12; month++) {
        balance += balance * monthlyRate + amount;
      }
      double invested = year * 12 * amount + startBalance;
      double profit = balance - invested;

      YearlyData.Add(new YearlyResult { Year = year, Balance = balance,
                                        Invested = invested });
    }

    Series = new List<ISeries> {
      new LineSeries<double> {
        Values = YearlyData.Select(y => y.Balance).ToArray(),
        Stroke = new SolidColorPaint(SKColors.Green, 3),
        Fill = new SolidColorPaint(SKColors.Green.WithAlpha(50)),
        GeometrySize = 8,
        GeometryStroke = new SolidColorPaint(SKColors.Green),
        GeometryFill = new SolidColorPaint(SKColors.Green),
        Name = "Balance",
        YToolTipLabelFormatter = point => point.Model.ToString("N2") + " €",

      },

      new LineSeries<double> {
        Values = YearlyData.Select(y => y.Invested).ToArray(),
        Stroke = new SolidColorPaint(SKColors.Red, 3),
        Fill = new SolidColorPaint(SKColors.Red.WithAlpha(50)),
        GeometrySize = 8,
        GeometryStroke = new SolidColorPaint(SKColors.Red),
        GeometryFill = new SolidColorPaint(SKColors.Red),
        Name = "Invested",
        YToolTipLabelFormatter = point => point.Model.ToString("N2") + " €",

      },

      new LineSeries<double> {
        Values = YearlyData.Select(y => y.Profit).ToArray(),
        Stroke = new SolidColorPaint(SKColors.Blue, 3),
        Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(50)),
        GeometrySize = 8,
        GeometryStroke = new SolidColorPaint(SKColors.Blue),
        GeometryFill = new SolidColorPaint(SKColors.Blue),
        Name = "Profit",
        YToolTipLabelFormatter = point => point.Model.ToString("N2") + " €",

      }
    };

    this.RaisePropertyChanged(nameof(YearlyData));
    this.RaisePropertyChanged(nameof(Series));
  }

  public IList<ISeries> Series { get; set; }
  public MainWindowViewModel() { Calculate(); }
}
