using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Environments;

namespace SqlClientUpdate
{
  [Config(typeof(Config))]
  public class SqlClientUpdate_SystemData : Benchmark
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var oldPackages = BaseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("System.Data.SqlClient", "4.6.0"),
            new NuGetReference("Dapper", "1.60.6"),
        });

        // Initial state - .NET Framework 4.8.2, Dapper 1.60.6, System.Data.SqlClient 4.6.0
        AddJob(oldPackages.WithRuntime(ClrRuntime.Net48));

        // ALL_BENCHMARKS: Uncomment the below to benchmark all variations of packages/runtimes
        // var newPackages = BaseJob.WithNuGet(new NuGetReferenceList() {
        //     new NuGetReference("System.Data.SqlClient", "4.8.2"),
        //     new NuGetReference("Dapper", "2.0.90"),
        // });
        // AddJob(oldPackages.WithRuntime(CoreRuntime.Core50));
        // AddJob(newPackages.WithRuntime(ClrRuntime.Net48));
        // AddJob(newPackages.WithRuntime(CoreRuntime.Core50));
      }
    }

    [GlobalSetup]
    public void Setup()
    {
      _connection = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString);
      _connection.Open();
    }
  }
}