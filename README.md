# SimpleSalaryCalculator

## Description
This is a simple salary calculator that calculates the taxable income, taxes and superannuation for the provided package input. Depending on the selected frequency it will provide the weekly, fortnightly, monthly net income value.

## Installation

1. Clone the repository: `git clone https://github.com/bzx2022/SimpleSalaryCalculator.git`
2. Navigate to the project directory: `cd SimpleSalaryCalculator`
3. Install the required packages: `dotnet restore`
4. Build the project: `dotnet build`

## Usage
Navigate to the SimpleSalaryCalculator.App project: `cd SimpleSalaryCalculator.App`
Run the SimpleSalaryCalculator.App project: `dotnet run`

You will be prompted to enter the package details:
- Enter the annual salary
- Enter the payment frequency

The application will then calculate the taxable income, taxes and superannuation for the provided package input. Depending on the selected frequency it will provide the weekly, fortnightly, monthly net income value.

There are additional settings available in the appsettings.json. The following settings can be configured:
- TaxRates: The tax rates for each tax bracket and multiple years of tax rates can be added
- Allowing for prompt of tax year, and the tax rates for that year will be used
- Default tax year to use when not prompted

## Assumptions and notes
I noticed contradictory information in the provided instructions pertaining to calculating the superannuation from the salary package amount and then calculating the taxable income and the provided examples did not seem to calculate correctly. 
I have made the following assumptions:
- The superannuation is calculated from the salary package amount
- The taxable income is calculated from the salary package amount minus the superannuation

Due to time constraints I felt like I could not cover all edge cases in testing and error handling. I have tried to cover the most common cases and have added some error handling and input validation in the application.
I also would have liked to make the application able to dynamically load other deductible rates and superannuation rates from the appsettings.json file.

If further development was to be done on this application I would like to add the following:
- Add more tests to cover more edge cases
- Add more error handling and input validation
- Add the ability to dynamically load other deductible rates and superannuation rates from the appsettings.json file
- Add other deductibles such as FBT and HELP/HECS.

## Testing
Navigate to the SimpleSalaryCalculator.Tests project: `cd SimpleSalaryCalculator.Tests`
Run the tests: `dotnet test`

