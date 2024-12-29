using System.ComponentModel.DataAnnotations.Schema;

namespace WingTechBot.Database.Models;

///Represents an emote that has a karma value.
public sealed class KarmaEmote(string emote, int amount) : Model
{
	///<summary>Represents a Discord emote in the format of either :thumbsup: or &lt;:name:id&gt;</summary>
	///<remarks>For more details: https://docs.discordnet.dev/guides/emoji/emoji.html?q=Emote</remarks>
	[Key]
	public string Emote { get; private set; } = emote;

	[Required]
	public int Amount { get; private set; } = amount;

	[NotMapped]
	public Emote ParsedEmote => Discord.Emote.Parse(Emote);

	public static async Task<KarmaEmote> AddKarmaEmote(string emote, int amount)
	{
		ValidateEmoteString(emote);
		
		await using BotDbContext context = new();
		KarmaEmote karmaEmote = new(emote, amount);
		await context.KarmaEmotes.AddAsync(karmaEmote);
		await context.SaveChangesAsync();
		return karmaEmote;
	}

	public static async Task RemoveKarmaEmote(string emote)
	{
		ValidateEmoteString(emote);
		
		await using BotDbContext context = new();
		var emoteToRemove = await context.KarmaEmotes.FirstOrDefaultAsync(karmaEmote => karmaEmote.Emote == emote);
		context.KarmaEmotes.Remove(emoteToRemove);
		await context.SaveChangesAsync();
	}

	private static bool ValidateEmoteString(string emote)
	{
		if (!Emoji.TryParse(emote, out _) && !Discord.Emote.TryParse(emote, out _))
			throw new FormatException("Emote string not in valid format");

		return true;
	}
}