using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //public int CalculateDeduction(int salary)
        //{
        //    double deduction = salary switch
        //    {
        //        _ when salary <= 18200 => 0,
        //        _ when salary <= 37000 => (salary - 18200) * 0.19,
        //        _ when salary <= 80000 => 3572 + (salary - 37000) * 0.325,
        //        _ when salary <= 180000 => 17547 + (salary - 80000) * 0.37,
        //        _ => 54547 + (salary - 180000) * 0.45,
        //    };

        //    return (int)Math.Ceiling(deduction);
        //}
    }
}
