namespace InvestmentCalculator.Models;
public class YearlyResult {

  public int Year { get; set; }
  public double Balance { get; set; }
  public double Invested { get; set; }
  public double Profit => Balance - Invested;
}
