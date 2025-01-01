namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class ReactionEmoteTests : ModelTests
{
	[TestCase("upvote", 123456ul)]
	[TestCase("downvote", 87589ul)]
	[TestCase("eyes", null)]
	public async Task ReactionEmote_AddEmote(string name, ulong? emoteId)
	{
		await ReactionEmote.AddEmote(name, emoteId);
		Assert.NotNull(await ReactionEmote.FindByName(name));
	}
	
	[TestCase("upvote", 0u), TestCase("", 87589u)]
	public void ReactionEmote_AddEmote_InvalidParams(string name, ulong emoteId)
	{
		Assert.ThrowsAsync<ArgumentException>(async () => await ReactionEmote.AddEmote(name, emoteId));
	}
}