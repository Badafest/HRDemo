﻿using System.ServiceModel;

namespace HRDemoReportService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IHRDemoReportService
	{
		[OperationContract]
		ReportResponse GetEmployeeMonthlyReport(ReportRequest request);
		[OperationContract]
		UserResponse GetUserDetails(UserRequest request);
		[OperationContract]
		EmployeeResponse[] GetEmployeeOptions(EmployeeRequest request);
    }
}