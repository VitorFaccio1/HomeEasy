using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Data;

public sealed class HomeEasyContext : DbContext
{
	public HomeEasyContext(DbContextOptions<HomeEasyContext> options) : base(options)
	{
	}

	public DbSet<User> Users { get; set; } = default!;

	public DbSet<Job> Jobs { get; set; } = default!;

	public DbSet<Ad> Ads { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Job>().HasData(
			new Job { Id = Guid.NewGuid(), Name = "Faxineiro(a)" },
			new Job { Id = Guid.NewGuid(), Name = "Pintor(a)" },
			new Job { Id = Guid.NewGuid(), Name = "Pedreiro(a)" }
		);

		modelBuilder.Entity<User>().HasData(
			new User
			{
				Id = Guid.NewGuid(),
				Email = "vitor@admin.com",
				Password = "AQAAAAIAAYagAAAAEA9Zd9k/oCFtBcUMOt94/Brh110GdKXeDqeFMwNEbxVUv2Mlv5xcn+dxEn8fkCrIWw==",
				Name = "Vitor Faccio",
				Type = UserType.Admin
			},
			new User
			{
				Id = Guid.NewGuid(),
				Email = "vitor@worker.com",
				Password = "AQAAAAIAAYagAAAAEA9Zd9k/oCFtBcUMOt94/Brh110GdKXeDqeFMwNEbxVUv2Mlv5xcn+dxEn8fkCrIWw==",
				Name = "Vitor Faccio",
				Type = UserType.Worker
			},
			new User
			{
				Id = Guid.NewGuid(),
				Email = "vitor@client.com",
				Password = "AQAAAAIAAYagAAAAEA9Zd9k/oCFtBcUMOt94/Brh110GdKXeDqeFMwNEbxVUv2Mlv5xcn+dxEn8fkCrIWw==",
				Name = "Vitor Faccio",
				Type = UserType.Client
			}
		);
	}
}