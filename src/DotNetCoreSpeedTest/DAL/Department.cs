using System.ComponentModel.DataAnnotations.Schema;

namespace FirstDotNetConsoleApp.DAL
{
	[Table("Department")]
	public class Department
	{
		public int id { get; set; }
		public string name { get; set; }
	}
}
