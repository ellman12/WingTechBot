using System.ComponentModel.DataAnnotations.Schema;

namespace WingTechBot.Database.Models;

///Represents a Discord emote used for the karma and reaction tracker systems.
public sealed class ReactionEmote(string name, ulong? emoteId, int karmaValue = 0) : Model
{
	///ID of the row.
	[Key]
	public int Id { get; private init; }

	///The name of the emote, such as 'upvote' or 'eyes'.
	[Required]
	public string Name { get; private init; } = name;

	///If a custom emote, the ID assigned by Discord.
	public ulong? EmoteId { get; private init; } = emoteId;

	///How much karma is given or received when a message receives this emote as a reaction.
	public int KarmaValue { get; private set; } = karmaValue;

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public DateTime CreatedAt { get; private init; }

	///See: https://docs.discordnet.dev/guides/emoji/emoji.html
	[NotMapped]
	public IEmote Parsed => EmoteId == null ? Emoji.Parse($":{Name}:") : Emote.Parse($"<:{Name}:{EmoteId}>");

	public async Task SetKarmaValue(int newValue)
	{
		await using BotDbContext context = new();
		KarmaValue = newValue;
		await context.SaveChangesAsync();
	}

	public static async Task<ReactionEmote> Find(string name, ulong? emoteId)
	{
		await using BotDbContext context = new();
		return await context.ReactionEmotes.FirstOrDefaultAsync(e => e.Name == name && e.EmoteId == emoteId);
	}

	public static async Task<ReactionEmote> AddEmote(string name, ulong? emoteId, int karmaValue = 0)
	{
		if (String.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Invalid name");

		if (emoteId == 0)
			throw new ArgumentException("Invalid id");

		await using BotDbContext context = new();

		var existing = await context.ReactionEmotes.FirstOrDefaultAsync(e => e.Name == name && e.EmoteId == emoteId);
		if (existing != null)
			throw new ArgumentException("Emote exists in ReactionEmote table");

		ReactionEmote emote = new(name, emoteId, karmaValue);
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