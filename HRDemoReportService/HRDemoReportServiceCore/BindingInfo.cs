namespace HRDemoReportServiceCore
{
    public class BindingInfo
    {
        public string Endpoint { get; set; } = "/HRDemoReportService.svc";
        public Binding Binding { get; set; } = new BasicHttpBinding(BasicHttpSecurityMode.None);
    }
}