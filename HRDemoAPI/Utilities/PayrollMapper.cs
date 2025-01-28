﻿using HRDemoAPI.Data;
using HRDemoApp.Models;

namespace HRDemoApp.Utilities
{
    public static class PayrollMapper
    {
        public static PayrollResponse MapQueryResult(this Payroll payroll)
        {
            return new PayrollResponse
            {
               PayrollID = payroll.PayrollID,
               Month = payroll.Month,
               Year = payroll.Year,
               Status = payroll.Status,
               Salary = payroll.Salary == null ? null : new PayrollResponseSalary
               {
                   GrossAmount = payroll.Salary.GrossAmount ?? 0,
                   PreTaxDeduction = payroll.Salary.PreTaxDeduction ?? 0,
                   TaxDeduction = payroll.Salary.TaxDeduction ?? 0,
                   PostTaxDeduction = payroll.Salary.PostTaxDeduction ?? 0,
                   NetAmount = payroll.Salary.NetAmount ?? 0
               },
               Employee = payroll.Employee?.MapQueryResult()
            };
        }
        public static Payroll MapPostRequest(this PayrollRequest payrollRequest)
        {
            var payroll = MapRequest(payrollRequest);
            payroll.Status = PayrollStatus.Pending;
            return payroll;
        }

        public static Payroll MapPutRequest(this PayrollRequest payrollRequest, int payrollId)
        {
            Payroll payroll = MapRequest(payrollRequest);
            payroll.PayrollID = payrollId;
            return payroll;
        }

        private static Payroll MapRequest(PayrollRequest payrollRequest)
        {
            return new Payroll()
            {
                Month = payrollRequest.Month,
                Year = payrollRequest.Year,
                Salary = payrollRequest.Salary == null ? null : new EmployeeSalary
                {
                    GrossAmount = payrollRequest.Salary.GrossAmount,
                    PreTaxDeduction = payrollRequest.Salary.PreTaxDeduction,
                    TaxDeduction = payrollRequest.Salary.TaxDeduction,
                    PostTaxDeduction = payrollRequest.Salary.PostTaxDeduction,
                    NetAmount = payrollRequest.Salary.GrossAmount - payrollRequest.Salary.PreTaxDeduction - payrollRequest.Salary.TaxDeduction - payrollRequest.Salary.PostTaxDeduction
                },
                EmployeeID = payrollRequest.EmployeeID,
            };
        }
    }
}