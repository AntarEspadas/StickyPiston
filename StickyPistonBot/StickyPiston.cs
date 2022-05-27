using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using StickyPistonBot.Services;
using System;

namespace StickyPistonBot;

public class StickyPiston
{
    private readonly DiscordSocketClient _client;
    private readonly Config _config;
    private readonly CommandService _commands;
    private readonly CommandHandler _handler;
    private readonly IServiceProvider _services;
    public McConsoleService McConsole { get; }

    public StickyPiston(Config config)
    {
        _config = config;
        var socketConfig = new DiscordSocketConfig();
        _client = new(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.Guilds
        });
        _client.Log += Log;
        _commands = new(new CommandServiceConfig
        {
            DefaultRunMode = RunMode.Async
        });

        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton(_config)
            .AddSingleton<CommandHandler>()
            .AddSingleton<McConsoleService>()
            .BuildServiceProvider();
        
        _handler = _services.GetService<CommandHandler>();
        McConsole = _services.GetService<McConsoleService>();
    }

    public async Task Start()
    {
        await _handler.InstallCommandsAsync();
        await _client.LoginAsync(TokenType.Bot, _config.Token);
        await _client.StartAsync();
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
}
