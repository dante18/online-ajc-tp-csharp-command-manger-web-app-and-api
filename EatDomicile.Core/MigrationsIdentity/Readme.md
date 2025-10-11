# Add Migrations to identity database
dotnet ef migrations add InitDatabase --context IdentityContext --startup-project ../EatDomicile.Api --output-dir MigrationsIdentity

# Update database
dotnet ef database update --startup-project ../EatDomicile.Api --context IdentityContext