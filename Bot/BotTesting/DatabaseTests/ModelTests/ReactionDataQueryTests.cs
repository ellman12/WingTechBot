namespace BotTesting.DatabaseTests.ModelTests;

///Tests all the data query methods for <see cref="Reaction"/>.
[TestFixture]
public sealed class ReactionDataQueryTests : ModelTests
{
	[TestCase(20, 4, 6)]
	[TestCase(40, 1, 6)]
	[TestCase(69, 0, 0)]
	public async Task GetReactionsUserReceived(int receiverId, int expectedEmotes, int expectedAmountPerEmote)
	{
		await ReactionSeeder.Seed(6, 10, 20, 40);
		var result = await Reaction.GetReactionsUserReceived((ulong) receiverId);
		Assert.AreEqual(result.Length, expectedEmotes);
		Assert.True(result.All(r => r.count == expectedAmountPerEmote));
	}
	
	[TestCase(20, 10, 4, 6)]
	[TestCase(40, 11, 1, 1)]
	[TestCase(69, 420, 0, 0)]
	public async Task GetReactionsUserReceivedFromUser(int receiverId, int giverId, int expectedEmotes, int expectedAmountPerEmote)
	{
		await ReactionSeeder.Seed(6, 10, 20, 40);
		var result = await Reaction.GetReactionsUserReceivedFromUser((ulong) receiverId, (ulong)giverId);
		Assert.AreEqual(result.Length, expectedEmotes);
		Assert.True(result.All(r => r.count == expectedAmountPerEmote));
	}

	[TestCase(3)]
	public async Task GetReactionsAllUsersReceived(int expectedGroups)
	{
		await ReactionSeeder.Seed(6, 10, 20, 40);
		var result = await Reaction.GetReactionsAllUsersReceived();
		Assert.AreEqual(result.Count, expectedGroups);
		Assert.True(result.All(r => r.Value.Length > 0));
	}
}