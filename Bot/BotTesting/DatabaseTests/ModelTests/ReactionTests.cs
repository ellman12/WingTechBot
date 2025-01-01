namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class ReactionTests : ModelTests
{
	[TestCase(123ul, 456ul, 789ul, "upvote", 8947589432758943ul)]
	public async Task Reaction_AddReaction_ReactionEmoteDoesNotExist(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong discordEmoteId)
	{
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);
		
		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);
	}

	[TestCase(123ul, 456ul, 789ul, "upvote", 8947589432758943ul)]
	public async Task Reaction_AddReaction_ReactionEmoteExists(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong discordEmoteId)
	{
		await ReactionEmote.AddEmote(emoteName, discordEmoteId);
		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);
		
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);
	}

	[TestCase(123ul, 456ul, 789ul, "upvote", 8947589432758943ul)]
	public async Task Reaction_AddReaction_ReactionExists(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong discordEmoteId)
	{
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
		
		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);
		
		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);

		Assert.ThrowsAsync<ArgumentException>(async () => await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId));
	}
}