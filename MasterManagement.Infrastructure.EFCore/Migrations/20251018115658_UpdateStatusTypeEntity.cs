using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class UpdateStatusTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف foreign key از جدول Supports
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports");

            // حذف کلید اصلی از جدول SupportStatusTypes
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes");

            // حذف ستون Id
            migrationBuilder.Sql("ALTER TABLE SupportStatusTypes DROP COLUMN Id;");

            // اضافه کردن ستون جدید بدون خاصیت IDENTITY
            migrationBuilder.Sql("ALTER TABLE SupportStatusTypes ADD Id INT NULL;");

            // مقداردهی یکتا به رکوردهای موجود
            migrationBuilder.Sql(@"
                UPDATE SupportStatusTypes SET Id = 1 WHERE Name = 'Open';
                UPDATE SupportStatusTypes SET Id = 2 WHERE Name = 'InProgress';
                UPDATE SupportStatusTypes SET Id = 3 WHERE Name = 'Closed';
            ");

            // تبدیل ستون به NOT NULL
            migrationBuilder.Sql("ALTER TABLE SupportStatusTypes ALTER COLUMN Id INT NOT NULL;");

            // تعریف کلید اصلی جدید
            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes",
                column: "Id");

            // بازگرداندن foreign key
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
            // حذف foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportStatusTypes_SupportStatusId",
                table: "Supports");

            // حذف کلید اصلی
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes");

            // حذف ستون Id
            migrationBuilder.Sql("ALTER TABLE SupportStatusTypes DROP COLUMN Id;");

            // اضافه کردن ستون با خاصیت IDENTITY
            migrationBuilder.Sql("ALTER TABLE SupportStatusTypes ADD Id INT IDENTITY(1,1) NOT NULL;");

            // تعریف کلید اصلی
            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportStatusTypes",
                table: "SupportStatusTypes",
                column: "Id");

            // بازگرداندن foreign key
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
