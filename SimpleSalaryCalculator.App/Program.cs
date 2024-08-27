using SimpleSalaryCalculator;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;


//get settings from app.config
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var config = builder.Build();

var settings = config.GetSection("Settings");
var taxYear = settings["TaxYear"] ?? "2021-22";
var taxRateConfig = new TaxRateConfig();
config.GetSection("TaxRates").Bind(taxRateConfig);
var txrates = config.GetSection("TaxRates");
foreach (var rate in txrates.GetChildren())
{
    var taxYearInfo = new TaxYearInfo();
    foreach (var bracket in rate.GetSection("Brackets").GetChildren())
    {
        taxYearInfo.Brackets.Add(new TaxBracket
        {
            Minimum = bracket.GetValue<int>("Minimum"),
            Threshold = bracket.GetValue<int?>("Threshold"),
            Rate = bracket.GetValue<double>("Rate")
        });
    }
    taxRateConfig.TaxRates.Add(rate.Key, taxYearInfo);
}

Console.Write("Please enter your salary package amount: ");
var salary = 0;
try
{
    salary = Convert.ToInt32(Console.ReadLine());
}
catch (FormatException)
{
    Console.WriteLine("Invalid salary amount");
    return;
}

Console.Write("Please enter your pay frequency (W for weekly, F for Fortnightly, M for Monthly): ");
var payFrequency = Console.ReadLine();

if (string.IsNullOrEmpty(payFrequency) || payFrequency.ToUpper() != "W" && payFrequency.ToUpper() != "F" && payFrequency.ToUpper() != "M")
{
    Console.WriteLine("Invalid pay frequency");
    return;
}



if (settings["EnableTaxYear"] != "false")
{
    Console.Write("Please enter the tax year (YYYY-YY): ");
    taxYear = Console.ReadLine();

    if (string.IsNullOrEmpty(taxYear) || !Regex.IsMatch(taxYear, @"^\d{4}-\d{2}$"))
    {
        Console.WriteLine("Invalid tax year format");
        return;
    }
}

var payCalculator = new PayCalculator(taxRateConfig, salary, payFrequency, taxYear);

try
{
    var calculatedPay = payCalculator.CalculatePay();

    Console.WriteLine($@"Calculating salary details...
    
    Gross Package: {calculatedPay.GrossPackage.ToString("C")}
    Superannuation: {calculatedPay.Superannuation.ToString("C2")}
    
    Taxable Income: {calculatedPay.TaxableIncome.ToString("C2")}
    
    Deductions:
    Medicare Levy: {calculatedPay.MedicareLevy.ToString("C2")}
    Budget Repair Levy: {calculatedPay.BudgetRepairLevy.ToString("C2")}
    Income Tax: {calculatedPay.IncomeTax.ToString("C2")}
    
    Net Income: {calculatedPay.NetIncome.ToString("C2")}
    Pay Packet: {calculatedPay.PayPacket.ToString("C2")} per {calculatedPay.PayFrequency}
    ");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"There was an error processing your salary, message received: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}
Console.WriteLine("Press any key to end...");
Console.ReadKey();

