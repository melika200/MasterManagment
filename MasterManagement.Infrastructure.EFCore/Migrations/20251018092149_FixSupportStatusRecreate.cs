using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class FixSupportStatusRecreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // داده‌های فعلی را نگه می‌داریم
            migrationBuilder.Sql(@"
                IF OBJECT_ID('tempdb..#TempSupportStatus') IS NOT NULL
                    DROP TABLE #TempSupportStatus;

                SELECT Id, Name, IsDeleted
                INTO #TempSupportStatus
                FROM SupportStatusTypes
            ");

            // حذف روابط خارجی برای حذف جدول
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Supports_SupportStatusTypes_SupportStatusId')
                    ALTER TABLE Supports DROP CONSTRAINT FK_Supports_SupportStatusTypes_SupportStatusId;
            ");

            // حذف جدول اصلی
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS SupportStatusTypes;
            ");

            // بازسازی جدول بدون IDENTITY
            migrationBuilder.Sql(@"
                CREATE TABLE [dbo].[SupportStatusTypes](
	                [Id] INT NOT NULL,
	                [Name] NVARCHAR(100) NOT NULL,
	                [IsDeleted] BIT NOT NULL,
	                CONSTRAINT [PK_SupportStatusTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
                );
            ");

            // بازیابی داده‌های قبلی بدون درج تکراری
            migrationBuilder.Sql(@"
                INSERT INTO SupportStatusTypes (Id, Name, IsDeleted)
                SELECT t.Id, t.Name, t.IsDeleted
                FROM #TempSupportStatus t
                WHERE NOT EXISTS (SELECT 1 FROM SupportStatusTypes s WHERE s.Id = t.Id);

                DROP TABLE #TempSupportStatus;
            ");

            // اضافه‌کردن مجدد رابطه
            migrationBuilder.Sql(@"
                ALTER TABLE Supports
                ADD CONSTRAINT FK_Supports_SupportStatusTypes_SupportStatusId
                FOREIGN KEY (SupportStatusId) REFERENCES SupportStatusTypes(Id) ON DELETE NO ACTION;
            ");

            // ✅ درج داده‌های پیش‌فرض فقط اگر وجود ندارند
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 1)
                    INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (1, N'Open', 0);
                IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 2)
                    INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (2, N'InProgress', 0);
                IF NOT EXISTS (SELECT 1 FROM SupportStatusTypes WHERE Id = 3)
                    INSERT INTO SupportStatusTypes (Id, Name, IsDeleted) VALUES (3, N'Closed', 0);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Supports_SupportStatusTypes_SupportStatusId')
                    ALTER TABLE Supports DROP CONSTRAINT FK_Supports_SupportStatusTypes_SupportStatusId;

                DROP TABLE IF EXISTS SupportStatusTypes;

                CREATE TABLE [dbo].[SupportStatusTypes](
	                [Id] INT IDENTITY(1,1) NOT NULL,
	                [Name] NVARCHAR(100) NOT NULL,
	                [IsDeleted] BIT NOT NULL,
	                CONSTRAINT [PK_SupportStatusTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
                );
            ");
        }
    }
}
