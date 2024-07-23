using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexesInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_users_user_id",
                schema: "authentication_schema",
                table: "users",
                column: "user_id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_user_id",
                schema: "authentication_schema",
                table: "users");

            migrationBuilder.DropIndex(
                name: "idx_email",
                schema: "authentication_schema",
                table: "user_data");

            migrationBuilder.DropIndex(
                name: "idx_username",
                schema: "authentication_schema",
                table: "user_data");
        }
    }
}
