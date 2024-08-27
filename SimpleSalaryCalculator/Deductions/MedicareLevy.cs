using SimpleSalaryCalculator.Interfaces;

namespace SimpleSalaryCalculator.Deductions
{
    public class MedicareLevy : IDeductions
    {
        public int CalculateDeduction(int salary)
        {
            double deduction = salary switch
            {
                _ when salary <= 21335 => 0,
                _ when salary <= 26668 => (salary - 21335) * 0.10,
                _ => salary * 0.02,
            };

            return (int)Math.Ceiling(deduction);
        }
    }
}
