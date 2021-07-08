# SQLClientUpdate

Compare the performance of two stacks that represent March 2019 and May 2021:
- Runtime: .NET 4.8 vs. .NET 5
- Data access: System.Data.SqlClient 4.6.0 vs. Microsoft.Data.SqlClient 3.0.0
- ORM: Dapper 1.60.6 vs. Dapper 2.0.90

Links:
- https://www.nuget.org/packages/Microsoft.Data.SqlClient/
- https://www.nuget.org/packages/System.Data.SqlClient/
- https://www.nuget.org/packages/Dapper

Also compares .NET Framework 4.8.2 vs. .NET5.

To compare everything (the full matrix of framework, Dapper, and SqlClient libraries - uncomment the sections of code in `Benchmark.cs` near `ALL_BENCHMARKS`)

To run only these benchmarks, or a subset:
```shell
dotnet run -c Release -f net48 --filter SqlClientUpdate* --join

dotnet run -c Release -f net48 --filter SqlClientUpdate* --allCategories Execute --join
```

And to run a shorter or longer benchmark modify the following in `Benchmark.cs`:

```csharp
public static Job Job = Job.Default; // Job.ShortRun, Job.VeryLongRun, etc.
```