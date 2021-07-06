# dotnet-sql-benchmarks

- Put job back to default (not short run)
- Consider a toggle for multiple runtimes

```shell
dotnet run -c Release --framework net48 --filter * --join

dotnet run -c Release --anyCategories execute --join
```