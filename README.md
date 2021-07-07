# dotnet-sql-benchmarks

Requires:
- .NET Framework 4.8.2 SDK
- .NET5 SDK
- A local instance of SQL Server (any edition)

```shell
# Run all the benchmarks
dotnet run -c Release --framework net48 --filter * --join

# Run all the SqlClientUpdate benchmarks
dotnet run -c Release --filter SqlClientUpdate* --join

# Run the execute category of benchmarks in SqlClientUpdate
dotnet run -c Release --filter SqlClientUpdate* --allCategories execute --join
```

## Benchmarks

- [SQLClientUpdate](./src/SqlClientUpdate/readme.md)