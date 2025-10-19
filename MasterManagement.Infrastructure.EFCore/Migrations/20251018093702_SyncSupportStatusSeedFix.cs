using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class SyncSupportStatusSeedFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ قبل از هر چیز داده‌های تکراری را حذف می‌کنیم
            migrationBuilder.Sql(@"
                DELETE FROM SupportStatusTypes WHERE Id IN (1, 2, 3);
            ");

            // ✅ دوباره درج داده‌های اولیه
            migrationBuilder.Sql(@"
                INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (1, N'Open', 0);
                INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (2, N'InProgress', 0);
                INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (3, N'Closed', 0);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM SupportStatusTypes WHERE Id IN (1,2,3);
            ");
        }
    }
}
