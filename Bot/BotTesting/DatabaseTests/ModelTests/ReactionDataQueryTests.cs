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

	[TestCase(3)]
	public async Task GetReactionsAllUsersReceived(int expectedGroups)
	{
		await ReactionSeeder.Seed(6, 10, 20, 40);
		var result = await Reaction.GetReactionsAllUsersReceived();
		Assert.AreEqual(result.Count, expectedGroups);
		Assert.True(result.All(r => r.Value.Length > 0));
	}
}