using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arekat.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMultidifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Badge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgSrc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseUser",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Avator = table.Column<byte[]>(type: "varbinary(max)", maxLength: 16384, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Intro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ChartDesignerLevel = table.Column<int>(type: "int", nullable: false),
                    AdminLevel = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUser", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerifyKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestClass = table.Column<int>(type: "int", nullable: false),
                    QuestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiverEmail = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifyKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Bpm = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreUids",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreUids", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "BadgeBaseUser",
                columns: table => new
                {
                    BadgesId = table.Column<int>(type: "int", nullable: false),
                    OwnersGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeBaseUser", x => new { x.BadgesId, x.OwnersGuid });
                    table.ForeignKey(
                        name: "FK_BadgeBaseUser_Badge_BadgesId",
                        column: x => x.BadgesId,
                        principalTable: "Badge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeBaseUser_BaseUser_OwnersGuid",
                        column: x => x.OwnersGuid,
                        principalTable: "BaseUser",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReadTime = table.Column<int>(type: "int", nullable: false),
                    ArthorId = table.Column<int>(type: "int", nullable: false),
                    ArthorGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HtmlText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notices_BaseUser_ArthorGuid",
                        column: x => x.ArthorGuid,
                        principalTable: "BaseUser",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistSong",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    SongsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistSong", x => new { x.ArtistId, x.SongsId });
                    table.ForeignKey(
                        name: "FK_ArtistSong_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistSong_Songs_SongsId",
                        column: x => x.SongsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    IndexChartId = table.Column<int>(type: "int", nullable: false),
                    Game = table.Column<int>(type: "int", nullable: false),
                    RatingClass = table.Column<int>(type: "int", nullable: false),
                    Diffifulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DownloadTime = table.Column<int>(type: "int", nullable: false),
                    IsStable = table.Column<bool>(type: "bit", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charts_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChart",
                columns: table => new
                {
                    ChartsId = table.Column<int>(type: "int", nullable: false),
                    DesignerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChart", x => new { x.ChartsId, x.DesignerGuid });
                    table.ForeignKey(
                        name: "FK_BaseUserChart_BaseUser_DesignerGuid",
                        column: x => x.DesignerGuid,
                        principalTable: "BaseUser",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChart_Charts_ChartsId",
                        column: x => x.ChartsId,
                        principalTable: "Charts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartId = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartsHistory_Charts_ChartId",
                        column: x => x.ChartId,
                        principalTable: "Charts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_Id",
                table: "Artists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_Name",
                table: "Artists",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSong_SongsId",
                table: "ArtistSong",
                column: "SongsId");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_Id",
                table: "Badge",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_Name",
                table: "Badge",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BadgeBaseUser_OwnersGuid",
                table: "BadgeBaseUser",
                column: "OwnersGuid");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_Id",
                table: "BaseUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_Name",
                table: "BaseUser",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChart_DesignerGuid",
                table: "BaseUserChart",
                column: "DesignerGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_Id",
                table: "Charts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_SongId",
                table: "Charts",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartsHistory_ChartId",
                table: "ChartsHistory",
                column: "ChartId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartsHistory_Id",
                table: "ChartsHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notices_ArthorGuid",
                table: "Notices",
                column: "ArthorGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_Title",
                table: "Songs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_StoreUids_Id",
                table: "StoreUids",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistSong");

            migrationBuilder.DropTable(
                name: "BadgeBaseUser");

            migrationBuilder.DropTable(
                name: "BaseUserChart");

            migrationBuilder.DropTable(
                name: "ChartsHistory");

            migrationBuilder.DropTable(
                name: "EmailVerifyKeys");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "StoreUids");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Badge");

            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.DropTable(
                name: "BaseUser");

            migrationBuilder.DropTable(
                name: "Songs");
        }
    }
}
