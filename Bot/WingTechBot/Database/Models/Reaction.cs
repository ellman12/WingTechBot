namespace WingTechBot.Database.Models;

///Represents an emote reaction to a message.
public sealed class Reaction(ulong giverId, ulong receiverId, ulong messageId, string emote) : Model
{
	[Key]
	public int Id { get; private set; }

	[Required]
	public ulong GiverId { get; private set; } = giverId;

	[Required]
	public ulong ReceiverId { get; private set; } = receiverId;

	[Required]
	public ulong MessageId { get; private set; } = messageId;

	[Required]
	public string Emote { get; private set; } = emote;

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public DateTime Timestamp { get; private set; }

	public static async Task<Reaction> Find(ulong giverId, ulong receiverId, ulong messageId, string emote)
	{
		await using BotDbContext context = new();
		return await context.Reactions.FirstOrDefaultAsync(r => r.GiverId == giverId && r.ReceiverId == receiverId && r.MessageId == messageId && r.Emote == emote);
	}

public sealed class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
	public void Configure(EntityTypeBuilder<Reaction> builder)
	{
		builder.Property(e => e.Id).ValueGeneratedOnAdd();
		builder.Property(e => e.Timestamp).HasDefaultValueSql("now()");

		//Adds a unique constraint for all four values. Throws error if violated. AddReaction() checks this before adding a row.
		builder.HasIndex(e => new {e.GiverId, e.ReceiverId, e.MessageId, e.Emote}).IsUnique();
	}
}