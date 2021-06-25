using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;
using System;

public static class Constants
{
  public static string ConnectionString = "server=localhost;initial catalog=master;integrated security=SSPI";
  public static string SysObjectsQuerySmall = "select top 100 o1.object_id, o1.name, o1.create_date, o1.is_ms_shipped from master.sys.objects o1 order by o1.object_id asc";
  public static string SysObjectsQueryLarge = "select top 100000 o1.object_id, o1.name, o1.create_date, o1.is_ms_shipped from master.sys.objects o1 cross apply master.sys.objects o2 cross apply master.sys.objects o3 order by o1.object_id, o2.object_id, o3.object_id";
}

public class SysObject
{
  public int object_id { get; set; }
  public string name { get; set; }
  public DateTime create_date { get; set; }
  public bool is_ms_shipped { get; set; }
}

// Dapper & SqlClient [Old/New] as of March 2019 -> // Latest of each
// https://www.nuget.org/packages/Microsoft.Data.SqlClient/
// https://www.nuget.org/packages/System.Data.SqlClient/
// https://www.nuget.org/packages/Dapper

[Config(typeof(Config))]
public class SqlClientUpdate_MicrosoftDataSqlClient
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

  [Benchmark]
  public void ExecuteNoResult()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Execute("select @@servername;");
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResult()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<SysObject>(Constants.SysObjectsQuerySmall);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResult_Large()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<SysObject>(Constants.SysObjectsQueryLarge);
    }
  }
}

[Config(typeof(Config))]
public class SqlClientUpdate_SystemDataSqlClient
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

  [Benchmark]
  public void ExecuteNoResult()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Execute("select @@servername;");
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResult()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<SysObject>(Constants.SysObjectsQuerySmall);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResult_Large()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<SysObject>(Constants.SysObjectsQueryLarge);
    }
  }
}