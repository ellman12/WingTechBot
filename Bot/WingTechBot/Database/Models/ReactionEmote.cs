using System.ComponentModel.DataAnnotations.Schema;

namespace WingTechBot.Database.Models;

///Represents a Discord emote used for the karma and reaction tracker systems.
public sealed class ReactionEmote(string name, ulong? emoteId) : Model
{
	///ID of the row.
	[Key]
	public int Id { get; private init; }

	///The name of the emote, such as 'upvote' or 'eyes'.
	[Required]
	public string Name { get; private init; } = name;

	///If a custom emote, the ID assigned by Discord.
	public ulong? EmoteId { get; private init; } = emoteId;

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public DateTime CreatedAt { get; private init; }
}

public sealed class ReactionEmoteConfiguration : IEntityTypeConfiguration<ReactionEmote>
{
	public void Configure(EntityTypeBuilder<ReactionEmote> builder)
	{
		builder.Property(e => e.Id).ValueGeneratedOnAdd();
		builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
	}
}