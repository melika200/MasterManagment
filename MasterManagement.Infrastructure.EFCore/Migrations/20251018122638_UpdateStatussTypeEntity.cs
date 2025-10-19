using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class UpdateStatussTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports");

         
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SupportStatusTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

     
            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes",
                column: "Id");

        
            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports",
                column: "SupportStatusId",
                principalTable: "SupportStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
     
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports");

         
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes");

       
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SupportStatusTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity", "1, 1");

           
            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes",
                column: "Id");

          
            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports",
                column: "SupportStatusId",
                principalTable: "SupportStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
