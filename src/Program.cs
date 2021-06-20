using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;
using NewSQL = Microsoft.Data.SqlClient;
using OldSQL = System.Data.SqlClient;

namespace dotnetSQLBenchmarks
{
  // March 2019 -> // Latest
  // https://www.nuget.org/packages/Microsoft.Data.SqlClient/
  // https://www.nuget.org/packages/System.Data.SqlClient/
  // https://www.nuget.org/packages/Dapper
  [Config(typeof(Config))]
  public class NewSqlConfig
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
    public void NewSQL()
    {
      using (var conn = new NewSQL.SqlConnection("server=localhost;initial catalog=master;integrated security=SSPI"))
      {
        var count = conn.Execute("select @@servername;");
      }
    }
  }

  [Config(typeof(Config))]
  public class OldSqlConfig
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
    public void OldSQL()
    {
      using (var conn = new OldSQL.SqlConnection("server=localhost;initial catalog=master;integrated security=SSPI"))
      {
        var count = conn.Execute("select @@servername;");
      }
    }
  }



  public class Program
  {
    public static void Main(string[] args) => BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);
    // public static void Main(string[] args) => Test(args);

    public static void Test(string[] args)
    {
      new OldSqlConfig().OldSQL();
      new NewSqlConfig().NewSQL();
    }
  }
}
