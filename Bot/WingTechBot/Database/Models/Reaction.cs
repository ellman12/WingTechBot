namespace WingTechBot.Database.Models;

///Represents an emote reaction to a message.
public sealed class Reaction(int id, ulong giverId, ulong receiverId, ulong messageId, string emote, DateTime timestamp) : Model
{
	[Key]
	public int Id { get; private set; } = id;

	[Required]
	public ulong GiverId { get; private set; } = giverId;

	[Required]
	public ulong ReceiverId { get; private set; } = receiverId;

	[Required]
	public ulong MessageId { get; private set; } = messageId;

	[Required]
	public string Emote { get; private set; } = emote;

	[Required]
	public DateTime Timestamp { get; private set; } = timestamp;
}