using WingTechBot.Commands.Reaction;

namespace WingTechBot;

public sealed class WingTechBot
{
	public DiscordSocketClient Client { get; } = new(DiscordConfig);

	public Config Config { get; } = Config.FromJson();

	public SocketTextChannel BotChannel { get; private set; }

	public static readonly DiscordSocketConfig DiscordConfig = new() {MessageCacheSize = 100, AlwaysDownloadUsers = true};

	private WingTechBot() {}

	private readonly ReactionCommand reactionCommand = new();
	
	public static async Task<WingTechBot> Create()
	{
		WingTechBot bot = new();
		bot.Client.Log += Logger.LogLine;
		bot.Client.Ready += bot.OnClientReady;

		await bot.Client.LoginAsync(TokenType.Bot, bot.Config.LoginToken);
		await bot.Client.SetCustomStatusAsync(bot.Config.StatusMessage);
		await bot.Client.StartAsync();

		return bot;
	}

	private async Task OnClientReady()
	{
		BotChannel = Client.GetChannel(Config.BotChannelId) as SocketTextChannel;

		if (BotChannel == null)
			throw new NullReferenceException("Could not find bot channel");

		await SetUpCommands();

		await BotChannel.SendMessageAsync("Bot started and ready");
	}

	private async Task SetUpCommands()
	{
		await ClearCommands();

		await reactionCommand.SetUp(this);
	}
	
	///Removes all slash commands from the bot. However, because Discord is terrible this is unreliable and often does nothing.
	private async Task ClearCommands()
	{
		var guild = Client.GetGuild(Config.ServerId);
		await guild.DeleteApplicationCommandsAsync();
	}
}