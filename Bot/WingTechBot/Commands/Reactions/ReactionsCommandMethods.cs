namespace WingTechBot.Commands.Reactions;

public sealed partial class ReactionsCommand
{
	//TODO: try to find a way to reduce repetition with these. Perhaps a helper method that takes in the method to run, the string to print when length > 0, and the string for when it == 0.
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