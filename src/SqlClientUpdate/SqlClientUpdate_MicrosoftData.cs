using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Environments;

namespace SqlClientUpdate
{
  [Config(typeof(Config))]
  public class SqlClientUpdate_MicrosoftData : Benchmark
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var newPackages = BaseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("Microsoft.Data.SqlClient", "3.0.0"),
            new NuGetReference("Dapper", "2.0.90"),
        });

        // Target state - .NET5, Dapper 2.0.90, Microsoft.Data.SqlClient 3.0.0
        AddJob(newPackages.WithRuntime(CoreRuntime.Core50));

        // ALL_BENCHMARKS: Uncomment the below to benchmark all variations of packages/runtimes
        // var oldPackages = BaseJob.WithNuGet(new NuGetReferenceList() {
        //     new NuGetReference("Microsoft.Data.SqlClient", "1.0.19239.1"),
        //     new NuGetReference("Dapper", "1.60.6"),
        // });
        // AddJob(oldPackages.WithRuntime(ClrRuntime.Net48));
        // AddJob(oldPackages.WithRuntime(CoreRuntime.Core50));
        // AddJob(newPackages.WithRuntime(ClrRuntime.Net48));

      }
    }

    [GlobalSetup]
    public void Setup()
    {
      _connection = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString);
      _connection.Open();
    }
  }
}