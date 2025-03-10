﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.Data.SqlClient;

namespace HRDemoAPI.DataCore.Models
{
    public partial class HRDemoApiContext
    {
        private IHRDemoApiContextProcedures _procedures;

        public virtual IHRDemoApiContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new HRDemoApiContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IHRDemoApiContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class HRDemoApiContextProcedures : IHRDemoApiContextProcedures
    {
        private readonly HRDemoApiContext _context;

        public HRDemoApiContextProcedures(HRDemoApiContext context)
        {
            _context = context;
        }

        public virtual async Task<List<GetEmployeeMonthlyReportResult>> GetEmployeeMonthlyReportAsync(int? employeeId, int? year, int? month, double? offset, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "employeeId",
                    Value = employeeId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "year",
                    Value = year ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "month",
                    Value = month ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "offset",
                    Value = offset ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Float,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<GetEmployeeMonthlyReportResult>("EXEC @returnValue = [dbo].[GetEmployeeMonthlyReport] @employeeId = @employeeId, @year = @year, @month = @month, @offset = @offset", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
