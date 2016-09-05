using System.ComponentModel.DataAnnotations.Schema;

namespace FirstDotNetConsoleApp.DAL
{
	[Table("Person")]
	public class Person
	{
		public int id { get; set; }
		public string first { get; set; }
		public string last { get; set; }
		public int department { get; set; }
	}
}
