namespace WingTechBot.Database;

///Manages interfacing with WingTech Bot's database.
public sealed class BotDbContext : DbContext
{
	//Tables
	public DbSet<KarmaEmote> KarmaEmotes { get; set; }
	public DbSet<Reaction> Reactions { get; set; }

	///Configures the database.
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string host = Environment.GetEnvironmentVariable("DATABASE_HOST");
		string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
		string user = Environment.GetEnvironmentVariable("DATABASE_USER");
		string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

		optionsBuilder.UseNpgsql($"Host={host}; Database={dbName}; Username={user}; Password={password}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new ReactionConfiguration());
	}
}