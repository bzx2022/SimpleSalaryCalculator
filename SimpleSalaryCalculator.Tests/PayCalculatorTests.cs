namespace SimpleSalaryCalculator.Tests
{
    [TestFixture]
    public class PayCalculatorTests
    {
        private TaxRateConfig _taxRateConfig;

        [SetUp]
        public void SetUp()
        {
            //setup taxrateconfig
            _taxRateConfig = new TaxRateConfig();
            _taxRateConfig.TaxRates = new Dictionary<string, TaxYearInfo>
            {
                {
                    "2022-23", new TaxYearInfo
                    {
                        Brackets = new List<TaxBracket>
                        {
                            new TaxBracket {Minimum = 0, Threshold = 18200, Rate = 0},
                            new TaxBracket {Minimum = 18201, Threshold = 37000, Rate = 0.19},
                            new TaxBracket {Minimum = 37001, Threshold = 87000, Rate = 0.325},
                            new TaxBracket {Minimum = 87001, Threshold = 180000, Rate = 0.37},
                            new TaxBracket {Minimum = 180001, Threshold = null, Rate = 0.45}
                        }
                    }
                }
            };
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectPostTaxIncome()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.NetIncome, Is.EqualTo(67573.0));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectSuperannuation()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.Superannuation, Is.EqualTo(9500.0));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectGrossPackage()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.GrossPackage, Is.EqualTo(100000.0));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectIncomeTax()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.IncomeTax, Is.EqualTo(21117));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectMedicareLevy()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.MedicareLevy, Is.EqualTo(1810));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectBudgetRepairLevy()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.BudgetRepairLevy, Is.EqualTo(0));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectPayPacket()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.PayPacket.ToString("F2"), Is.EqualTo("1299.48"));
        }

        [Test]
        public void CalculatePay_WhenSalaryIs100000_ReturnsCorrectPayFrequency()
        {
            // Arrange
            var salary = 100000;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Act
            var result = payCalculator.CalculatePay();

            // Assert
            Assert.That(result.PayFrequency, Is.EqualTo("week"));
        }

        [Test]
        public void CalculatePay_WhenSalaryIsLessThanOrEqualTo0_ThrowsArgumentException()
        {
            // Arrange
            var salary = 0;
            var frequency = "w";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Assert
            Assert.Throws<ArgumentException>(() => payCalculator.CalculatePay());
        }

        [Test]
        public void CalculatePay_WhenPayFrequencyIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            var salary = 100000;
            var frequency = "x";
            var payCalculator = new PayCalculator(_taxRateConfig, salary, frequency, "2022-23");


            // Assert
            Assert.Throws<ArgumentException>(() => payCalculator.CalculatePay());
        }
    }

}