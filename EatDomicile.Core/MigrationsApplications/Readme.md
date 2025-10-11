# Add Migrations to drink database
dotnet ef migrations add InitDatabase --context ApplicationStoreContext --startup-project ../EatDomicile.Api --output-dir MigrationsApplications

# Update database
dotnet ef database update --startup-project ../EatDomicile.Api --context ApplicationStoreContext