namespace BotTesting.DatabaseTests.ModelTests;

[TestFixture]
public sealed class KarmaEmotesTests : ModelTests
{
	[TestCase("<:upvote:672248776903098369>", 1)]
	[TestCase("<:downvote:672248822474211334>", -1)]
	[TestCase(":thumbsup:", 1)]
	public async Task KarmaEmotes_AddNewEmote(string emote, int amount)
	{
		await CreateKarmaEmote(emote, amount);
	}

	[TestCase("<:upvote:672248776903098369>")]
	[TestCase("<:downvote:672248822474211334>")]
	[TestCase(":thumbsup:")]
	public async Task KarmaEmotes_RemoveEmote(string emote)
	{
		await using BotDbContext context = new();
		await CreateKarmaEmote(emote, 0);

		await KarmaEmote.RemoveKarmaEmote(emote);
		Assert.IsNull(await context.KarmaEmotes.FirstOrDefaultAsync(karmaEmote => karmaEmote.Emote == emote));
	}

	[TestCase(""), TestCase("cheese"), TestCase("69420")]
	public void KarmaEmotes_AddNewEmote_InvalidString(string invalidEmote)
	{
		Assert.ThrowsAsync<FormatException>(async () => await KarmaEmote.AddKarmaEmote(invalidEmote, 0));
	}
	
	[TestCase(""), TestCase("cheese"), TestCase("69420")]
	public void KarmaEmotes_RemoveEmote_InvalidString(string invalidEmote)
	{
		Assert.ThrowsAsync<FormatException>(async () => await KarmaEmote.RemoveKarmaEmote(invalidEmote));
	}

	private static async Task<KarmaEmote> CreateKarmaEmote(string emote, int amount)
	{
		await using BotDbContext context = new();
		await KarmaEmote.AddKarmaEmote(emote, amount);
		var newEmote = await context.KarmaEmotes.FirstOrDefaultAsync(karmaEmote => karmaEmote.Emote == emote);
		Assert.NotNull(newEmote);
		return newEmote;
	}
}