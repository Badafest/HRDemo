using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class ReportResponse
{
    [DataMember]
    public EmployeeReport EmployeeReport { get; set; } = new EmployeeReport();
    [DataMember]
    public IEnumerable<PayrollReport> PayrollReports { get; set; } = new PayrollReport[] { };
}
