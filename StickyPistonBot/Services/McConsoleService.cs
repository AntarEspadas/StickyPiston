using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Text;

namespace StickyPistonBot.Services;

public class McConsoleService
{

    private readonly ProcessStartInfo _processStartInfo;
    private readonly ulong _channelId;
    private readonly DiscordSocketClient _client;
    private readonly System.Timers.Timer _timer = new();
    private readonly StringBuilder _outputBuilder = new();
    private ISocketMessageChannel? _channel;
    private Process? _process;

    public McConsoleService(Config config, DiscordSocketClient client)
    {
        _processStartInfo = new()
        {
            FileName = config.File,
            Arguments = config.Args,
            WorkingDirectory = config.WorkingDir,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };
        _channelId = config.Channel;
        _client = client;

        _timer.Interval = 2000;
        _timer.Elapsed += SendOutput;

        client.MessageReceived += HandleMessageReceived;
    }

    private void SendOutput(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (_outputBuilder.Length == 0)
            return;
        var output = _outputBuilder.ToString();
        _outputBuilder.Clear();
        if (string.IsNullOrWhiteSpace(output))
            return;
        _channel?.SendMessageAsync($"```accesslog\n{output}```");
    }

    private async Task HandleMessageReceived(SocketMessage message)
    {
        if (message.Channel.Id != _channelId)
            return;
        if (message.Author.IsBot)
            return;
        var result = HandleCommand(message.Content);
        if (result is null)
            return;
        await message.Channel.SendMessageAsync(result);
    }

    public string? HandleCommand (string command)
    {
        switch (command.ToLower())
        {
            case "start":
                return Start();
            case "forcestop":
            case "force stop":
                return ForceStop();
            default:
                return Send(command);
        }
    }

    public string Start()
    {
        if (_process is not null && !_process.HasExited)
            return "Server already started";

        _channel = _client.GetChannelAsync(_channelId).Result as ISocketMessageChannel;
        _timer.Start();
        _process = new() { StartInfo = _processStartInfo};
        _process.Start();
        _process.OutputDataReceived += handleOutputReceived;
        _process.BeginOutputReadLine();
        return "Starting server...";
    }

    private void handleOutputReceived(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
        _outputBuilder.AppendLine(e.Data);
    }

    public string? Send(string? command)
    {
        if (_process is null || _process.HasExited)
            return "Server not running";

        if (string.IsNullOrEmpty(command))
            return null;

        _process.StandardInput.WriteLine(command);
        return null;
    }

    public string ForceStop()
    {
        if (_process is null || _process.HasExited)
            return "Server not running";
        _process.Kill();
        return "Forcefully stopped server";
    }
}
