namespace HRDemoReportServiceCore
{
    public class ApiKey
    {
        public ApiKey(string nameAndKeyString)
        {
            var parts = nameAndKeyString.Split('.');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid API Key: Not in Format NAME.KEY");
            }
            Name = parts[0];
            Key = parts[1];
        }
        public string Name { get; }
        public string Key { get; }
    }

    public class ApiKeyAuthorizationManager : ServiceAuthorizationManager
    {
        private readonly ApiKey[] _validKeys = [.. File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apikeys.txt"))
                                                   .Where(k => k.Length > 1)
                                                   .Select(k => new ApiKey(k))];
        protected override ValueTask<bool> CheckAccessCoreAsync(OperationContext operationContext)
        {
            var headers = operationContext.RequestContext.RequestMessage.Headers;
            var headerIndex = headers.FindHeader("X-Api-Key", "http://tempuri.org/");
            if (headerIndex == -1)
            {
                return new(false);
            }
            return new(ValidateApiKey(headers.GetHeader<string>(headerIndex)));
        }

        private bool ValidateApiKey(string apiKey)
        {
            ApiKey obtainedKey = new(apiKey);
            // In practice this method would validate the API key against
            // a secured data store such as a database or a key vault

            // Replace with your validation logic
            foreach (var validKey in _validKeys)
            {
                if (validKey.Name == obtainedKey.Name && validKey.Key == obtainedKey.Key)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
