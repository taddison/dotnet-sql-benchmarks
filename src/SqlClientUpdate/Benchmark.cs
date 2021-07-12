using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;
using BenchmarkDotNet.Environments;

namespace SqlClientUpdate
{
  [MemoryDiagnoser]
  public abstract class Benchmark
  {
    public static Job Job = Job.Default;
    protected System.Data.Common.DbConnection _connection;

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();

    [Benchmark]
    [BenchmarkCategory("Execute")]
    public void Execute()
    {
      var count = _connection.Execute("select @@servername;");
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsSmall", "ColumnsSmall")]
    public void Query_10()
    {
      var count = _connection.Query<Sample>(Constants.Query_10);
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsLarge", "ColumnsSmall")]
    public void Query_100_000()
    {
      var count = _connection.Query<Sample>(Constants.Query_100_000);
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsSmall", "ColumnsLarge")]
    public void Query_ManyColumns_10()
    {
      var count = _connection.Query<SampleManyColumns>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsLarge", "ColumnsLarge")]
    public void Query_ManyColumns_100_000()
    {
      var count = _connection.Query<SampleManyColumns>(Constants.Query_ManyColumns_100_000);
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsSmall", "ColumnsLarge", "IgnoreColumns")]
    public void Query_ManyColumns_IgnoreMost_10()
    {
      var count = _connection.Query<Sample>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    [BenchmarkCategory("Query", "RowsLarge", "ColumnsLarge", "IgnoreColumns")]
    public void Query_ManyColumns_IgnoreMost_100_000()
    {
      var count = _connection.Query<Sample>(Constants.Query_ManyColumns_100_000);
    }
  }

  [Config(typeof(Config))]
  public class SqlClientUpdate_SystemData : Benchmark
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var baseJob = Benchmark.Job;

        var oldPackages = baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("System.Data.SqlClient", "4.6.0"),
            new NuGetReference("Dapper", "1.60.6"),
        });

        // Initial state - .NET Framework 4.8.2, Dapper 1.60.6, System.Data.SqlClient 4.6.0
        AddJob(oldPackages.WithRuntime(ClrRuntime.Net48));

        // ALL_BENCHMARKS: Uncomment the below to benchmark all variations of packages/runtimes
        // var newPackages = baseJob.WithNuGet(new NuGetReferenceList() {
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

  [Config(typeof(Config))]
  public class SqlClientUpdate_MicrosoftData : Benchmark
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var baseJob = Benchmark.Job;

        var newPackages = baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("Microsoft.Data.SqlClient", "3.0.0"),
            new NuGetReference("Dapper", "2.0.90"),
        });

        // Target state - .NET5, Dapper 2.0.90, Microsoft.Data.SqlClient 3.0.0
        AddJob(newPackages.WithRuntime(CoreRuntime.Core50));

        // ALL_BENCHMARKS: Uncomment the below to benchmark all variations of packages/runtimes
        // var oldPackages = baseJob.WithNuGet(new NuGetReferenceList() {
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