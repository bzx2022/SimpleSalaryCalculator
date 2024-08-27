namespace SimpleSalaryCalculator
{
    public class TaxRateConfig
    {
        public Dictionary<string, TaxYearInfo> TaxRates { get; set; }

        public TaxRateConfig()
        {
            TaxRates = new Dictionary<string, TaxYearInfo>();
        }
    }

    public class TaxYearInfo
    {
        public List<TaxBracket> Brackets { get; set; }

        public TaxYearInfo()
        {
            Brackets = new List<TaxBracket>();
        }
    }

    public class TaxBracket
    {
        public int Minimum { get; set; }
        public int? Threshold { get; set; }
        public double Rate { get; set; }
    }
}
