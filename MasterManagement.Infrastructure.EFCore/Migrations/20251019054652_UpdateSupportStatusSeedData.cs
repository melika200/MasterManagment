using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportStatusSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Opened");

            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Progress");

            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Answer And Closed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "InProgress");

            migrationBuilder.UpdateData(
                table: "SupportStatusTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Closed");
        }
    }
}
