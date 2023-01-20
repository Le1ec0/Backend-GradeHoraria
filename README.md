# Passos para executar

```shell
dotnet tool install --global dotnet-ef

dotnet restore

dotnet ef migrations add InitialMigration

dotnet ef database drop

dotnet ef database update

dotnet watch run
```

# Passos para publicar no IIS

Confirme se no código tem o item abaixo:

```C#
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

```shell
dotnet build
```

Após o comando acima vai ser gerado os arquivos para públicar, abaixo o caminho:

    bin\Debug\net6.0

Copie o conteúdo da pasta acima para a pasta do IIS

    C:\inetpub\wwwroot

O conteudo do arquivo web.config na pasta acima:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\GradeHoraria.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```
luckoito was here