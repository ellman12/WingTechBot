namespace BotTesting.DatabaseTests.ModelTests;

public sealed class ReactionTests : ModelTests
{
	[TestCase(123u, 123u, 123u, "<:downvote:672248822474211334>")]
	[TestCase(123u, 456u, 789u, "<:upvote:672248776903098369>")]
	public async Task Reaction_AddReaction(ulong giverId, ulong receiverId, ulong messageId, string emote)
	{
		await using BotDbContext context = new();
		await Reaction.AddReaction(giverId, receiverId, messageId, emote);
		var newReaction = Reaction.Find(giverId, receiverId, messageId, emote);
		Assert.NotNull(newReaction);
	}
	
	[TestCase(123u, 123u, 123u, "<:downvote:672248822474211334>")]
	[TestCase(123u, 456u, 789u, "<:upvote:672248776903098369>")]
	public async Task Reaction_AddReaction_ErrorOnDuplicateValues(ulong giverId, ulong receiverId, ulong messageId, string emote)
	{
		await using BotDbContext context = new();
		await Reaction.AddReaction(giverId, receiverId, messageId, emote);
		await Task.Delay(200);
		var newReaction = await Reaction.Find(giverId, receiverId, messageId, emote);
		Assert.NotNull(newReaction);
		
		Assert.ThrowsAsync<ArgumentException>(async () => await Reaction.AddReaction(giverId, receiverId, messageId, emote));
	}
}