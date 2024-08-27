using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSalaryCalculator.Interfaces
{
    public interface IDeductions
    {
        int CalculateDeduction(int salary);
    }
}
