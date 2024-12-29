namespace WingTechBot.Database.Models;

///Represents an emote reaction to a message.
public sealed class Reaction(int id, ulong giverId, ulong receiverId, ulong messageId, Emote emote, DateTime timestamp) : Model
{
	public int Id { get; } = id;

	public ulong GiverId { get; } = giverId;

	public ulong ReceiverId { get; } = receiverId;

	public ulong MessageId { get; } = messageId;
	
	public Emote Emote { get; } = emote;

	public DateTime Timestamp { get; } = timestamp;

}