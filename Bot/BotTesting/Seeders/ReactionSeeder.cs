namespace BotTesting.Seeders;

///Seeds the Reactions table with values for testing.
public static class ReactionSeeder
{
	private static readonly ReactionEmote[] emotes =
	[
		new("upvote", 123456, 1),
		new("downvote", 456789, -1),
		new("thumbsup", null),
		new("eyes", null)
	];

	public static async Task Seed(int amount, ulong startGiverId, ulong startReceiverId, ulong startMessageId)
	{
		ulong giverId = startGiverId;
		ulong receiverId = startReceiverId;
		ulong messageId = startMessageId;

		//Same user giving multiple reactions to same message.
		foreach (int i in Enumerable.Range(1, amount))
		{
			foreach (var emote in emotes)
			{
				await Reaction.AddReaction(giverId, receiverId, messageId, emote.Name, emote.DiscordEmoteId);
			}

			messageId *= 2;
		}

		//Multiple users giving the same reaction to the same message.
		receiverId *= 2;
		foreach (int i in Enumerable.Range(1, amount))
		{
			giverId++;

			var emote = emotes.First();
			await Reaction.AddReaction(giverId, receiverId, messageId, emote.Name, emote.DiscordEmoteId);
		}

		messageId *= 2;
	}
}