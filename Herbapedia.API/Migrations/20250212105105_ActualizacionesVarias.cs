using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Herbapedia.API.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionesVarias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlantTypes",
                columns: table => new
                {
                    PlantTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantTypeName = table.Column<string>(type: "text", nullable: true),
                    PlantTypeDescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantTypes", x => x.PlantTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleType = table.Column<string>(type: "text", nullable: true),
                    RoleDescription = table.Column<string>(type: "text", nullable: true),
                    HomePage = table.Column<int>(type: "integer", nullable: false),
                    PlantPage = table.Column<int>(type: "integer", nullable: false),
                    LoginPage = table.Column<int>(type: "integer", nullable: false),
                    UnregisterPage = table.Column<int>(type: "integer", nullable: false),
                    UserPage = table.Column<int>(type: "integer", nullable: false),
                    AdminPage = table.Column<int>(type: "integer", nullable: false),
                    PostPage = table.Column<int>(type: "integer", nullable: false),
                    ChatPage = table.Column<int>(type: "integer", nullable: false),
                    PlantLogPage = table.Column<int>(type: "integer", nullable: false),
                    PlantEditionPage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    UserSurname = table.Column<string>(type: "text", nullable: true),
                    UserBirthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserCreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserPassword = table.Column<string>(type: "text", nullable: true),
                    UserLoginName = table.Column<string>(type: "text", nullable: true),
                    UserPhone = table.Column<string>(type: "text", nullable: true),
                    UserEmail = table.Column<string>(type: "text", nullable: true),
                    UserRoleRoleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_UserRoleRoleId",
                        column: x => x.UserRoleRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageTransmitterUserId = table.Column<int>(type: "integer", nullable: true),
                    MessageReceiverUserId = table.Column<int>(type: "integer", nullable: true),
                    MessageContent = table.Column<string>(type: "text", nullable: true),
                    MessageDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Users_MessageReceiverUserId",
                        column: x => x.MessageReceiverUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Messages_Users_MessageTransmitterUserId",
                        column: x => x.MessageTransmitterUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantName = table.Column<string>(type: "text", nullable: true),
                    PlantDescription = table.Column<string>(type: "text", nullable: true),
                    PlantTips = table.Column<string>(type: "text", nullable: true),
                    PlantCreatorUserId = table.Column<int>(type: "integer", nullable: true),
                    PlantEditorUserId = table.Column<int>(type: "integer", nullable: true),
                    PlantTypeId = table.Column<int>(type: "integer", nullable: false),
                    PlantCreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlantModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlantLogId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantId);
                    table.ForeignKey(
                        name: "FK_Plants_PlantTypes_PlantTypeId",
                        column: x => x.PlantTypeId,
                        principalTable: "PlantTypes",
                        principalColumn: "PlantTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plants_Users_PlantCreatorUserId",
                        column: x => x.PlantCreatorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Plants_Users_PlantEditorUserId",
                        column: x => x.PlantEditorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostCreatorUserId = table.Column<int>(type: "integer", nullable: true),
                    PostContent = table.Column<string>(type: "text", nullable: true),
                    PostDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Users_PostCreatorUserId",
                        column: x => x.PostCreatorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentCreatorUserId = table.Column<int>(type: "integer", nullable: true),
                    CommentContent = table.Column<string>(type: "text", nullable: true),
                    CommentPostPostId = table.Column<int>(type: "integer", nullable: true),
                    CommentPlantPlantId = table.Column<int>(type: "integer", nullable: true),
                    CommentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Plants_CommentPlantPlantId",
                        column: x => x.CommentPlantPlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_CommentPostPostId",
                        column: x => x.CommentPostPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_Comments_Users_CommentCreatorUserId",
                        column: x => x.CommentCreatorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentCreatorUserId",
                table: "Comments",
                column: "CommentCreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentPlantPlantId",
                table: "Comments",
                column: "CommentPlantPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentPostPostId",
                table: "Comments",
                column: "CommentPostPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageReceiverUserId",
                table: "Messages",
                column: "MessageReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageTransmitterUserId",
                table: "Messages",
                column: "MessageTransmitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantCreatorUserId",
                table: "Plants",
                column: "PlantCreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantEditorUserId",
                table: "Plants",
                column: "PlantEditorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantLogId",
                table: "Plants",
                column: "PlantLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantTypeId",
                table: "Plants",
                column: "PlantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostCreatorUserId",
                table: "Posts",
                column: "PostCreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleRoleId",
                table: "Users",
                column: "UserRoleRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "PlantTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
