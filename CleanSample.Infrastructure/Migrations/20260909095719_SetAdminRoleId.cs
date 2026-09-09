using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetAdminRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Set Admin user RoleId to 1 (or Admin RoleId)
                UPDATE Users 
                SET RoleId = 1 
                WHERE UserName = 'admin';

                -- Also fix any null RoleIds to 1 if RoleId is null
                UPDATE Users 
                SET RoleId = 1 
                WHERE RoleId IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
