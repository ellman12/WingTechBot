namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class ReactionEmoteTests : ModelTests
{
	[TestCase("upvote", 123456ul, 1)]
	[TestCase("downvote", 87589ul, -1)]
	[TestCase("eyes", null, 0)]
	public async Task ReactionEmote_SetKarmaValue(string name, ulong? emoteId, int newValue)
	{
		await using BotDbContext context = new();
		var emote = await ReactionEmote.AddEmote(name, emoteId);
		Assert.NotNull(await ReactionEmote.Find(name, emoteId));

		await emote.SetKarmaValue(newValue);
		Assert.True(emote.KarmaValue == newValue);
	}

	[TestCase("upvote", 123ul, "<:upvote:123>")]
	[TestCase("eyes", null, "👀")]
	public void ReactionEmote_VerifyParsedValue(string name, ulong? emoteId, string expected)
	{
		ReactionEmote emote = new(name, emoteId);
		Assert.True(emote.Parsed.ToString() == expected);
	}

	[TestCase("upvote", 123456ul)]
	[TestCase("downvote", 87589ul)]
	[TestCase("eyes", null)]
	public async Task ReactionEmote_AddEmote(string name, ulong? emoteId)
	{
		await ReactionEmote.AddEmote(name, emoteId);
		Assert.NotNull(await ReactionEmote.Find(name, emoteId));
	}

	[TestCase("upvote", 0u), TestCase("", 87589u)]
	public void ReactionEmote_AddEmote_InvalidParams(string name, ulong emoteId)
	{
		Assert.ThrowsAsync<ArgumentException>(async () => await ReactionEmote.AddEmote(name, emoteId));
	}

	[TestCase("upvote", 123456ul, 123ul, 456ul, 789ul)]
	public async Task ReactionEmote_FindReferencedReactions(string emoteName, ulong emoteId, ulong giverId, ulong receiverId, ulong messageId)
	{
		const int ReactionEmoteRowId = 1;

		await ReactionEmote.AddEmote(emoteName, emoteId);
		var newEmote = await ReactionEmote.Find(emoteName, emoteId);
		Assert.NotNull(newEmote);

		foreach (int i in Enumerable.Range(1, 4))
		{
			messageId++;
			await Reaction.AddReaction(giverId, receiverId, messageId, ReactionEmoteRowId);
			newEmote = await ReactionEmote.Find(emoteName, emoteId); //Refresh its data.
			Assert.True(newEmote != null && newEmote.Reactions.Count == i);
		}
	}
}