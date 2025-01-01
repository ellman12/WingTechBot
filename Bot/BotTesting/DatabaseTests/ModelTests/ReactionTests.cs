namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class ReactionTests : ModelTests
{
	[TestCase(123ul, 456ul, 789ul, 1)]
	public async Task Reaction_AddReaction(ulong giverId, ulong receiverId, ulong messageId, int emoteId)
	{
		await ReactionEmote.AddEmote("upvote", 147589435889);
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteId);

		var found = await Reaction.Find(giverId, receiverId, messageId, emoteId);
		Assert.NotNull(found);
		Assert.NotNull(found.ReactionEmote);
	}
}