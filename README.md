These applications are developed and migrated as an effort to provide minimal business applications as case studies of migrating .NET 4 applications. The repository contains four applications, each of different type.
1. HRDemoApi: This is a .NET 4 Web API application migrated to .NET 8 Web API.
2. HRDemoMvc: This is a .NET 4 MVC application migrated to .NET 8 MVC application.
3. HRDemoWebForm: This is .NET 4 Web Form application migrated to .NET 8 Blazor application.
4. HRDemoWcf: This is a .NET 4 WCF service migrated to .NET Core WCF service.

Note that, the Web Form and WCF applications are not supported as they are in .NET 8, and they may be migrated to other alternatives as well. For example, migrating WCF service to a gRPC service may be a better option than Core WCF. Similarly, Web Form UI can be migrated to Razor Pages too.
