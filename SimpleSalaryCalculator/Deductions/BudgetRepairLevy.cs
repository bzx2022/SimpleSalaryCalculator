using SimpleSalaryCalculator.Interfaces;

namespace SimpleSalaryCalculator.Deductions
{
    public class BudgetRepairLevy : IDeductions
    {
        public int CalculateDeduction(int salary)
        {
            double deduction = salary switch
            {
                _ when salary <= 180000 => 0,
                _ => (salary - 180000) * 0.02,
            };

            return (int)Math.Ceiling(deduction);
        }
    }
}
