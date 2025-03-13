using CrystalDecisions.CrystalReports.Engine;
using HRDemoReportUI.Service.HRDemoReportService;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace HRDemoReportUI.PDF
{
    public static class PDFReportGenerator
    {
        public static Stream Generate(ReportResponse reportResponse, short selectedYear, string selectedMonth)
        {
            var employeeReport = reportResponse.EmployeeReport;
            var payrollReports = reportResponse.PayrollReports;

            // Initialize Report Document
            ReportDocument report = new MonthlyPayrollReport();

            // Set Data Source as Payroll Reports
            DataTable dataTable = new DataTable();
            string[] columnNames = new string[] { "GrossAmount", "PreTaxDeduction", "TaxDeduction", "PostTaxDeduction", "NetAmount" };

            foreach (var columnName in columnNames)
            {
                dataTable.Columns.Add(new DataColumn(columnName, typeof(string)));
            }

            var totals = new double[] { 0, 0, 0, 0, 0 };
            foreach (var payrollReport in payrollReports)
            {
                var payrollDictionary = ConvertClassToDictionary(payrollReport);
                DataRow row = dataTable.NewRow();
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string key = columnNames[i];
                    bool parsed = double.TryParse(payrollDictionary[key], out double parsedValue);
                    double value = parsed ? parsedValue : 0.0;
                    row[key] = value.ToString("C", new System.Globalization.CultureInfo("en-US"));
                    totals[i] += value;
                }
                dataTable.Rows.Add(row);
            }

            report.SetDataSource(dataTable);

            // Set Parameter Values
            for (int i = 0; i < columnNames.Length; i++)
            {
                var name = "Total" + columnNames[i];
                var value = totals[i].ToString("C", new System.Globalization.CultureInfo("en-US"));
                report.SetParameterValue(name, value);
            }

            Dictionary<string, string> employeeReportDictionary = ConvertClassToDictionary(employeeReport);
            var parameterNames = new string[] {
                "PresentDays", "LateDays", "LeaveDays", "AbsentDays",
                "SickLeave", "CasualLeave", "ParentalLeave", "BereavementLeave", "UnpaidLeave",
                "WorkingDaysInMonth", "WorkingDaysInYear", "AnnualSalary", "MonthSalary",
                "FirstName", "LastName", "State", "Country"
            };

            foreach (var key in parameterNames)
            {
                report.SetParameterValue(key, employeeReportDictionary[key]);
            }

            report.SetParameterValue("Year", selectedYear);
            report.SetParameterValue("Month", selectedMonth);


            //report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, $"{employeeReport.FirstName}_{employeeReport.LastName}_{selectedYear}_{selectedMonth}.pdf");
            return report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        }
        private static Dictionary<string, string> ConvertClassToDictionary(object obj)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var property in obj.GetType().GetProperties())
            {
                dictionary.Add(property.Name, property.GetValue(obj).ToString());
            }
            return dictionary;
        }
    }
}
