# SQLClientUpdate

## Scenario

Compare the performance of two stacks that represent March 2019 and May 2021:

- Runtime: .NET 4.8 vs. .NET 5
- Data access: System.Data.SqlClient 4.6.0 vs. Microsoft.Data.SqlClient 3.0.0
- ORM: Dapper 1.60.6 vs. Dapper 2.0.90

The benchmarks are:

- Execute (no result set)
- Queries returning 10|100,000 rows and 4|31 columns
- Queries returning 10|100,000 rows and 31 columns, but only mapping 4 columns

## Running the benchmarks

```shell
# Run all benchmarks
dotnet run -c Release -f net48 --filter SqlClientUpdate* --join

# Run only the Execute category
dotnet run -c Release -f net48 --filter SqlClientUpdate* --allCategories Execute --join

# Categories include
# Execute
# Query
# RowsSmall, RowsLarge
# ColumnsSmall, ColumnsLarge
# IgnoreColumns
```

## Changing the duration

Modify the following in `Benchmark.cs`:

```csharp
public static Job Job = Job.Default; // Job.ShortRun, Job.VeryLongRun, etc.
```

## Running additional combinations

To compare everything (the full matrix of framework, Dapper, and SqlClient libraries - uncomment the sections of code in `Benchmark.cs` near `ALL_BENCHMARKS` - there are two blocks to uncomment).

## Links

- https://www.nuget.org/packages/Microsoft.Data.SqlClient/
- https://www.nuget.org/packages/System.Data.SqlClient/
- https://www.nuget.org/packages/Dapper

## Sample results

```ini
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1081 (21H1/May2021Update)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4360.0), X64 RyuJIT
  Job-NNUNVH : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  Job-OXRWXW : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  Job-XFMXPM : .NET Framework 4.8 (4.8.4360.0), X64 RyuJIT
  Job-MKMJCN : .NET Framework 4.8 (4.8.4360.0), X64 RyuJIT
```

| Stack | Method                               | Mean          | StdDev        | Allocated     |
| ----- | ------------------------------------ | ------------- | ------------- | ------------- |
| Old   | Execute                              | 124.03 μs     | 3.165 μs      | 933 B         |
| New   | Execute                              | 101.87 μs     | 1.317 μs      | 576 B         |
| Old   | Query_10                             | 121.56 μs     | 4.090 μs      | 4,992 B       |
| New   | Query_10                             | 92.56 μs      | 1.996 μs      | 3,983 B       |
| Old   | Query_100_000                        | 155,143.98 μs | 2,221.016 μs  | 18,951,408 B  |
| New   | Query_100_000                        | 121,151.24 μs | 2,912.253 μs  | 16,492,605 B  |
| Old   | Query_ManyColumns_10                 | 289.17 μs     | 15.380 μs     | 24,789 B      |
| New   | Query_ManyColumns_10                 | 230.56 μs     | 23.728 μs     | 21,081 B      |
| Old   | Query_ManyColumns_100_000            | 937,473.18 μs | 39,480.192 μs | 109,737,680 B |
| New   | Query_ManyColumns_100_000            | 823,025.37 μs | 13,220.497 μs | 106,841,600 B |
| Old   | Query_ManyColumns_IgnoreMost_10      | 217.32 μs     | 27.113 μs     | 15,720 B      |
| New   | Query_ManyColumns_IgnoreMost_10      | 191.71 μs     | 6.891 μs      | 12,047 B      |
| Old   | Query_ManyColumns_IgnoreMost_100_000 | 659,251.21 μs | 7,667.372 μs  | 18,968,848 B  |
| New   | Query_ManyColumns_IgnoreMost_100_000 | 675,893.78 μs | 29,096.822 μs | 16,501,008 B  |
