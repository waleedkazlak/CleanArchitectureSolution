using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePickToLoadAndLoadToOffload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing check constraints if present
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_FieldJobs_PickRequestId')
                    ALTER TABLE [FieldJobs] DROP CONSTRAINT [CK_FieldJobs_PickRequestId];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_PickRequests_Status')
                    ALTER TABLE [PickRequests] DROP CONSTRAINT [CK_PickRequests_Status];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_PickRequestLines_Quantity')
                    ALTER TABLE [PickRequestLines] DROP CONSTRAINT [CK_PickRequestLines_Quantity];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_PickRequestParts_RequiredQuantity')
                    ALTER TABLE [PickRequestParts] DROP CONSTRAINT [CK_PickRequestParts_RequiredQuantity];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_PickRequestParts_PickedQuantity')
                    ALTER TABLE [PickRequestParts] DROP CONSTRAINT [CK_PickRequestParts_PickedQuantity];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Picks_Quantity')
                    ALTER TABLE [Picks] DROP CONSTRAINT [CK_Picks_Quantity];
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_VehicleLoadItems_Quantity')
                    ALTER TABLE [VehicleLoadItems] DROP CONSTRAINT [CK_VehicleLoadItems_Quantity];
            ");

            // Rename Tables and Columns using sp_rename
            migrationBuilder.Sql(@"
                -- 1. PickRequests -> LoadRequests
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PickRequests')
                BEGIN
                    EXEC sp_rename 'PickRequests', 'LoadRequests';
                    EXEC sp_rename 'LoadRequests.PickRequestId', 'LoadRequestId', 'COLUMN';
                END

                -- 2. PickRequestLines -> LoadRequestLines
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PickRequestLines')
                BEGIN
                    EXEC sp_rename 'PickRequestLines', 'LoadRequestLines';
                    EXEC sp_rename 'LoadRequestLines.PickRequestLineId', 'LoadRequestLineId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestLines.PickRequestId', 'LoadRequestId', 'COLUMN';
                END

                -- 3. PickRequestParts -> LoadRequestParts
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PickRequestParts')
                BEGIN
                    EXEC sp_rename 'PickRequestParts', 'LoadRequestParts';
                    EXEC sp_rename 'LoadRequestParts.PickRequestPartId', 'LoadRequestPartId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.PickRequestId', 'LoadRequestId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.PickRequestLineId', 'LoadRequestLineId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.PickedQuantity', 'LoadedQuantity', 'COLUMN';
                END

                -- 4. Picks -> Loads
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Picks')
                BEGIN
                    EXEC sp_rename 'Picks', 'Loads';
                    EXEC sp_rename 'Loads.PickId', 'LoadId', 'COLUMN';
                    EXEC sp_rename 'Loads.PickRequestId', 'LoadRequestId', 'COLUMN';
                    EXEC sp_rename 'Loads.PickRequestPartId', 'LoadRequestPartId', 'COLUMN';
                    EXEC sp_rename 'Loads.PickedBy', 'LoadedBy', 'COLUMN';
                    EXEC sp_rename 'Loads.PickDate', 'LoadDate', 'COLUMN';
                END

                -- 5. VehicleLoads -> VehicleOffloads
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VehicleLoads')
                BEGIN
                    EXEC sp_rename 'VehicleLoads', 'VehicleOffloads';
                    EXEC sp_rename 'VehicleOffloads.VehicleLoadId', 'VehicleOffloadId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloads.PickRequestId', 'LoadRequestId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloads.LoadDate', 'OffloadDate', 'COLUMN';
                END

                -- 6. VehicleLoadItems -> VehicleOffloadItems
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VehicleLoadItems')
                BEGIN
                    EXEC sp_rename 'VehicleLoadItems', 'VehicleOffloadItems';
                    EXEC sp_rename 'VehicleOffloadItems.VehicleLoadItemId', 'VehicleOffloadItemId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.VehicleLoadId', 'VehicleOffloadId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.PickId', 'LoadId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.LoadedAt', 'OffloadedAt', 'COLUMN';
                END

                -- 7. FieldJobs.PickRequestId -> LoadRequestId
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('FieldJobs') AND name = 'PickRequestId')
                BEGIN
                    EXEC sp_rename 'FieldJobs.PickRequestId', 'LoadRequestId', 'COLUMN';
                END

                -- 8. Issues.PickRequestId -> LoadRequestId
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Issues') AND name = 'PickRequestId')
                BEGIN
                    EXEC sp_rename 'Issues.PickRequestId', 'LoadRequestId', 'COLUMN';
                END
            ");

            // Update existing status values to match new terminology
            migrationBuilder.Sql(@"
                UPDATE [LoadRequests] SET [Status] = 'Loading' WHERE [Status] = 'Picking';
                UPDATE [LoadRequestParts] SET [Status] = 'Loaded' WHERE [Status] = 'Picked';
                UPDATE [Loads] SET [Status] = 'Loaded' WHERE [Status] = 'Picked';
                UPDATE [VehicleOffloads] SET [Status] = 'Offloaded' WHERE [Status] = 'Loaded';
            ");

            // Add Check Constraints with new names
            migrationBuilder.Sql(@"
                ALTER TABLE [LoadRequests] ADD CONSTRAINT [CK_LoadRequests_Status] CHECK ([Status] IN ('Created', 'Loading', 'Completed', 'Cancelled'));
                ALTER TABLE [LoadRequestLines] ADD CONSTRAINT [CK_LoadRequestLines_Quantity] CHECK ([Quantity] > 0);
                ALTER TABLE [LoadRequestParts] ADD CONSTRAINT [CK_LoadRequestParts_RequiredQuantity] CHECK ([RequiredQuantity] > 0);
                ALTER TABLE [LoadRequestParts] ADD CONSTRAINT [CK_LoadRequestParts_LoadedQuantity] CHECK ([LoadedQuantity] >= 0);
                ALTER TABLE [Loads] ADD CONSTRAINT [CK_Loads_Quantity] CHECK ([Quantity] > 0);
                ALTER TABLE [VehicleOffloadItems] ADD CONSTRAINT [CK_VehicleOffloadItems_Quantity] CHECK ([Quantity] > 0);
            ");

            // Update Screens table entries
            migrationBuilder.Sql(@"
                UPDATE [Screens] SET [Code] = 'LOAD_REQUESTS', [Name] = 'Load Requests', [Description] = 'Manage warehouse load allocations' WHERE [Code] = 'PICK_REQUESTS';
                UPDATE [Screens] SET [Code] = 'LOADS', [Name] = 'Loads Scanning', [Description] = 'Barcode load scanning and logging' WHERE [Code] = 'PICKS';
                UPDATE [Screens] SET [Code] = 'VEHICLE_OFFLOADS', [Name] = 'Vehicle Offloads', [Description] = 'Manage truck offloading manifests' WHERE [Code] = 'VEHICLE_LOADS';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse renames if rolling back
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LoadRequests')
                BEGIN
                    EXEC sp_rename 'LoadRequests.LoadRequestId', 'PickRequestId', 'COLUMN';
                    EXEC sp_rename 'LoadRequests', 'PickRequests';
                END

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LoadRequestLines')
                BEGIN
                    EXEC sp_rename 'LoadRequestLines.LoadRequestLineId', 'PickRequestLineId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestLines.LoadRequestId', 'PickRequestId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestLines', 'PickRequestLines';
                END

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LoadRequestParts')
                BEGIN
                    EXEC sp_rename 'LoadRequestParts.LoadRequestPartId', 'PickRequestPartId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.LoadRequestId', 'PickRequestId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.LoadRequestLineId', 'PickRequestLineId', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts.LoadedQuantity', 'PickedQuantity', 'COLUMN';
                    EXEC sp_rename 'LoadRequestParts', 'PickRequestParts';
                END

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Loads')
                BEGIN
                    EXEC sp_rename 'Loads.LoadId', 'PickId', 'COLUMN';
                    EXEC sp_rename 'Loads.LoadRequestId', 'PickRequestId', 'COLUMN';
                    EXEC sp_rename 'Loads.LoadRequestPartId', 'PickRequestPartId', 'COLUMN';
                    EXEC sp_rename 'Loads.LoadedBy', 'PickedBy', 'COLUMN';
                    EXEC sp_rename 'Loads.LoadDate', 'PickDate', 'COLUMN';
                    EXEC sp_rename 'Loads', 'Picks';
                END

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VehicleOffloads')
                BEGIN
                    EXEC sp_rename 'VehicleOffloads.VehicleOffloadId', 'VehicleLoadId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloads.LoadRequestId', 'PickRequestId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloads.OffloadDate', 'LoadDate', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloads', 'VehicleLoads';
                END

                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VehicleOffloadItems')
                BEGIN
                    EXEC sp_rename 'VehicleOffloadItems.VehicleOffloadItemId', 'VehicleLoadItemId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.VehicleOffloadId', 'VehicleLoadId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.LoadId', 'PickId', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems.OffloadedAt', 'LoadedAt', 'COLUMN';
                    EXEC sp_rename 'VehicleOffloadItems', 'VehicleLoadItems';
                END

                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('FieldJobs') AND name = 'LoadRequestId')
                BEGIN
                    EXEC sp_rename 'FieldJobs.LoadRequestId', 'PickRequestId', 'COLUMN';
                END

                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Issues') AND name = 'LoadRequestId')
                BEGIN
                    EXEC sp_rename 'Issues.LoadRequestId', 'PickRequestId', 'COLUMN';
                END

                UPDATE [Screens] SET [Code] = 'PICK_REQUESTS', [Name] = 'Pick Requests', [Description] = 'Manage warehouse pick allocations' WHERE [Code] = 'LOAD_REQUESTS';
                UPDATE [Screens] SET [Code] = 'PICKS', [Name] = 'Picks Scanning', [Description] = 'Barcode pick scanning and logging' WHERE [Code] = 'LOADS';
                UPDATE [Screens] SET [Code] = 'VEHICLE_LOADS', [Name] = 'Vehicle Loads', [Description] = 'Manage truck loading manifests' WHERE [Code] = 'VEHICLE_OFFLOADS';
            ");
        }
    }
}
