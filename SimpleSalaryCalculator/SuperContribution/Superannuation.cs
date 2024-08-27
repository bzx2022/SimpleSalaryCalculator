using SimpleSalaryCalculator.Interfaces;

namespace SimpleSalaryCalculator.SuperContribution
{
    public class Superannuation
    {
        public double CalculateSuperContribution(double salary)
        {
            return salary * 0.095;
        }
    }
}
