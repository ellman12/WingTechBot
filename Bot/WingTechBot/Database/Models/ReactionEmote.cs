namespace WingTechBot.Database.Models;

///Represents a Discord emote used for the karma and reaction tracker systems.
public sealed class ReactionEmote(string name, ulong? discordEmoteId, int karmaValue = 0) : Model
{
	///ID of the row.
	[Key]
	public int Id { get; private init; }

	///The name of the emote, such as 'upvote' or 'eyes'.
	[Required]
	public string Name { get; private init; } = name;

	public ulong? DiscordEmoteId { get; private init; } = discordEmoteId;

	///How much karma is given or received when a message receives this emote as a reaction.
	public int KarmaValue { get; private set; } = karmaValue;

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public DateTime CreatedAt { get; private init; }

	public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

	///See: https://docs.discordnet.dev/guides/emoji/emoji.html
	[NotMapped]
	public IEmote Parsed => DiscordEmoteId == null ? Emoji.Parse($":{Name}:") : Emote.Parse($"<:{Name}:{DiscordEmoteId}>");

	///Convert this ReactionEmote into a string that Discord can interpret.
	public override string ToString() => DiscordEmoteId == null ? $":{Name}:" : $"<:{Name}:{DiscordEmoteId}>";

	public async Task SetKarmaValue(int newValue)
	{
		await using BotDbContext context = new();
		KarmaValue = newValue;
		await context.SaveChangesAsync();
	}

	public static async Task<ReactionEmote> Find(string name, ulong? discordEmoteId)
	{
		await using BotDbContext context = new();
		return await context.ReactionEmotes
			.Include(re => re.Reactions)
			.FirstOrDefaultAsync(e => e.Name == name && e.DiscordEmoteId == discordEmoteId);
	}

	public static async Task<ReactionEmote> AddEmote(string name, ulong? discordEmoteId, int karmaValue = 0)
	{
		if (String.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Invalid name");

		if (discordEmoteId == 0)
			throw new ArgumentException("Invalid id");

		await using BotDbContext context = new();

		var existing = await context.ReactionEmotes.FirstOrDefaultAsync(e => e.Name == name && e.DiscordEmoteId == discordEmoteId);
		if (existing != null)
			throw new ArgumentException("Emote exists in ReactionEmote table");

		ReactionEmote emote = new(name, discordEmoteId, karmaValue);
		await context.ReactionEmotes.AddAsync(emote);
		await context.SaveChangesAsync();
		return emote;
	}
}

public sealed class ReactionEmoteConfiguration : IEntityTypeConfiguration<ReactionEmote>
{
	public void Configure(EntityTypeBuilder<ReactionEmote> builder)
	{
		builder.Property(e => e.Id).ValueGeneratedOnAdd();
		builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
	}
}