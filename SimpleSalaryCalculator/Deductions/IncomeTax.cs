using SimpleSalaryCalculator.Interfaces;

namespace SimpleSalaryCalculator.Deductions
{
    public class IncomeTax : IDeductions
    {
        private readonly TaxYearInfo _taxYearInfo;

        public IncomeTax(TaxYearInfo taxYearInfo)
        {
            _taxYearInfo = taxYearInfo;
        }

        public int CalculateDeduction(int salary)
        {
            var brackets = _taxYearInfo.Brackets.OrderBy(bracket => bracket.Minimum).ToList();
            double deduction = 0;

            foreach (var bracket in brackets)
            {
                if (salary > bracket.Minimum)
                {
                    // Determine the upper limit for this bracket
                    int upperLimit = bracket.Threshold.HasValue ? Math.Min(salary, bracket.Threshold.Value) : salary;
                    // Calculate the taxable amount within this bracket
                    int taxableAmount = upperLimit - bracket.Minimum;
                    if (taxableAmount > 0)
                    {
                        deduction += taxableAmount * bracket.Rate;
                    }
                }
                else
                {
                    break; // No need to process further if the salary is below the current bracket's minimum
                }
            }

            return (int)Math.Ceiling(deduction);
        }
    }
}
