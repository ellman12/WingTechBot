namespace WingTechBot.Database.Models;

///Represents a reaction to a Discord message.
public sealed class Reaction(ulong giverId, ulong receiverId, ulong messageId, int emoteId) : Model
{
	[Key]
	public int Id { get; private init; }

	[Required]
	public ulong GiverId { get; private init; } = giverId;

	[Required]
	public ulong ReceiverId { get; private init; } = receiverId;

	[Required]
	public ulong MessageId { get; private init; } = messageId;

	[Required]
	public int EmoteId { get; private init; } = emoteId;

	public ReactionEmote Emote { get; private init; }

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public DateTime CreatedAt { get; private init; }

	public static async Task<Reaction> Find(ulong giverId, ulong receiverId, ulong messageId, int emoteId)
	{
		await using BotDbContext context = new();
		return await context.Reactions
			.Include(r => r.Emote)
			.FirstOrDefaultAsync(e => e.GiverId == giverId && e.ReceiverId == receiverId && e.MessageId == messageId && e.EmoteId == emoteId);
	}

}

public sealed class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
	public void Configure(EntityTypeBuilder<Reaction> builder)
	{
		builder.Property(e => e.Id).ValueGeneratedOnAdd();
		builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

		//One ReactionEmote can have 0, 1, or more Reactions referencing it.
		builder
			.HasOne(r => r.Emote)
			.WithMany(re => re.Reactions)
			.HasForeignKey(r => r.EmoteId);
	}
}