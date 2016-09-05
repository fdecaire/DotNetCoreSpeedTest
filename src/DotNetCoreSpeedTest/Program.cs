namespace FirstDotNetConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var efsqlTests = new EfsqlPerformanceTests();
			efsqlTests.RunAllTests();
		}
	}
}
