namespace WingTechBot.Commands.Reactions;

public sealed partial class ReactionsCommand : SlashCommand
{
	public override async Task SetUp(WingTechBot bot)
	{
		Bot = bot;
		Bot.Client.SlashCommandExecuted += HandleCommand;

		var reactionsCommand = new SlashCommandBuilder()
			.WithName("reactions")
			.WithDescription("Shows totals for all reactions you have received this year")
			.AddOption(new SlashCommandOptionBuilder()
				.WithName("from")
				.WithDescription("Get reactions received from a user this year")
				.WithType(ApplicationCommandOptionType.Mentionable)
			)
			.AddOption(new SlashCommandOptionBuilder()
				.WithName("given-to")
				.WithDescription("Get reactions given to a user this year")
				.WithType(ApplicationCommandOptionType.Mentionable)
			)
		;

		try
		{
			await Bot.Client.CreateGlobalApplicationCommandAsync(reactionsCommand.Build());
		}
		catch (Exception e)
		{
			Logger.LogLine("Error adding reactions command");
			Logger.LogException(e);
		}
	}

	public override async Task HandleCommand(SocketSlashCommand command)
	{
		await PreprocessCommand(command);

		if (command.CommandName != "reactions")
			return;

		var options = command.Data.Options;

		if (options.Count == 0)
		{
			await UserReactionsForYear(command);
			return;
		}

		var first = options.First();
		if (first.Name == "from" && first.Value is SocketGuildUser giver)
		{
			await UserReactionsFromUserForYear(command, giver);
		}
		else if (first.Name == "given-to" && first.Value is SocketGuildUser receiver)
		{
			await UserReactionsGivenToUser(command, receiver);
		}
	}

}