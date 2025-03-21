namespace HRDemoReportServiceCore
{
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
