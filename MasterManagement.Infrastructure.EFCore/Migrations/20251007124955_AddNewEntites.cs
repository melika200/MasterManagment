using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStateId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "OrderStateName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingStatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ShippingStatusName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Paid" },
                    { 3, "Shipped" },
                    { 4, "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "ShippingStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Preparing" },
                    { 2, "Sent to Post" },
                    { 3, "In Transit" },
                    { 4, "Delivered" },
                    { 5, "Returned" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStateId",
                table: "Orders",
                column: "OrderStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStates_OrderStateId",
                table: "Orders",
                column: "OrderStateId",
                principalTable: "OrderStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId",
                principalTable: "ShippingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStates_OrderStateId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderStates");

            migrationBuilder.DropTable(
                name: "ShippingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderStateId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStateId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStateName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingStatusName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Orders");
        }
    }
}
