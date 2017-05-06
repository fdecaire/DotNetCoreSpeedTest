using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FirstDotNetConsoleApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace FirstDotNetConsoleApp
{
	public class EfsqlPerformanceTests
	{
		private int _departmentKey;

		public EfsqlPerformanceTests()
		{
			File.Delete(@"c:\dotnet_speed_tests.txt");
		}

		public void InitializeData()
		{
			// clean up any data from previous runs
			using (var db = new SampleDataSqlContext())
			{
				// delete any records from previous run
				var personQuery = (from pers in db.Persons select pers).ToList();
				db.Persons.RemoveRange(personQuery);
				db.SaveChanges();

				var deptQuery = (from dept in db.Departments select dept).ToList();
				db.Departments.RemoveRange(deptQuery);
				db.SaveChanges();

				// insert one department
				var myDepartment = new Department
				{
					name = "Operations"
				};

				db.Departments.Add(myDepartment);
				db.SaveChanges();

				// select the primary key of the department table for the only record that exists
				_departmentKey = (from d in db.Departments where d.name == "Operations" select d.id).FirstOrDefault();
			}
		}

		public void RunAllTests()
		{
			double smallest = -1;
			for (int i = 0; i < 5; i++)
			{
				InitializeData();
				double result = TestInsert();

				if (smallest < 0)
				{
					smallest = result;
				}
				else
				{
					if (result < smallest)
					{
						smallest = result;
					}
				}
			}
			WriteLine("INSERT:" + smallest);
			Console.WriteLine("INSERT:" + smallest);

			smallest = -1;
			for (int i = 0; i < 5; i++)
			{
				InitializeData();
				TestInsert();

				double result = TestUpdate();

				if (smallest < 0)
				{
					smallest = result;
				}
				else
				{
					if (result < smallest)
					{
						smallest = result;
					}
				}
			}
			WriteLine("UPDATE:" + smallest);
			Console.WriteLine("UPDATE:" + smallest);

			smallest = -1;
			for (int i = 0; i < 5; i++)
			{
				InitializeData();
				TestInsert();

				double result = TestSelect();

				if (smallest < 0)
				{
					smallest = result;
				}
				else
				{
					if (result < smallest)
					{
						smallest = result;
					}
				}
			}
			WriteLine("SELECT:" + smallest);
			Console.WriteLine("SELECT:" + smallest);

			smallest = -1;
			for (int i = 0; i < 5; i++)
			{
				InitializeData();
				TestInsert();

				double result = TestDelete();

				if (smallest < 0)
				{
					smallest = result;
				}
				else
				{
					if (result < smallest)
					{
						smallest = result;
					}
				}
			}
			WriteLine("DELETE:" + smallest);
			Console.WriteLine("DELETE:" + smallest);

			WriteLine("");
		}

		public double TestInsert()
		{
			using (var db = new SampleDataSqlContext())
			{
				db.ChangeTracker.AutoDetectChangesEnabled = false;

				// read first and last names
				var firstnames = new List<string>();
				using (var sr = new StreamReader(File.OpenRead(@"Data\firstnames.txt")))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
						firstnames.Add(line);
				}

				var lastnames = new List<string>();
				using (var sr = new StreamReader(File.OpenRead(@"Data\lastnames.txt")))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
						lastnames.Add(line);
				}

				//test inserting 10000 records (only ~1,000 names in text)
				var startTime = DateTime.Now;
				for (int j = 0; j < 10; j++)
				{
					for (int i = 0; i < 1000; i++)
					{
						var personRecord = new Person
						{
							first = firstnames[i],
							last = lastnames[i],
							department = _departmentKey
						};

						db.Persons.Add(personRecord);
					}
				}

				db.SaveChanges();
				var elapsedTime = DateTime.Now - startTime;

				return elapsedTime.TotalSeconds;
			}
		}

		public double TestSelect()
		{
			using (var db = new SampleDataSqlContext())
			{
				db.ChangeTracker.AutoDetectChangesEnabled = false;

				var startTime = DateTime.Now;
				for (int i = 0; i < 1000; i++)
				{
					var query = (from p in db.Persons
								 join d in db.Departments on p.department equals d.id
								 select p).ToList();
				}
				var elapsedTime = DateTime.Now - startTime;

				return elapsedTime.TotalSeconds;
			}
		}

		public double TestUpdate()
		{
			using (var db = new SampleDataSqlContext())
			{
				var startTime = DateTime.Now;
				var query = (from p in db.Persons select p).ToList();
				foreach (var item in query)
				{
					item.last = item.last + "2";
				}
				db.SaveChanges();

				var elapsedTime = DateTime.Now - startTime;

				return elapsedTime.TotalSeconds;
			}
		}

		public double TestDelete()
		{
			using (var db = new SampleDataSqlContext())
			{
				var startTime = DateTime.Now;
				var personQuery = (from pers in db.Persons select pers).AsNoTracking().ToList();

				db.Persons.RemoveRange(personQuery);
				db.SaveChanges();
				var elapsedTime = DateTime.Now - startTime;

				return elapsedTime.TotalSeconds;
			}
		}

		public void WriteLine(string text)
		{
			using (var fs = new FileStream(@"c:\dotnet_speed_tests.txt", FileMode.Append, FileAccess.Write))
			using (var writer = new StreamWriter(fs))
			{
				writer.WriteLine(text);
			}
		}
	}

}
