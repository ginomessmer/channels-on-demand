# VCOD - Voice Channels on Demand
[![.NET Build and Test](https://github.com/ginomessmer/discord-vcaas/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ginomessmer/discord-vcaas/actions/workflows/dotnet.yml)
[![Docker Build](https://github.com/ginomessmer/discord-vcod/actions/workflows/docker.yml/badge.svg)](https://github.com/ginomessmer/discord-vcod/actions/workflows/docker.yml)
[![Docker Pulls](https://img.shields.io/docker/pulls/ginomessmer/discord-vcod?logo=docker)](https://hub.docker.com/r/ginomessmer/discord-vcod)
[![ko-fi](https://img.shields.io/badge/%E2%98%95-buy%20me%20a%20coffee-orange)](https://ko-fi.com/P5P72WHKK)

**A lightweight Discord bot that creates voice channels for you on demand, whenever you need them.**

![Demo](./assets/demo.gif)

## Features
- [x] Creates channels
- [x] Purges empty channels
- [x] Multi-server support
- [x] Let's you use pre-defined voice channel names chosen at random

## Pending Work
- [x] Use a relational database to improve performance (EF Core)
- [ ] Use a logger provider (such as Serilog)
- [ ] Help command
- [ ] More tests

---

## Run
1. Create a new Discord application
2. Create a bot registration for your application
3. Invite the bot to your server
   - Required OAuth 2 scopes: `Bot`, `Manage channels`, `Move users`
4. Copy the Discord bot token

### Docker
5. Run: `docker run ginomessmer/discord-vcod -e ConnectionStrings:DiscordBotToken=<TOKEN>`

6. Register your first lobby. See Commands below for instructions.

## Commands
|Command|Description|
|---|---|
|`.lobby register <VOICE_CHANNEL_ID> [category_id]`|Registers the channel as a lobby. The `category_id` is optional and will be used as the parent for all rooms in this lobby.|
|`.lobby deregister <VOICE_CHANNEL_ID>`|Deregisters the lobby.|
|`.lobby list`|Lists all lobbies on the server.|
|`.lobby set names [NAME] <names...>`|Sets the possible names for rooms. The names will be chosen randomly if more than one name exists.|

---

## Build
- Uses .NET 5.
- Built using Visual Studio 2019 (.NET Workload enabled). VS Code works too.
- Run the bot like any other .NET application. Make sure to provide a bot token in the secrets file.

## Contribute
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/P5P72WHKK)
### Bugs and Fixes
You found a bug? Awesome! Head over to the GitHub issues and create a new issue to discuss possible solutions. If you have a solution in mind already, feel free to fork the project and create a new pull request. Remember to create a new branch and set the destination branch to the main branch.

## Suggestions
Do you have any ideas in mind? Great, go ahead. Navigate to the Discussions tab and create a new discussion. I'd love to hear what you have in mind. And if you're feeling adventurous, feel free to create a fork.
