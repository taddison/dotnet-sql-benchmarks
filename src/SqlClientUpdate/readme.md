Compare versions of Dapper and  System.Data.SqlClient|Microsoft.Data.SqlClient as of March 2019 -> To the latest (as of June 2021)
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