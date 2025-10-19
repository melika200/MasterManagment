using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportEntoty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyMessage",
                table: "Supports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyMessage",
                table: "Supports");
        }
    }
}
