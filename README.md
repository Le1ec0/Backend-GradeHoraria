# Passos para executar

```shell
dotnet tool install --global dotnet-ef

dotnet restore

dotnet ef migrations add InitialMigration

dotnet ef database drop

dotnet ef database update

dotnet watch run
```

luckoito was here
