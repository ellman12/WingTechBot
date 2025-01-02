namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class ReactionTests : ModelTests
{
	public static IEnumerable<TestCaseData> TestCases()
	{
		yield return new TestCaseData(123ul, 456ul, 789ul, "upvote", 8947589432758943ul);
		yield return new TestCaseData(123ul, 456ul, 789ul, "eyes", null);
	}

	#region AddReaction
	[Test, TestCaseSource(nameof(TestCases))]
	public async Task Reaction_AddReaction_ReactionEmoteDoesNotExist(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong? discordEmoteId)
	{
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);

		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);
		
		Assert.True(reaction.Emote.Name == emote.Name);
	}

	[Test, TestCaseSource(nameof(TestCases))]
	public async Task Reaction_AddReaction_ReactionEmoteExists(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong? discordEmoteId)
	{
		await ReactionEmote.AddEmote(emoteName, discordEmoteId);
		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);

		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);
		
		Assert.True(reaction.Emote.Name == emote.Name);
	}

	[Test, TestCaseSource(nameof(TestCases))]
	public async Task Reaction_AddReaction_ReactionExists(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong? discordEmoteId)
	{
		await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);

		var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
		Assert.NotNull(emote);

		var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
		Assert.NotNull(reaction);

		Assert.ThrowsAsync<ArgumentException>(async () => await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId));
	}
	#endregion

	#region RemoveReaction
	[Test, TestCaseSource(nameof(TestCases))]
	public async Task Reaction_RemoveReaction_ReactionExists(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong? discordEmoteId)
	{
		foreach (int i in Enumerable.Range(1, 4))
		{
			messageId++;
			await Reaction.AddReaction(giverId, receiverId, messageId, emoteName, discordEmoteId);
			var emote = await ReactionEmote.Find(emoteName, discordEmoteId);
			var reaction = await Reaction.Find(giverId, receiverId, messageId, emote.Id);
			Assert.NotNull(emote);
			Assert.NotNull(reaction);
		}

		await using BotDbContext context = new();
		Assert.True(context.Reactions.Count() == 4);
		Assert.True(context.Reactions.OrderBy(r => r.MessageId).Last().MessageId == messageId);

		await Reaction.RemoveReaction(giverId, messageId, emoteName, discordEmoteId);
		Assert.True(context.Reactions.Count() == 3);
		Assert.True(context.Reactions.OrderBy(r => r.MessageId).Last().MessageId == messageId - 1);
	}

	[Test, TestCaseSource(nameof(TestCases))]
	public async Task Reaction_RemoveReaction_ReactionDoesNotExist(ulong giverId, ulong receiverId, ulong messageId, string emoteName, ulong? discordEmoteId)
	{
		await using BotDbContext context = new();
		Assert.IsEmpty(context.Reactions);

		Assert.ThrowsAsync<ArgumentException>(async () => await Reaction.RemoveReaction(giverId, messageId, emoteName, discordEmoteId));
	}
	#endregion
}