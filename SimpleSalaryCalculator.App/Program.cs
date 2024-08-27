using SimpleSalaryCalculator;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using SimpleSalaryCalculator.Models;

class Program
{
    static void Main(string[] args)
    {
        var config = LoadConfiguration();
        var settings = config.GetSection("Settings");
        var taxYear = GetTaxYear(settings);
        var taxRateConfig = ConfigureTaxRates(config);

        var salary = GetSalary();
        if (salary == null) return;

        var payFrequency = GetPayFrequency();
        if (payFrequency == null) return;

        var payCalculator = new PayCalculator(taxRateConfig, salary.Value, payFrequency, taxYear);

        try
        {
            DisplaySalaryDetails(payCalculator.CalculatePay());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Press any key to end...");
        Console.ReadKey();
    }

    private static void DisplaySalaryDetails(SalaryDetails salaryDetails)
    {
        Console.WriteLine($@"Calculating salary details...
    
        Gross Package: {salaryDetails.GrossPackage.ToString("C")}
        Superannuation: {salaryDetails.Superannuation.ToString("C2")}
    
        Taxable Income: {salaryDetails.TaxableIncome.ToString("C2")}
    
        Deductions:
        Medicare Levy: {salaryDetails.MedicareLevy.ToString("C2")}
        Budget Repair Levy: {salaryDetails.BudgetRepairLevy.ToString("C2")}
        Income Tax: {salaryDetails.IncomeTax.ToString("C2")}
    
        Net Income: {   salaryDetails.NetIncome.ToString("C2")}
        Pay Packet: {salaryDetails.PayPacket.ToString("C2")} per {salaryDetails.PayFrequency}
        ");
    }

    private static string GetPayFrequency()
    {
        Console.Write("Please enter your pay frequency (W for weekly, F for Fortnightly, M for Monthly): ");
        var payFrequency = Console.ReadLine();

        if (string.IsNullOrEmpty(payFrequency) || payFrequency.ToUpper() != "W" && payFrequency.ToUpper() != "F" && payFrequency.ToUpper() != "M")
        {
            Console.WriteLine("Invalid pay frequency");
            return "";
        }

        return payFrequency;
    }

    private static int? GetSalary()
    {
        Console.Write("Please enter your salary package amount: ");
        try
        {
            return Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid salary entered.");
            return null;
        }
    }

    private static TaxRateConfig ConfigureTaxRates(IConfigurationRoot config)
    {
        var taxRateConfig = new TaxRateConfig();

        var taxRates = config.GetSection("TaxRates");
        foreach (var rate in taxRates.GetChildren())
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

        return taxRateConfig;
    }

    private static string? GetTaxYear(IConfigurationSection settings)
    {
        var taxYear = settings["TaxYear"] ?? "2022-23";

        if (settings["EnableTaxYear"] != "false")
        {
            Console.Write("Please enter the tax year (YYYY-YY): ");
            taxYear = Console.ReadLine();

            if (string.IsNullOrEmpty(taxYear) || !Regex.IsMatch(taxYear, @"^\d{4}-\d{2}$"))
            {
                Console.WriteLine("Invalid tax year format");
                return null;
            }

            //taxYear should also be validated against the available tax rates in appSettings.json
        }

        return taxYear;
    }

    static IConfigurationRoot LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        return builder.Build();
    }
}