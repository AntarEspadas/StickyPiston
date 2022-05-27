using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyPistonBot;
public class Config
{
    public string? Token { get; set; }
    public string? Prefix { get; set; } = "mc";
    public string? File { get; set; } = "./start.sh";
    public string? Args { get; set; } = null;
    public ulong Channel { get; set; }
    public string? WorkingDir { get; set; } = ".";
}
