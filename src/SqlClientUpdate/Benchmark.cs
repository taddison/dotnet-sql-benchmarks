using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;

namespace SqlClientUpdate
{
  public abstract class Base {
    protected System.Data.Common.DbConnection _connection;

    [Benchmark]
    public void Execute()
    {
      var count = _connection.Execute("select @@servername;");
    }

    [Benchmark]
    public void Query_10()
    {
      var count = _connection.Query<Sample>(Constants.Query_10);
    }

    [Benchmark]
    public void Query_100_000()
    {
      var count = _connection.Query<Sample>(Constants.Query_100_000);
    }

    [Benchmark]
    public void Query_ManyColumns_10()
    {
      var count = _connection.Query<SampleManyColumns>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    public void Query_ManyColumns_100_000()
    {
      var count = _connection.Query<SampleManyColumns>(Constants.Query_ManyColumns_100_000);
    }

    [Benchmark]
    public void Query_ManyColumns_IgnoreMost_10()
    {
      var count = _connection.Query<Sample>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    public void Query_ManyColumns_IgnoreMost_100_000()
    {
      var count = _connection.Query<Sample>(Constants.Query_ManyColumns_100_000);
    }
  }

  [Config(typeof(Config))]
  public class MicrosoftDataSqlClient : Base
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var baseJob = Job.Default;

        AddJob(baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("Microsoft.Data.SqlClient", "1.0.19239.1"),
            new NuGetReference("Dapper", "1.60.6"),
        }));
        AddJob(baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("Microsoft.Data.SqlClient", "3.0.0"),
            new NuGetReference("Dapper", "2.0.90"),
        }));

        AddDiagnoser(MemoryDiagnoser.Default);
      }
    }

    [GlobalSetup]
    public void Setup()
    {
      _connection = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString);
      _connection.Open();
    }

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();

    
  }

  [Config(typeof(Config))]
  public class SystemDataSqlClient : Base
  {
    private class Config : ManualConfig
    {
      public Config()
      {
        var baseJob = Job.Default;

        AddJob(baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("System.Data.SqlClient", "4.6.0"),
            new NuGetReference("Dapper", "1.60.6"),
        }));
        AddJob(baseJob.WithNuGet(new NuGetReferenceList() {
            new NuGetReference("System.Data.SqlClient", "4.8.2"),
            new NuGetReference("Dapper", "2.0.90"),
        }));

        AddDiagnoser(MemoryDiagnoser.Default);
      }
    }

    [GlobalSetup]
    public void Setup() {
      _connection = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString);
      _connection.Open();
    }

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();
  }
}