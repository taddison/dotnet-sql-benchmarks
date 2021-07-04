using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;
using System;

namespace SqlClientUpdate
{
  public static class Constants
  {
    public static string ConnectionString = "server=localhost;initial catalog=master;integrated security=SSPI";
    // https://dba.stackexchange.com/a/152536
    public static string BaseQuery = @"
  with
L0 as (select 1 as c union all select 1),
L1 as (select 1 as c from L0 A cross join L0 B),
L2 as (select 1 as c from L1 A cross join L1 B),
L3 as (select 1 as c from L2 A cross join L2 B),
L4 as (select 1 as c from L3 A cross join L3),
L5 as (select 1 as c from L4 A cross join L4),
nums as (select 1 as NUM from L5)   
select top {rowcount}
  cast(row_number() over (order by (select null)) AS int) id
, cast(t.rand_value AS int) random_int
, cast(t.rand_value AS varchar(100)) random_string
, cast(getutcdate() AS datetime) fixed_date
from nums cross join (select round(1000 * rand(checksum(newid())), 0) rand_value) t;
  ";
    public static string Query_10 = BaseQuery.Replace("{rowcount}", "10");
    public static string Query_100_000 = BaseQuery.Replace("{rowcount}", "100000");

    public static string BaseQuery_ManyColumns = @"
  with
L0 as (select 1 as c union all select 1),
L1 as (select 1 as c from L0 A cross join L0 B),
L2 as (select 1 as c from L1 A cross join L1 B),
L3 as (select 1 as c from L2 A cross join L2 B),
L4 as (select 1 as c from L3 A cross join L3),
L5 as (select 1 as c from L4 A cross join L4),
nums as (select 1 as NUM from L5)   
select top {rowcount}
  cast(row_number() over (order by (select null)) AS int) id
, cast(t.rand_value AS int) random_int
, cast(t.rand_value AS int) random_int_1
, cast(t.rand_value AS int) random_int_2
, cast(t.rand_value AS int) random_int_3
, cast(t.rand_value AS int) random_int_4
, cast(t.rand_value AS int) random_int_5
, cast(t.rand_value AS int) random_int_6
, cast(t.rand_value AS int) random_int_7
, cast(t.rand_value AS int) random_int_8
, cast(t.rand_value AS int) random_int_9
, cast(t.rand_value AS varchar(100)) random_string
, cast(t.rand_value AS varchar(100)) random_string_1
, cast(t.rand_value AS varchar(100)) random_string_2
, cast(t.rand_value AS varchar(100)) random_string_3
, cast(t.rand_value AS varchar(100)) random_string_4
, cast(t.rand_value AS varchar(100)) random_string_5
, cast(t.rand_value AS varchar(100)) random_string_6
, cast(t.rand_value AS varchar(100)) random_string_7
, cast(t.rand_value AS varchar(100)) random_string_8
, cast(t.rand_value AS varchar(100)) random_string_9
, cast(getutcdate() AS datetime) fixed_date
, cast(getutcdate() AS datetime) fixed_date_1
, cast(getutcdate() AS datetime) fixed_date_2
, cast(getutcdate() AS datetime) fixed_date_3
, cast(getutcdate() AS datetime) fixed_date_4
, cast(getutcdate() AS datetime) fixed_date_5
, cast(getutcdate() AS datetime) fixed_date_6
, cast(getutcdate() AS datetime) fixed_date_7
, cast(getutcdate() AS datetime) fixed_date_8
, cast(getutcdate() AS datetime) fixed_date_9
from nums cross join (select round(1000 * rand(checksum(newid())), 0) rand_value) t;
  ";
    public static string Query_ManyColumns_10 = BaseQuery_ManyColumns.Replace("{rowcount}", "10");
    public static string Query_ManyColumns_100_000 = BaseQuery_ManyColumns.Replace("{rowcount}", "100000");
  }

  public class Sample
  {
    public int id { get; set; }
    public string random_string { get; set; }
    public DateTime fixed_date { get; set; }
    public int random_int { get; set; }
  }

  public class Sample_ManyColumns
  {
    public int id { get; set; }
    public string random_string { get; set; }
    public string random_string_1 { get; set; }
    public string random_string_2 { get; set; }
    public string random_string_3 { get; set; }
    public string random_string_4 { get; set; }
    public string random_string_5 { get; set; }
    public string random_string_6 { get; set; }
    public string random_string_7 { get; set; }
    public string random_string_8 { get; set; }
    public string random_string_9 { get; set; }
    public DateTime fixed_date { get; set; }
    public DateTime fixed_date_1 { get; set; }
    public DateTime fixed_date_2 { get; set; }
    public DateTime fixed_date_3 { get; set; }
    public DateTime fixed_date_4 { get; set; }
    public DateTime fixed_date_5 { get; set; }
    public DateTime fixed_date_6 { get; set; }
    public DateTime fixed_date_7 { get; set; }
    public DateTime fixed_date_8 { get; set; }
    public DateTime fixed_date_9 { get; set; }
    public int random_int { get; set; }
    public int random_int_1 { get; set; }
    public int random_int_2 { get; set; }
    public int random_int_3 { get; set; }
    public int random_int_4 { get; set; }
    public int random_int_5 { get; set; }
    public int random_int_6 { get; set; }
    public int random_int_7 { get; set; }
    public int random_int_8 { get; set; }
    public int random_int_9 { get; set; }
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

    private Microsoft.Data.SqlClient.SqlConnection _connection;

    [GlobalSetup]
    public void Setup()
    {
      _connection = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString);
    }

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();

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
      var count = _connection.Query<Sample_ManyColumns>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    public void Query_ManyColumns_100_000()
    {
      var count = _connection.Query<Sample_ManyColumns>(Constants.Query_ManyColumns_100_000);
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

    private System.Data.SqlClient.SqlConnection _connection;

    [GlobalSetup]
    public void Setup() => _connection = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString);

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();

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
      var count = _connection.Query<Sample_ManyColumns>(Constants.Query_ManyColumns_10);
    }

    [Benchmark]
    public void Query_ManyColumns_100_000()
    {
      var count = _connection.Query<Sample_ManyColumns>(Constants.Query_ManyColumns_100_000);
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
}