# Password Manager - Assessment

My implementation to the tasks for the interview. Tasks can be found in issue [#1](https://github.com/YoussefWaelMohamedLotfy/PasswordManagerAssessment/issues/1)  
UI Design uses [Bootstrap 5.2.0](https://getbootstrap.com/)

## Project Requirements

In order to run the project, .NET 6.0.8 [(SDK 6.0.400)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) is required to be installed on your machine in order to run the project.

## Project Architecture

The following image shows the current state of the system's architecture.
![Simple Architecture Diagram](https://user-images.githubusercontent.com/40206862/181909662-9282cd76-d30c-4f18-a5b9-6741133bcd41.png)

## Database Connection String Setup

The database used for development is `SQL Server LocalDB Instance` that comes with VS Community. So the connection string should be changed to your instance's server address. i.e. Change the `(localdb)\MSSQLLocalDB` in the [AppDbContext File](https://github.com/YoussefWaelMohamedLotfy/PasswordManagerAssessment/blob/main/src/PasswordManager.API/Data/AppDbContext.cs#L19) to your instance.

## How to run

The following steps shows how to run the project:

+ Clone the repo on your machine.
+ Navigate to the repo's folder on your machine.
+ Open `PasswordManagerAssessment.sln` using Visual Studio Community 2022.
+ Make sure that the project `PasswordManager.UI`, `PasswordManager.API` and `IdentityServer` are set as Startup Projects.
+ Build the Solution using `Ctrl + Shift + B`, or Right-click on the solution file, then select `Build Solution`
+ Run the project using `Ctrl + F5`, or click on run located under the menu bar.

## IdentityServer Database Migrations

There are 3 DbContexts in `IdentityServer` project. In case you want to create a new migration for any of them, use the following commands:

+ ConfigurationDbContext

```powershell
Add-Migration <Migration Name> -Context ConfigurationDbContext -OutputDir "Migrations\ConfigurationDb"
Update-Database -Context ConfigurationDbContext
```

+ PersistedGrantDbContext

```powershell
Add-Migration <Migration Name> -Context PersistedGrantDbContext -OutputDir "Migrations\PersistedGrantDb"
Update-Database -Context PersistedGrantDbContext
```

+ AppDbContext

```powershell
Add-Migration <Migration Name> -Context AppDbContext -OutputDir "Migrations\AppDb"
Update-Database -Context AppDbContext
```

> Replace `<Migration Name>` with any name you prefer.

## Accounts for logging in

There are 2 accounts stored in-memory that are available for use.

|Subject ID|Username|Password|
|----------|:------:|:------:|
|1|alice|Pass123$|
|2|bob|Pass123$|
