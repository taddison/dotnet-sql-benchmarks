using BenchmarkDotNet.Running;

public class Program
{
  public static void Main(string[] args) => BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);
}
