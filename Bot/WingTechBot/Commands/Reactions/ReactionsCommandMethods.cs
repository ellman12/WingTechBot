namespace WingTechBot.Commands.Reactions;

public sealed partial class ReactionsCommand
{
	private static async Task UserReactionsForYear(SocketSlashCommand command)
	{
		int year = DateTime.Now.Year;
		var reactions = await Reaction.GetReactionsUserReceived(command.User.Id, year);

		string message;
		if (reactions.Length > 0)
		{
			message = reactions.Aggregate($"In {year}, {command.User.Username} has received\n", (current, reaction) => current + $"* {reaction.count} {reaction.reactionEmote.ToString()}\n");
		}
		else
		{
			message = $"No reactions received for {year}";
		}

		await command.FollowupAsync(message);
	}
}