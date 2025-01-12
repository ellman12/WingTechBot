namespace WingTechBot.Database.Models;

public sealed partial class Reaction
{
	///Gets each reaction a user has received and the amount of each.
	public static async Task<(ReactionEmote reactionEmote, int count)[]> GetReactionsUserReceived(ulong receiverId, int? year = null)
	{
		await using BotDbContext context = new();
		return context.Reactions
			.Include(r => r.Emote)
			.Where(r => r.ReceiverId == receiverId && (year == null ? r.Emote.CreatedAt.Year > 0 : r.Emote.CreatedAt.Year == year))
			.GroupBy(r => r.EmoteId)
			.AsEnumerable()
			.Select(g => (g.First().Emote, g.Count()))
			.ToArray();
	}

	///Gets all reactions every user has received and the amount of each.
	public static async Task<Dictionary<ulong, (ReactionEmote emote, int count)[]>> GetReactionsAllUsersReceived()
	{
		await using BotDbContext context = new();
		return context.Reactions
			.Include(r => r.Emote)
			.GroupBy(r => r.ReceiverId)
			.AsEnumerable()
			.ToDictionary(idGroup => idGroup.Key, idGroup => idGroup
				.GroupBy(r => r.Emote)
				.Select(emoteGroup => (emoteGroup.Key, emoteGroup.Count()))
				.ToArray()
			);
	}
}