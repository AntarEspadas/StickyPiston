using StickyPistonBot;
using System.Text.Json;


//proc.WaitForExit();
var json = File.ReadAllText("./StickyPistonConfig.json");
var config = JsonSerializer.Deserialize<Config>(json) ?? new();
var bot = new StickyPiston(config);
await bot.Start();
while (true)
{
    var input = Console.ReadLine();
    if (input is null)
        continue;
    var result = bot.McConsole.HandleCommand(input);
    if (result is not null)
        Console.WriteLine(result);
}
