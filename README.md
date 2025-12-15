# 1. Create App Service -> Without DB

Follow the standard Azure Portal steps to provision an App Service instance:

* Resource Group: Select or create a new one.
* Name: Unique app name (e.g., myapp.azurewebsites.net).
* Publish: Code.
* Runtime Stack: .NET (latest LTS).
* Region: Choose closest to users.
* App Service Plan: Basic or higher for production.
* Enable System-assigned Managed Identity: Turn On (under Identity blade after creation).


# 2. Create a sql server -> SQL Server authenticaion mode -> On -> Set Admin Creds

### Azure SQL Server Configuration (MANDATORY)

1. Set Microsoft Entra ID (Azure AD) Admin
2. Azure Portal → SQL Server (not database)
3. Microsoft Entra ID → Set admin
4. Select the correct Azure AD user or Managed Identity
5. For Initial Setup -> Use (Account Id -> ex. pd@gmail.com)
6. Save 

Without this step, Azure SQL will reject all AAD tokens.

### Connection String Rules (MANDATORY)

```sh
"DefaultConnection": "Server=tcp:<server>.database.windows.net,1433;Database=<db>;Encrypt=True;"
```

# 3. Visual Studio/IDE -> Login with Azure Account 

# 4. Setup Code base -> supporting Azure AD authentication -> replace sql server authentication code with this code snippet

`program.cs`

```sh
var tokenCredential = new DefaultAzureCredential();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
});
```

`appsettings.json`

```sh
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:<sqlserver>.database.windows.net;Database=GDW2;Encrypt=True;"
}
```

> Add migration
> Update Database
> If any login failed error occurs -> check if admin is correctly set


Make sure app is running and is connected to DB locally.

# 5. Publish App on App Service (Web App)

* Publish -> Azure -> Azure App Service (Windows)
* Select App Service instance
* Skip this step -> API Management
* Publish (generates pubxml file)
* Finish
* Connect Database -> SQL Server Database
* Connect -> Azure SQL Database
* Select Database
* Connection string name -> Select same name as in appsettings.json
* Summary of changes -> keep all 3 selected

**Publish**

Still Database connection won't get established.

# 6. Final Setup

### Azure SQL Server Configuration (MANDATORY)

1. App service instance -> on which app is currently running
2. Search -> Identity -> System assigned -> Object (principal) ID -> Copy
3. Set Microsoft Entra ID (Azure AD) Admin
4. Azure Portal → SQL Server (not database)
5. Microsoft Entra ID → Set admin
6. For Final Setup -> Use (Object (principal) ID -> ex. 31e845da-a648-4d82-9c9c-77e0c)

# 7. Event Logs (Troubleshooting and Monitoring)

* In App Service → Support & troubleshooting → Diagnose and solve problems.
* Category: Diagnostic tools → Application Event Logs.
* Check for errors like "Login failed" (indicates missing DB user) or token issues (check identity status).
* Advanced: Use Kudu (https://<app>.scm.azurewebsites.net) → Debug console → Log stream for real-time logs.
