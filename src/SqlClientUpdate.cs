using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;
using System;

public static class Constants
{
  public static string ConnectionString = "server=localhost;initial catalog=master;integrated security=SSPI";

  public static string Query_10 = @"
  WITH
L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
NUMS AS (SELECT 1 AS NUM FROM L5)   
SELECT TOP 10
  CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) id
, CAST(t.RAND_VALUE AS int) random_int
, CAST(t.RAND_VALUE AS VARCHAR(100)) random_string
, CAST(GETDATE() AS DATETIME) current_date
FROM NUMS CROSS JOIN (SELECT ROUND(1000 * RAND(CHECKSUM(NEWID())), 0) RAND_VALUE) t;
  ";
  public static string Query_1_000 = @"
  WITH
L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
NUMS AS (SELECT 1 AS NUM FROM L5)   
SELECT TOP 1000
  CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) id
, CAST(t.RAND_VALUE AS int) random_int
, CAST(t.RAND_VALUE AS VARCHAR(100)) random_string
, CAST(GETDATE() AS DATETIME) current_date
FROM NUMS CROSS JOIN (SELECT ROUND(1000 * RAND(CHECKSUM(NEWID())), 0) RAND_VALUE) t;
  ";

  public static string Query_100_000 = @"
  WITH
L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
NUMS AS (SELECT 1 AS NUM FROM L5)   
SELECT TOP 100000
  CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) id
, CAST(t.RAND_VALUE AS int) random_int
, CAST(t.RAND_VALUE AS VARCHAR(100)) random_string
, CAST(GETDATE() AS DATETIME) current_date
FROM NUMS CROSS JOIN (SELECT ROUND(1000 * RAND(CHECKSUM(NEWID())), 0) RAND_VALUE) t;
  ";

  public static string Query_1_000_000 = @"
  WITH
L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
L1 AS (SELECT 1 AS c FROM L0 A CROSS JOIN L0 B),
L2 AS (SELECT 1 AS c FROM L1 A CROSS JOIN L1 B),
L3 AS (SELECT 1 AS c FROM L2 A CROSS JOIN L2 B),
L4 AS (SELECT 1 AS c FROM L3 A CROSS JOIN L3),
L5 AS (SELECT 1 AS c FROM L4 A CROSS JOIN L4),
NUMS AS (SELECT 1 AS NUM FROM L5)   
SELECT TOP 1000000
  CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) id
, CAST(t.RAND_VALUE AS int) random_int
, CAST(t.RAND_VALUE AS VARCHAR(100)) random_string
, CAST(GETDATE() AS DATETIME) current_date
FROM NUMS CROSS JOIN (SELECT ROUND(1000 * RAND(CHECKSUM(NEWID())), 0) RAND_VALUE) t;
  ";
}

public class Sample
{
  public int id { get; set; }
  public string random_string { get; set; }
  public DateTime current_date { get; set; }
  public int random_int { get; set; }
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
  public void ExecuteQueryWithResults_10()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_10);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_1_000()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_1_000);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_100_000()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_100_000);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_1_000_000()
  {
    using (var conn = new Microsoft.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_1_000_000);
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
  public void ExecuteQueryWithResults_10()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_10);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_1_000()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_1_000);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_100_000()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_100_000);
    }
  }

  [Benchmark]
  public void ExecuteQueryWithResults_1_000_000()
  {
    using (var conn = new System.Data.SqlClient.SqlConnection(Constants.ConnectionString))
    {
      var count = conn.Query<Sample>(Constants.Query_1_000_000);
    }
  }
}