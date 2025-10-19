using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterManagement.Infrastructure.EFCore.Migrations
{
    public partial class UpdateSupportStatusTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
     
            migrationBuilder.Sql(@"
IF EXISTS (SELECT * FROM sys.columns 
           WHERE Name = N'IsDeleted' AND Object_ID = Object_ID(N'SupportStatusTypes'))
BEGIN
    ALTER TABLE SupportStatusTypes DROP COLUMN IsDeleted;
END
");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE Name = N'IsDeleted' AND Object_ID = Object_ID(N'SupportStatusTypes'))
BEGIN
    ALTER TABLE SupportStatusTypes ADD IsDeleted BIT NOT NULL DEFAULT(0);
END
");
        }
    }
}
