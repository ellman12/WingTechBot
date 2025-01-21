using WingTechBot.Commands.Reactions;

namespace WingTechBot;

public sealed class WingTechBot
{
	public DiscordSocketClient Client { get; } = new(DiscordConfig);

	public Config Config { get; private set; }

	public SocketTextChannel BotChannel { get; private set; }

	public static readonly DiscordSocketConfig DiscordConfig = new() {MessageCacheSize = 100, AlwaysDownloadUsers = true};

	private WingTechBot() {}

	private readonly ReactionTracker reactionTracker = new();
	private readonly ReactionsCommand reactionsCommand = new();

	public static async Task<WingTechBot> Create(string configPath = null)
	{
		WingTechBot bot = new();
		bot.Config = String.IsNullOrWhiteSpace(configPath) ? Config.FromJson() : Config.FromJson(configPath);

		bot.Client.Log += Logger.LogLine;
		bot.Client.Ready += bot.OnClientReady;

		await bot.Client.LoginAsync(TokenType.Bot, bot.Config.LoginToken);
		await bot.Client.SetCustomStatusAsync(bot.Config.StatusMessage);
		await bot.Client.StartAsync();

		bot.reactionTracker.SetUp(bot);

		return bot;
	}

	private async Task OnClientReady()
	{
		BotChannel = Client.GetChannel(Config.BotChannelId) as SocketTextChannel;

		if (BotChannel == null)
			throw new NullReferenceException("Could not find bot channel");

		await SetUpCommands();

		// await BotChannel.SendMessageAsync("Bot started and ready");
	}
	
	private async Task SetUpCommands()
	{
		await ClearCommands();

		await reactionsCommand.SetUp(this);
	}
	
	///Removes all slash commands from the bot. However, because Discord is terrible this is unreliable and often does nothing.
	private async Task ClearCommands()
	{
		var guild = Client.GetGuild(Config.ServerId);
		await guild.DeleteApplicationCommandsAsync();
	}
}