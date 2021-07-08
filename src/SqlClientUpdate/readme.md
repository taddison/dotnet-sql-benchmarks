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

