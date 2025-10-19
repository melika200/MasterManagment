using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class SeedSupportStatusSafely : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert رکوردهای ثابت فقط اگر وجود ندارند
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 1)
    INSERT INTO SupportStatusTypes (Id, Name) VALUES (1, 'Open');

IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 2)
    INSERT INTO SupportStatusTypes (Id, Name) VALUES (2, 'InProgress');

IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 3)
    INSERT INTO SupportStatusTypes (Id, Name) VALUES (3, 'Closed');
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // در صورت rollback، حذف رکوردهای ثابت
            migrationBuilder.Sql(@"
DELETE FROM SupportStatusTypes WHERE Id IN (1,2,3);
");
        }
    }
}
