using DiscountService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DiscountService.Data;

public class DiscountServiceContext : DbContext
{
	public DbSet<Discount> Discounts { get; set; }

	public DiscountServiceContext(DbContextOptions<DiscountServiceContext> options) : base(options)
	{
		try
		{
			if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator dbCreator)
			{
				if (!dbCreator.CanConnect()) dbCreator.Create();
				if (!dbCreator.HasTables()) dbCreator.CreateTables();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}
}
