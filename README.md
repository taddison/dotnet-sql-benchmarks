# dotnet-sql-benchmarks

- Opening a single query before all benchmarks would mean each benchmark has less code
- Copy-pasting the query is error-prone - could it be generated ahead of time for each different benchmark?

```shell
dotnet run -c Release --filter ** --join
```