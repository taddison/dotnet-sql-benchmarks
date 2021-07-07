# dotnet-sql-benchmarks

Requires:
- .NET Framework 4.8.2 SDK
- .NET5 SDK
- A local instance of SQL Server (any edition)

```shell
dotnet run -c Release --framework net48 --filter * --join

dotnet run -c Release --anyCategories execute --join
```

## Benchmarks

- [SQLClientUpdate](./src/SqlClientUpdate/readme.md)