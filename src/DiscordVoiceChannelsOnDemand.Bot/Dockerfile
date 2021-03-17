#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DiscordVoiceChannelsOnDemand.Bot/DiscordVoiceChannelsOnDemand.Bot.csproj", "DiscordVoiceChannelsOnDemand.Bot/"]
RUN dotnet restore "DiscordVoiceChannelsOnDemand.Bot/DiscordVoiceChannelsOnDemand.Bot.csproj"
COPY . .
WORKDIR "/src/DiscordVoiceChannelsOnDemand.Bot"
RUN dotnet build "DiscordVoiceChannelsOnDemand.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordVoiceChannelsOnDemand.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordVoiceChannelsOnDemand.Bot.dll"]