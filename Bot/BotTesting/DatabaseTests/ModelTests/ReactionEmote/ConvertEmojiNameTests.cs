namespace BotTesting.DatabaseTests.ModelTests.ReactionEmote;
using ReactionEmote = WingTechBot.Database.Models.ReactionEmote;

public sealed class ConvertEmojiNameTests
{
	[TestCase(":eyes:", "👀")]
	[TestCase("👍", "👍")]
	[TestCase(":ok_hand_tone2:", "👌🏼")]
	[TestCase("🙋🏼‍♂️", "🙋🏼‍♂️")]
	public void ConvertEmojiName(string name, string expected)
	{
		name = ReactionEmote.ConvertEmojiName(name);
		Assert.AreEqual(name, expected);
	}
}