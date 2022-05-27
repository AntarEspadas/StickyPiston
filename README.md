# Sticky Piston Bot

## A bot that turns a Discord text channel into a Minecraft: Java Edition server terminal

This bot sends all output from a Minecraft server to a Discord channel and interprets all messages sent to that channel as Minecraft commands

### Setup

The following steps assume you already have a working Minecraft server

 * If you don't have one yet, [create a Discord bot application](https://discord.com/developers/docs/getting-started#creating-an-app) and invite (install) it to your server
 * In your server, create the text channel where the bot is going to receive Minecraft commands and log the server output (It is recommended that you mute this channel)
 * Download the appropriate executable file from the [releases page](https://github.com/Naratna/StickyPiston/releases) of this repo along with the `StickyPistonConfig.json` file
 * Place both files in **the same directory** as your `server.jar` file
 * Edit the `StickyPistonConfig.json` file with the following information:
     * **Your bot's token.**
     * **The id of the Discord text channel**. You can get it by enabling Discord's `Developer Mode` under `Advanced` settings, then right-clicking the channel and selecting `Copy ID`

 * Your `StickyPistonConfig.json` file should look something like this:

 ```json
 {
  "Token": "8sgpz5SeRs2xmS2pQ.pqQ.Qs-mDgAqw5LztJsYu3DF8cCcRTSCN-a3zjfVx",
  "Channel": 6871900342856128304,
  "File": "java",
  "Args": "-jar server.jar -nogui",
  "WorkingDir": "."
}
 ```

### Usage

* Once the setup is complete, make sure the Minecraft server is **NOT** running, then run the `StickyPistonBot` executable file
* After the program prints out `Gateway Ready` to the terminal, you can start typing commands, either directly in the terminal or through the Discord channel that you created
* Type `start` to start the server. After a while, you may begin typing regular Minecraft commands. `start` is a special keyword that is not a standard Minecraft command and can only be used when the server is not running
* Another special keyword is `forcestop`, this will forcefully terminate the server process and is useful in case the server becomes unresponsive