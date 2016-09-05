using Microsoft.EntityFrameworkCore;

namespace FirstDotNetConsoleApp.DAL
{
	public class SampleDataSqlContext : DbContext
	{
		public SampleDataSqlContext()
		{

		}

		public SampleDataSqlContext(DbContextOptions<SampleDataSqlContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(
					@"Data Source=localhost;Initial Catalog=sampledata;Integrated Security=True;MultipleActiveResultSets=True");
			}
		}

		public DbSet<Department> Departments { get; set; }
		public DbSet<Person> Persons { get; set; }
	}
}
