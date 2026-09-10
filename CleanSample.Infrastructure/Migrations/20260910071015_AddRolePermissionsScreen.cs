using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionsScreen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Insert ROLE_PERMISSIONS screen in Screens table under Administration module
                IF NOT EXISTS (SELECT 1 FROM [Screens] WHERE [Code] = 'ROLE_PERMISSIONS')
                BEGIN
                    INSERT INTO [Screens] ([Code], [Name], [Module], [Description], [IsActive], [CreatedAt])
                    VALUES ('ROLE_PERMISSIONS', 'Role Permissions', 'Administration', 'Configure role access rights and screen permissions', 1, SYSUTCDATETIME());
                END

                -- 2. Link ROLE_PERMISSIONS screen to Admin role with full permissions
                DECLARE @AdminRoleId INT;
                DECLARE @RolePermissionsScreenId INT;

                SELECT TOP 1 @AdminRoleId = [RoleId] FROM [Roles] WHERE [Name] = 'Admin' ORDER BY [RoleId];
                IF @AdminRoleId IS NULL
                    SET @AdminRoleId = 1;

                SELECT TOP 1 @RolePermissionsScreenId = [ScreenId] FROM [Screens] WHERE [Code] = 'ROLE_PERMISSIONS';

                IF @RolePermissionsScreenId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId] = @AdminRoleId AND [ScreenId] = @RolePermissionsScreenId)
                BEGIN
                    INSERT INTO [RolePermissions] ([RoleId], [ScreenId], [CanView], [CanCreate], [CanUpdate], [CanDelete], [CreatedAt])
                    VALUES (@AdminRoleId, @RolePermissionsScreenId, 1, 1, 1, 1, SYSUTCDATETIME());
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @RolePermissionsScreenId INT;
                SELECT TOP 1 @RolePermissionsScreenId = [ScreenId] FROM [Screens] WHERE [Code] = 'ROLE_PERMISSIONS';

                IF @RolePermissionsScreenId IS NOT NULL
                BEGIN
                    DELETE FROM [RolePermissions] WHERE [ScreenId] = @RolePermissionsScreenId;
                    DELETE FROM [Screens] WHERE [ScreenId] = @RolePermissionsScreenId;
                END
            ");
        }
    }
}
