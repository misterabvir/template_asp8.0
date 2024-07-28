using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "authentication_schema");

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "authentication_schema",
                columns: table => new
                {
                    outbox_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.outbox_message_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "authentication_schema",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "user_data",
                schema: "authentication_schema",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<byte[]>(type: "bytea", nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_data", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_data",
                        column: x => x.user_id,
                        principalSchema: "authentication_schema",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                schema: "authentication_schema",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    profile_picture = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    cover_picture = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    website = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profiles", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_profile",
                        column: x => x.user_id,
                        principalSchema: "authentication_schema",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_email",
                schema: "authentication_schema",
                table: "user_data",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_username",
                schema: "authentication_schema",
                table: "user_data",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_id",
                schema: "authentication_schema",
                table: "users",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "authentication_schema");

            migrationBuilder.DropTable(
                name: "user_data",
                schema: "authentication_schema");

            migrationBuilder.DropTable(
                name: "user_profiles",
                schema: "authentication_schema");

            migrationBuilder.DropTable(
                name: "users",
                schema: "authentication_schema");
        }
    }
}
