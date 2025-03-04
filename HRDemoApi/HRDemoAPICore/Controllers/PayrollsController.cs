﻿using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Models;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollsController(HRDemoApiContext hRDemoAPIDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hRDemoAPIDb;

        [HttpGet]
        // GET api/payrolls
        public IEnumerable<PayrollResponse> Get(int count = default, int page = default, int employee = default, int month = default, int year = default)
        {
            return _hRDemoAPIDb.Payrolls
                .Where(p => employee == default || employee == p.EmployeeID)
                .Where(p => month == default || month == p.Month)
                .Where(p => year == default || year == p.Year)
                .OrderBy(p => p.PayrollID)
                .Paginate(count, page, HttpContext)
                .Include("Employee")
                .AsNoTracking()
                .ToList()
                .Select(p => p.MapQueryResult());
        }

        [HttpGet("{id}")]

        // GET api/payrolls/5
        public ObjectResult Get(int id)
        {
            PayrollResponse? payroll = _hRDemoAPIDb.Payrolls
                .Where(p => p.PayrollID == id)
                .Include("Employee")
                .AsNoTracking()
                .ToList()
                .Select(p => p.MapQueryResult())
                .FirstOrDefault();
            if (payroll == null || payroll.PayrollID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return payroll.CreateResponseMessage();
        }

        [HttpPost]

        // POST api/payrolls
        public ObjectResult Post([FromBody] PayrollRequest payrollRequest)
        {
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, payrollRequest.EmployeeID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Payroll newPayroll = payrollRequest.MapPostRequest();
            Payroll savedPayroll = _hRDemoAPIDb.Payrolls.Add(newPayroll).Entity;
            _hRDemoAPIDb.SaveChanges();
            return savedPayroll.CreateResponseMessage();
        }

        [HttpPut("{id}")]

        // PUT api/payrolls/5
        public ObjectResult Put(int id, [FromBody] PayrollRequest payrollRequest)
        {
            Payroll? payroll = _hRDemoAPIDb.Payrolls.Include("Employee").Where(p => p.PayrollID == id).FirstOrDefault();
            if (payroll == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, payroll.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }

            Payroll newPayroll = payrollRequest.MapPutRequest(id);
            payroll.Month = newPayroll.Month;
            payroll.Year = newPayroll.Year;
            payroll.Salary = newPayroll.Salary;
            _hRDemoAPIDb.SaveChanges();
            return payroll.CreateResponseMessage();
        }

        [HttpPatch("{id}")]

        // PATCH api/Payrolls/5
        public ObjectResult Patch(int id, [FromBody] PayrollRequest payrollRequest)
        {
            Payroll? payroll = _hRDemoAPIDb.Payrolls.Include("Employee").Where(p => p.PayrollID == id).FirstOrDefault();
            if (payroll == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, payroll.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (payrollRequest.Year != default)
            {
                payroll.Year = payrollRequest.Year;
            }
            if (payrollRequest.Month != default)
            {
                payroll.Month = payrollRequest.Month;
            }
            if (payrollRequest.Salary != null)
            {
                if (payrollRequest.Salary.GrossAmount != default)
                {
                    payroll.Salary.GrossAmount = payrollRequest.Salary.GrossAmount;
                }
                if (payrollRequest.Salary.PreTaxDeduction != default)
                {
                    payroll.Salary.PreTaxDeduction = payrollRequest.Salary.PreTaxDeduction;
                }
                if (payrollRequest.Salary.TaxDeduction != default)
                {
                    payroll.Salary.TaxDeduction = payrollRequest.Salary.TaxDeduction;
                }
                if (payrollRequest.Salary.PostTaxDeduction != default)
                {
                    payroll.Salary.PostTaxDeduction = payrollRequest.Salary.PostTaxDeduction;
                }
                payroll.Salary.NetAmount = payroll.Salary.GrossAmount - payroll.Salary.PreTaxDeduction - payroll.Salary.TaxDeduction - payroll.Salary.PostTaxDeduction;
            }
            _hRDemoAPIDb.SaveChanges();
            return payroll.CreateResponseMessage();
        }

        [HttpDelete("{id}")]

        // DELETE api/payrolls/5
        public ObjectResult Delete(int id)
        {
            Payroll? payroll = _hRDemoAPIDb.Payrolls.Include("Employee").Where(p => p.PayrollID == id).FirstOrDefault();
            if (payroll == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, payroll.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            _hRDemoAPIDb.Payrolls.Remove(payroll);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
