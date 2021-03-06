# COD - Channels on Demand for Discord
[![.NET Build and Test](https://github.com/ginomessmer/discord-vcaas/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ginomessmer/discord-vcaas/actions/workflows/dotnet.yml)
[![Docker Build](https://github.com/ginomessmer/channels-on-demand/actions/workflows/docker.yml/badge.svg)](https://github.com/ginomessmer/channels-on-demand/actions/workflows/docker.yml)
[![Docker Pulls](https://img.shields.io/docker/pulls/ginomessmer/discord-cod?logo=docker)](https://hub.docker.com/r/ginomessmer/discord-cod)
[![ko-fi](https://img.shields.io/badge/%E2%98%95-buy%20me%20a%20coffee-orange)](https://ko-fi.com/P5P72WHKK)

**A lightweight Discord bot that creates voice and text channels for you on demand, whenever you need them.**

![Demo](./assets/demo.gif)

## Features
- [x] Creates temporary voice and text channels
- [x] Purges empty channels
- [x] Multi-server support
- [x] Let's you use pre-defined voice channel names chosen at random

## Pending Work
- [x] Use a relational database to improve performance (EF Core)
- [ ] Use a logger provider (such as Serilog)
- [x] Help command
- [ ] More tests

---

## Run
1. Create a new Discord application
2. Create a bot registration for your application
3. Invite the bot to your server
   - Required OAuth 2 scopes: `Bot`, `Manage channels`, `Move users`
4. Copy the Discord bot token

### Docker
5. Run: `docker run ginomessmer/discord-cod -e ConnectionStrings:DiscordBotToken=<TOKEN>`

6. Register your first lobby. See Commands below for instructions.

## Commands
|Command|Description|
|---|---|
|`.lobby register <VOICE_CHANNEL_ID> [category_id]`|Registers the channel as a lobby. The `category_id` is optional and will be used as the parent for all rooms in this lobby.|
|`.lobby deregister <VOICE_CHANNEL_ID>`|Deregisters the lobby.|
|`.lobby list`|Lists all lobbies on the server.|
|`.lobby set names [NAME] <names...>`|Sets the possible names for rooms. The names will be chosen randomly if more than one name exists.|
|`.lobby set space autocreate <VOICE_CHANNEL_ID> <true\|false>`|Auto creates spaces when users join the lobby|
|`.space enable`|Enables spaces for the entire server.|
|`.space disable`|Disables spaces for the entire server.|
|`.space new [USERS...]`|Creates a new space and invites all mentioned users.|

---

## Build
- Uses .NET 5.
- Built using Visual Studio 2019 (.NET Workload enabled). VS Code works too.
- Run the bot like any other .NET application. Make sure to provide a bot token in the secrets file.

## Contribute
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P72WHKK)
### Bugs and Fixes
You found a bug? Head over to the issues tab and create a new issue. If you have a solution in mind already, feel free to fork the project and create a new pull request. Remember to create a new branch and set the destination branch to the `dev` branch.

## Suggestions
Do you have any ideas in mind? Great, go ahead. Navigate to the discussions tab and start one. I'd love to hear what you have in mind. And if you're feeling adventurous, feel free to create a fork.
