﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace WingTechBot;

public static class Program
{
	public static DiscordSocketClient Client { get; private set; }

	public static Config Config { get; private set; }

	public static SocketTextChannel BotChannel { get; private set; }

	public static void Main()
	{
		try
		{
			MainAsync().GetAwaiter().GetResult();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			Console.ReadLine();
			throw;
		}
	}

	private static async Task MainAsync()
	{
		Config = Config.FromJson();

		DiscordSocketConfig config = new() {MessageCacheSize = 100, AlwaysDownloadUsers = true};
		Client = new DiscordSocketClient(config);

		Client.Log += Log;

		Client.Ready += Start;

		await Client.LoginAsync(TokenType.Bot, Config.LoginToken);
		await Client.SetGameAsync(Config.StatusMessage);
		await Client.StartAsync();

		await Task.Delay(Timeout.Infinite);
	}

	private static Task Start()
	{
		BotChannel = Client.GetChannel(Config.BotChannelID) as SocketTextChannel;

		return Task.CompletedTask;
	}

	private static Task Log(LogMessage message)
	{
		Console.WriteLine(message.ToString());
		return Task.CompletedTask;
	}

	public static SocketUser GetUserFromMention(SocketMessage message, string[] arguments, int index = 1)
	{
		var requested = message.Author;
		var parsed = true;

		if (arguments.Length > 1)
		{
			parsed = MentionUtils.TryParseUser(arguments[index], out var id);
			requested = Client.GetUser(id);
		}

		if (requested is null || !parsed)
		{
			message.Channel.SendMessageAsync("User not found.");
			Console.WriteLine("User not found.");
			Console.WriteLine(arguments[index]);
			if (parsed)
			{
				Console.WriteLine(MentionUtils.ParseUser(arguments[index]));
			}
		}

		return requested;
	}
}
