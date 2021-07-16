using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using Dapper;

namespace SqlClientUpdate
{
  [MemoryDiagnoser]
  public abstract class Benchmark
  {
    protected static Job BaseJob = Job.Default;
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
}