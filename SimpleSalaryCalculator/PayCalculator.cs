using SimpleSalaryCalculator.Deductions;
using SimpleSalaryCalculator.Models;
using SimpleSalaryCalculator.SuperContribution;

namespace SimpleSalaryCalculator
{
    public class PayCalculator
    {
        private readonly BudgetRepairLevy _budgetRepairLevy;
        private readonly IncomeTax _incomeTax;
        private readonly MedicareLevy _medicareLevy;
        private readonly Superannuation _superannuation;
        private readonly TaxRateConfig _taxRateConfig;
        private int _salary;
        private string _payFrequency;


        public PayCalculator(TaxRateConfig taxRateConfig, int salary, string payFrequency, string? taxYear)
        {
            _incomeTax = new IncomeTax(taxRateConfig.TaxRates[taxYear ?? "2022-23"]);
            _medicareLevy = new MedicareLevy();
            _superannuation = new Superannuation();
            _budgetRepairLevy = new BudgetRepairLevy();
            _salary = salary;
            _payFrequency = payFrequency;
        }

        public SalaryDetails CalculatePay()
        {
            if (_salary <= 0)
                throw new ArgumentException("Invalid salary amount");

            if (_payFrequency.ToUpper() != "W" && _payFrequency.ToUpper() != "F" && _payFrequency.ToUpper() != "M" || string.IsNullOrEmpty(_payFrequency))
                throw new ArgumentException("Invalid pay frequency");

            var salaryDetails = new SalaryDetails();
            var payAmount = 0.0;

            salaryDetails.GrossPackage = _salary;
            salaryDetails.Superannuation = _superannuation.CalculateSuperContribution(_salary);
            salaryDetails.TaxableIncome = _salary - salaryDetails.Superannuation;
            salaryDetails.IncomeTax = _incomeTax.CalculateDeduction((int)salaryDetails.TaxableIncome);
            salaryDetails.MedicareLevy = _medicareLevy.CalculateDeduction((int)salaryDetails.TaxableIncome);
            salaryDetails.BudgetRepairLevy = _budgetRepairLevy.CalculateDeduction((int)salaryDetails.TaxableIncome);
            salaryDetails.NetIncome = salaryDetails.TaxableIncome - salaryDetails.IncomeTax - salaryDetails.MedicareLevy - salaryDetails.BudgetRepairLevy;
            
            switch (_payFrequency.ToUpper())
            {
                case "W":
                    payAmount = salaryDetails.NetIncome / 52;
                    break;
                case "F":
                    payAmount = salaryDetails.NetIncome / 26;
                    break;
                case "M":
                    payAmount = salaryDetails.NetIncome / 12;
                    break;
                default:
                    throw new ArgumentException("Invalid pay frequency");
            }

            salaryDetails.PayPacket = payAmount;
            salaryDetails.PayFrequency = GetPayFrequencyDescription(_payFrequency);
            return salaryDetails;
        }

        string GetPayFrequencyDescription(string payFrequency)
        {
            return payFrequency.ToUpper() switch
            {
                "W" => "week",
                "F" => "fortnight",
                "M" => "month",
                _ => "unknown frequency"
            };
        }
    }
}
