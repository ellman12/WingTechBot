namespace WingTechBot.Database.Models;

///Represents an emote that has a karma value.
public sealed class KarmaEmote(Emote emote, int amount) : Model
{
	public Emote Emote { get; } = emote;

	public int Amount { get; } = amount;
	
}