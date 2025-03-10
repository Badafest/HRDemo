using HRDemoReportUI.Service.HRDemoReportService;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace HRDemoReportUI.Service
{
    public class ReportService
    {
        private readonly string apiKey = null;
        private readonly string apiUrl = null;

        public ReportService(string serviceApiUrl, string serviceApiKey)
        {
            apiKey = serviceApiKey;
            apiUrl = serviceApiUrl;
        }

        public ReportResponse GetReportData(ReportRequest request)
        {
            var (scope, client) = GetServiceClient();
            var response = client.GetEmployeeMonthlyReport(request);
            scope.Dispose();
            return response;
        }

        public UserResponse GetUserData(UserRequest request)
        {
            var (scope, client) = GetServiceClient();
            var response = client.GetUserDetails(request);
            scope.Dispose();
            return response;
        }
        
        public EmployeeResponse[] GetEmployeeOptions(EmployeeRequest request)
        {
            var (scope, client) = GetServiceClient();
            var response = client.GetEmployeeOptions(request);
            scope.Dispose();
            return response;
        }

        private (OperationContextScope, HRDemoReportServiceClient) GetServiceClient()
        {
            // Create a binding (e.g., BasicHttpBinding)
            var scheme = new Uri(apiUrl).Scheme.ToLower();
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = scheme == "https" ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None// HTTPS recommended
                }
            };

            // Create an EndpointAddress
            var endpoint = new EndpointAddress(apiUrl);

            // Initialize the service client
            var client = new HRDemoReportServiceClient(binding, endpoint);

            // Inject the API Key into the outgoing message headers
            var scope = new OperationContextScope(client.InnerChannel);
            var header = MessageHeader.CreateHeader("X-Api-Key", "http://tempuri.org", apiKey);
            OperationContext.Current.OutgoingMessageHeaders.Add(header);

            // scope should be disposed after the client is used
            return (scope, client);
        }
    }
}
