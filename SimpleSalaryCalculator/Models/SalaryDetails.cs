namespace SimpleSalaryCalculator.Models
{
    public class SalaryDetails
    {
        public int GrossPackage { get; set; }
        public double Superannuation { get; set; }
        public double TaxableIncome { get; set; }
        public int MedicareLevy { get; set; }
        public int BudgetRepairLevy { get; set; }
        public int IncomeTax { get; set; }
        public double NetIncome { get; set; }
        public double PayPacket { get; set; }
        public string PayFrequency { get; set; }
    }
}
