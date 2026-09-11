using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoadRequestStatusToIntAndRemoveConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Drop check constraint CK_LoadRequests_Status / CK_PickRequests_Status if present
                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_LoadRequests_Status')
                    ALTER TABLE [LoadRequests] DROP CONSTRAINT [CK_LoadRequests_Status];

                IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_PickRequests_Status')
                    ALTER TABLE [LoadRequests] DROP CONSTRAINT [CK_PickRequests_Status];

                -- 2. Drop any existing indexes on LoadRequests.Status
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PickRequests_Status' AND object_id = OBJECT_ID('LoadRequests'))
                    DROP INDEX [IX_PickRequests_Status] ON [LoadRequests];

                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoadRequests_Status' AND object_id = OBJECT_ID('LoadRequests'))
                    DROP INDEX [IX_LoadRequests_Status] ON [LoadRequests];

                -- 3. Drop default constraint on LoadRequests.Status if present
                DECLARE @defaultConstraintName NVARCHAR(256);
                SELECT @defaultConstraintName = dc.name
                FROM sys.default_constraints dc
                JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                WHERE dc.parent_object_id = OBJECT_ID('LoadRequests') AND c.name = 'Status';

                IF @defaultConstraintName IS NOT NULL
                    EXEC('ALTER TABLE [LoadRequests] DROP CONSTRAINT [' + @defaultConstraintName + '];');

                -- 4. Convert existing string status values to numeric strings
                IF EXISTS (
                    SELECT 1 FROM sys.columns c
                    JOIN sys.types t ON c.user_type_id = t.user_type_id
                    WHERE c.object_id = OBJECT_ID('LoadRequests') 
                      AND c.name = 'Status' 
                      AND t.name LIKE '%char%'
                )
                BEGIN
                    UPDATE [LoadRequests] SET [Status] = '1' WHERE [Status] IN ('Created', 'New') OR [Status] IS NULL;
                    UPDATE [LoadRequests] SET [Status] = '2' WHERE [Status] = 'Loading';
                    UPDATE [LoadRequests] SET [Status] = '3' WHERE [Status] = 'Offloaded';
                    UPDATE [LoadRequests] SET [Status] = '4' WHERE [Status] = 'Completed';
                    UPDATE [LoadRequests] SET [Status] = '7' WHERE [Status] = 'Cancelled';
                    UPDATE [LoadRequests] SET [Status] = '1' WHERE [Status] NOT IN ('1', '2', '3', '4', '7');
                END

                -- 5. Alter column type to INT NOT NULL
                ALTER TABLE [LoadRequests] ALTER COLUMN [Status] INT NOT NULL;

                -- 6. Add default constraint value 1
                ALTER TABLE [LoadRequests] ADD CONSTRAINT [DF_LoadRequests_Status] DEFAULT 1 FOR [Status];

                -- 7. Recreate index IX_LoadRequests_Status
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoadRequests_Status' AND object_id = OBJECT_ID('LoadRequests'))
                    CREATE INDEX [IX_LoadRequests_Status] ON [LoadRequests] ([Status]);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Drop index on LoadRequests.Status
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoadRequests_Status' AND object_id = OBJECT_ID('LoadRequests'))
                    DROP INDEX [IX_LoadRequests_Status] ON [LoadRequests];

                -- 2. Drop default constraint
                DECLARE @defaultConstraintName NVARCHAR(256);
                SELECT @defaultConstraintName = dc.name
                FROM sys.default_constraints dc
                JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                WHERE dc.parent_object_id = OBJECT_ID('LoadRequests') AND c.name = 'Status';

                IF @defaultConstraintName IS NOT NULL
                    EXEC('ALTER TABLE [LoadRequests] DROP CONSTRAINT [' + @defaultConstraintName + '];');

                -- 3. Alter column to NVARCHAR(50)
                ALTER TABLE [LoadRequests] ALTER COLUMN [Status] NVARCHAR(50) NOT NULL;

                -- 4. Map integer status back to string
                UPDATE [LoadRequests] SET [Status] = 'Created' WHERE [Status] = '1';
                UPDATE [LoadRequests] SET [Status] = 'Loading' WHERE [Status] = '2';
                UPDATE [LoadRequests] SET [Status] = 'Offloaded' WHERE [Status] = '3';
                UPDATE [LoadRequests] SET [Status] = 'Completed' WHERE [Status] = '4';
                UPDATE [LoadRequests] SET [Status] = 'Cancelled' WHERE [Status] = '7';

                -- 5. Add default constraint 'Created'
                ALTER TABLE [LoadRequests] ADD CONSTRAINT [DF_LoadRequests_Status] DEFAULT 'Created' FOR [Status];

                -- 6. Add check constraint
                IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_LoadRequests_Status')
                    ALTER TABLE [LoadRequests] ADD CONSTRAINT [CK_LoadRequests_Status] CHECK ([Status] IN ('Created', 'Loading', 'Offloaded', 'Completed', 'Cancelled'));

                -- 7. Recreate index
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoadRequests_Status' AND object_id = OBJECT_ID('LoadRequests'))
                    CREATE INDEX [IX_LoadRequests_Status] ON [LoadRequests] ([Status]);
            ");
        }
    }
}
