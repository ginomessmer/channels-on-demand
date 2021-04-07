using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiscordChannelsOnDemand.Bot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    GuildId = table.Column<string>(type: "text", nullable: false),
                    SpaceConfiguration_SpaceCategoryId = table.Column<string>(type: "text", nullable: true),
                    SpaceConfiguration_SpaceTimeoutThreshold = table.Column<TimeSpan>(type: "interval", nullable: true),
                    SpaceConfiguration_SpaceDefaultNames = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Lobby",
                columns: table => new
                {
                    TriggerVoiceChannelId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    RoomNames = table.Column<string>(type: "text", nullable: true),
                    AutoCreateSpace = table.Column<bool>(type: "boolean", nullable: false),
                    ServerGuildId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobby", x => x.TriggerVoiceChannelId);
                    table.ForeignKey(
                        name: "FK_Lobby_Servers_ServerGuildId",
                        column: x => x.ServerGuildId,
                        principalTable: "Servers",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Spaces",
                columns: table => new
                {
                    TextChannelId = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spaces", x => x.TextChannelId);
                    table.ForeignKey(
                        name: "FK_Spaces_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ChannelId = table.Column<string>(type: "text", nullable: false),
                    HostUserId = table.Column<string>(type: "text", nullable: true),
                    GuildId = table.Column<string>(type: "text", nullable: true),
                    LinkedSpaceTextChannelId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ChannelId);
                    table.ForeignKey(
                        name: "FK_Rooms_Spaces_LinkedSpaceTextChannelId",
                        column: x => x.LinkedSpaceTextChannelId,
                        principalTable: "Spaces",
                        principalColumn: "TextChannelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_ServerGuildId",
                table: "Lobby",
                column: "ServerGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LinkedSpaceTextChannelId",
                table: "Rooms",
                column: "LinkedSpaceTextChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_ServerId",
                table: "Spaces",
                column: "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lobby");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Spaces");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
