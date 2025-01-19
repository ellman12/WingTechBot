namespace WingTechBot.Commands;

public sealed class ReactionCommand : SlashCommand
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

	private static async Task UserReactionsForYear(SocketSlashCommand command)
	{
		int year = DateTime.Now.Year;
		var reactions = await Reaction.GetReactionsUserReceived(command.User.Id, year);

		string message;
		if (reactions.Length > 0)
		{
			message = reactions.Aggregate($"{command.User.Username} received\n", (current, reaction) => current + $"* {reaction.count} {reaction.reactionEmote}\n");
			message += $"in {year}";
		}
		else
		{
			message = $"No reactions received for {year}";
		}

		await command.FollowupAsync(message);
	}

	private static async Task UserReactionsFromUserForYear(SocketSlashCommand command, SocketGuildUser giver)
	{
		int year = DateTime.Now.Year;
		var reactions = await Reaction.GetReactionsUserReceivedFromUser(command.User.Id, giver.Id, year);

		string message;
		if (reactions.Length > 0)
		{
			message = reactions.Aggregate($"{command.User.Username} received\n", (current, reaction) => current + $"* {reaction.count} {reaction.reactionEmote}\n");
			message += $"from {giver.Username} in {year}";
		}
		else
		{
			message = $"No reactions received from {giver.Username} for {year}";
		}

		await command.FollowupAsync(message);
	}
	
	private static async Task UserReactionsGivenToUser(SocketSlashCommand command, SocketGuildUser receiver)
	{
		int year = DateTime.Now.Year;
		var reactions = await Reaction.GetReactionsUserGivenToUser(command.User.Id, receiver.Id, year);

		string message;
		if (reactions.Length > 0)
		{
			message = reactions.Aggregate($"{command.User.Username} has given\n", (current, reaction) => current + $"* {reaction.count} {reaction.reactionEmote}\n");
			message += $"to {receiver.Username} in {year}";
		}
		else
		{
			message = $"No reactions given to {receiver.Username} for {year}";
		}

		await command.FollowupAsync(message);
	}
}