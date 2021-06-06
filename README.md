# SMART PARKING PLATFORM

## Structure

SmartParking.Share
    Constants
    IServices
    Enums
    Dto
SmartParkingCoreModels
    Identity
        Model
        Repository
    Parking
SmartParkingCoreServices
    Implement Service
SmartParkingImplementation

## First Migration
```PCM
Add-Migration InitialIdentityServerPersistedGrantDbMigration -Context PersistedGrantDbContext -Out Data/Migrations/IdentityServer/PersistedGrantDb
Add-Migration InitialIdentityServerConfigurationDbMigration -Context ConfigurationDbContext -Out Data/Migrations/IdentityServer/ConfigurationDb
Add-Migration InitialIdentityServerConfigurationDbMigration -Context ApplicationIdentityContext -Out Data/Migrations/IdentityServer/ApplicationDb

Add-Migration InitializeDbMigration -Context ApplicationDbContext -Out Data/Migrations/SmartParking
Update-Database -Context ApplicationDbContext
```

## Migrate Data
```cmd
dotnet run /seed
```