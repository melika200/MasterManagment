using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ReSeedSupportStatus : Migration
    {
        /// <inheritdoc />
       
            protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SupportStatusTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
            { 1, "Opened" },
            { 2, "Progress" },
            { 3, "Answer And Closed" }
                });
        }

        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
