# .NET ♥ SQL Benchmarks

## Requirements

- [.NET Framework 4.8.2 SDK] (Get the Developer Pack)
- [.NET5 SDK]
- A local instance of SQL Server (any edition)

## Running benchmarks

```shell
# Run all the benchmarks
dotnet run -c Release --framework net48 --filter * --join

# Run all the SqlClientUpdate benchmarks
dotnet run -c Release --filter SqlClientUpdate* --join

# Run the execute category of benchmarks in SqlClientUpdate
dotnet run -c Release --filter SqlClientUpdate* --allCategories execute --join
```

## List of benchmarks

- [SQLClientUpdate](./src/SqlClientUpdate/readme.md)

[.net framework 4.8.2 sdk]: https://dotnet.microsoft.com/download/dotnet-framework/net48
[.net5 sdk]: https://dotnet.microsoft.com/download/dotnet/5.0
