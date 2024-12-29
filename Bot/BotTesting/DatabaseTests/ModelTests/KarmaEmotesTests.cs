namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class KarmaEmotesTests : ModelTests
{
	[TestCase("<:upvote:672248776903098369>", 1)]
	[TestCase("<:downvote:672248822474211334>", -1)]
	[TestCase(":thumbsup:", 1)]
	public async Task KarmaEmotes_AddNewEmote(string emote, int amount)
	{
		await using BotDbContext context = new();
		await KarmaEmote.AddKarmaEmote(emote, amount);
		
		Assert.NotNull(await context.KarmaEmotes.FirstOrDefaultAsync(karmaEmote => karmaEmote.Emote == emote));
	}
}