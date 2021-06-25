# dotnet-sql-benchmarks

## TODO
- There is some query overhead in `SysObjectsQueryLarge` - ideally stage the data in something that runs at the start of the benchmark (##temp table)

```shell
dotnet run -c Release --filter ** --join
```