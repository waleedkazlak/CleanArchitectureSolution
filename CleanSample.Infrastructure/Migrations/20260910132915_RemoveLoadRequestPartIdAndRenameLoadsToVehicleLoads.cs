using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLoadRequestPartIdAndRenameLoadsToVehicleLoads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @sql NVARCHAR(MAX) = N'';

                -- 1. Drop all foreign keys referencing Loads or on Loads
                SELECT @sql += N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];' + CHAR(13)
                FROM sys.foreign_keys
                WHERE parent_object_id = OBJECT_ID('Loads') OR referenced_object_id = OBJECT_ID('Loads');

                IF @sql <> N''
                    EXEC sp_executesql @sql;

                -- 2. Drop all check constraints on Loads
                SET @sql = N'';
                SELECT @sql += N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];' + CHAR(13)
                FROM sys.check_constraints
                WHERE parent_object_id = OBJECT_ID('Loads');

                IF @sql <> N''
                    EXEC sp_executesql @sql;

                -- 3. Drop indexes on LoadRequestPartId if present
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Loads') AND name = 'LoadRequestPartId')
                BEGIN
                    DECLARE @idxSql NVARCHAR(MAX) = N'';
                    SELECT @idxSql += N'DROP INDEX [' + i.name + N'] ON [' + OBJECT_NAME(i.object_id) + N'];' + CHAR(13)
                    FROM sys.indexes i
                    JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                    JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                    WHERE i.object_id = OBJECT_ID('Loads') AND c.name = 'LoadRequestPartId';

                    IF @idxSql <> N''
                        EXEC sp_executesql @idxSql;

                    ALTER TABLE [Loads] DROP COLUMN [LoadRequestPartId];
                END

                -- 4. Rename table Loads to VehicleLoads if present
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Loads')
                BEGIN
                    EXEC sp_rename 'Loads', 'VehicleLoads';
                END
            ");

            migrationBuilder.AddCheckConstraint(
                name: "CK_VehicleLoads_Quantity",
                table: "VehicleLoads",
                sql: "[Quantity] > 0");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_Driver",
                table: "VehicleLoads",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_LoadRequests",
                table: "VehicleLoads",
                column: "LoadRequestId",
                principalTable: "LoadRequests",
                principalColumn: "LoadRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_LoadedBy",
                table: "VehicleLoads",
                column: "LoadedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_Parts",
                table: "VehicleLoads",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "PartId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_Vehicle",
                table: "VehicleLoads",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleOffloadItems_VehicleLoads",
                table: "VehicleOffloadItems",
                column: "LoadId",
                principalTable: "VehicleLoads",
                principalColumn: "LoadId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @sql NVARCHAR(MAX) = N'';

                SELECT @sql += N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];' + CHAR(13)
                FROM sys.foreign_keys
                WHERE parent_object_id = OBJECT_ID('VehicleLoads') OR referenced_object_id = OBJECT_ID('VehicleLoads');

                IF @sql <> N''
                    EXEC sp_executesql @sql;

                SET @sql = N'';
                SELECT @sql += N'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + N'].[' + OBJECT_NAME(parent_object_id) + N'] DROP CONSTRAINT [' + name + N'];' + CHAR(13)
                FROM sys.check_constraints
                WHERE parent_object_id = OBJECT_ID('VehicleLoads');

                IF @sql <> N''
                    EXEC sp_executesql @sql;

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VehicleLoads')
                BEGIN
                    EXEC sp_rename 'VehicleLoads', 'Loads';
                END

                IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Loads') AND name = 'LoadRequestPartId')
                BEGIN
                    ALTER TABLE [Loads] ADD [LoadRequestPartId] bigint NULL;
                END
            ");
        }
    }
}
