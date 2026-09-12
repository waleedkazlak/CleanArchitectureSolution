using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusLookupTablesAndEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [Orders] SET [Status] = CASE 
                    WHEN [Status] = 'Draft' THEN '1' 
                    WHEN [Status] = 'Pending' THEN '2' 
                    WHEN [Status] = 'Processing' OR [Status] = 'Confirmed' THEN '3' 
                    WHEN [Status] = 'Completed' THEN '4' 
                    WHEN [Status] = 'Cancelled' THEN '5' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;

                UPDATE [VehicleLoads] SET [Status] = CASE 
                    WHEN [Status] = 'Good' OR [Status] = 'Loaded' OR [Status] = 'Pending' THEN '1' 
                    WHEN [Status] = 'Damaged' THEN '2' 
                    WHEN [Status] = 'Missing' THEN '3' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;

                UPDATE [VehicleOffloads] SET [Status] = CASE 
                    WHEN [Status] = 'Good' OR [Status] = 'Offloaded' OR [Status] = 'Pending' OR [Status] = 'Offloading' THEN '1' 
                    WHEN [Status] = 'Damaged' THEN '2' 
                    WHEN [Status] = 'Missing' THEN '3' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;

                UPDATE [FieldJobs] SET [Status] = CASE 
                    WHEN [Status] = 'Scheduled' THEN '1' 
                    WHEN [Status] = 'InProgress' THEN '2' 
                    WHEN [Status] = 'Completed' THEN '3' 
                    WHEN [Status] = 'Cancelled' THEN '4' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;

                UPDATE [FieldAssemblies] SET [Status] = CASE 
                    WHEN [Status] = 'InProgress' OR [Status] = 'Pending' THEN '1' 
                    WHEN [Status] = 'Completed' THEN '2' 
                    WHEN [Status] = 'Cancelled' THEN '3' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;

                UPDATE [Issues] SET [Status] = CASE 
                    WHEN [Status] = 'Open' THEN '1' 
                    WHEN [Status] = 'InProgress' THEN '2' 
                    WHEN [Status] = 'Resolved' THEN '3' 
                    WHEN [Status] = 'Closed' THEN '4' 
                    ELSE '1' 
                END WHERE TRY_CAST([Status] AS int) IS NULL;
            ");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoadRequestParts");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VehicleOffloads",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Offloading");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VehicleLoads",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Loaded");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Draft");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Open");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "FieldJobs",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Scheduled");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "FieldAssemblies",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Pending");

            migrationBuilder.CreateTable(
                name: "FieldAssemblyStatuses",
                columns: table => new
                {
                    FieldAssemblyStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldAssemblyStatuses", x => x.FieldAssemblyStatusId);
                });

            migrationBuilder.CreateTable(
                name: "FieldJobStatuses",
                columns: table => new
                {
                    FieldJobStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldJobStatuses", x => x.FieldJobStatusId);
                });

            migrationBuilder.CreateTable(
                name: "IssueStatuses",
                columns: table => new
                {
                    IssueStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueStatuses", x => x.IssueStatusId);
                });

            migrationBuilder.CreateTable(
                name: "LoadRequestStatuses",
                columns: table => new
                {
                    LoadRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadRequestStatuses", x => x.LoadRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleLoadStatuses",
                columns: table => new
                {
                    VehicleLoadStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLoadStatuses", x => x.VehicleLoadStatusId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleOffloadStatuses",
                columns: table => new
                {
                    VehicleOffloadStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleOffloadStatuses", x => x.VehicleOffloadStatusId);
                });

            migrationBuilder.InsertData(
                table: "FieldAssemblyStatuses",
                columns: new[] { "FieldAssemblyStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly in progress", "InProgress", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly completed", "Completed", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly cancelled", "Cancelled", null }
                });

            migrationBuilder.InsertData(
                table: "FieldJobStatuses",
                columns: new[] { "FieldJobStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Field job scheduled", "Scheduled", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Field job in progress", "InProgress", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Field job completed", "Completed", null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Field job cancelled", "Cancelled", null }
                });

            migrationBuilder.InsertData(
                table: "IssueStatuses",
                columns: new[] { "IssueStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issue open and unresolved", "Open", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issue in progress of investigation/fixing", "InProgress", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issue resolved", "Resolved", null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issue closed", "Closed", null }
                });

            migrationBuilder.InsertData(
                table: "LoadRequestStatuses",
                columns: new[] { "LoadRequestStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New load request created", "New", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Loading in progress", "Loading", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Offloaded at destination", "Offloaded", null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Load request completed", "Completed", null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Load request cancelled", "Cancelled", null }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Draft order created", "Draft", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order pending approval/processing", "Pending", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order in processing", "Processing", null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order fulfilled and completed", "Completed", null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order cancelled", "Cancelled", null }
                });

            migrationBuilder.InsertData(
                table: "VehicleLoadStatuses",
                columns: new[] { "VehicleLoadStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Loaded in good condition", "Good", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Loaded in damaged condition", "Damaged", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Missing items during load", "Missing", null }
                });

            migrationBuilder.InsertData(
                table: "VehicleOffloadStatuses",
                columns: new[] { "VehicleOffloadStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Offloaded in good condition", "Good", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Offloaded in damaged condition", "Damaged", null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Missing items during offload", "Missing", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOffloads_Status",
                table: "VehicleOffloads",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoads_Status",
                table: "VehicleLoads",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAssemblies_Status",
                table: "FieldAssemblies",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldAssemblies_FieldAssemblyStatuses",
                table: "FieldAssemblies",
                column: "Status",
                principalTable: "FieldAssemblyStatuses",
                principalColumn: "FieldAssemblyStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldJobs_FieldJobStatuses",
                table: "FieldJobs",
                column: "Status",
                principalTable: "FieldJobStatuses",
                principalColumn: "FieldJobStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueStatuses",
                table: "Issues",
                column: "Status",
                principalTable: "IssueStatuses",
                principalColumn: "IssueStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadRequests_LoadRequestStatuses",
                table: "LoadRequests",
                column: "Status",
                principalTable: "LoadRequestStatuses",
                principalColumn: "LoadRequestStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses",
                table: "Orders",
                column: "Status",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLoads_VehicleLoadStatuses",
                table: "VehicleLoads",
                column: "Status",
                principalTable: "VehicleLoadStatuses",
                principalColumn: "VehicleLoadStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleOffloads_VehicleOffloadStatuses",
                table: "VehicleOffloads",
                column: "Status",
                principalTable: "VehicleOffloadStatuses",
                principalColumn: "VehicleOffloadStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldAssemblies_FieldAssemblyStatuses",
                table: "FieldAssemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldJobs_FieldJobStatuses",
                table: "FieldJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueStatuses",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadRequests_LoadRequestStatuses",
                table: "LoadRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLoads_VehicleLoadStatuses",
                table: "VehicleLoads");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleOffloads_VehicleOffloadStatuses",
                table: "VehicleOffloads");

            migrationBuilder.DropTable(
                name: "FieldAssemblyStatuses");

            migrationBuilder.DropTable(
                name: "FieldJobStatuses");

            migrationBuilder.DropTable(
                name: "IssueStatuses");

            migrationBuilder.DropTable(
                name: "LoadRequestStatuses");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "VehicleLoadStatuses");

            migrationBuilder.DropTable(
                name: "VehicleOffloadStatuses");

            migrationBuilder.DropIndex(
                name: "IX_VehicleOffloads_Status",
                table: "VehicleOffloads");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLoads_Status",
                table: "VehicleLoads");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Status",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_FieldAssemblies_Status",
                table: "FieldAssemblies");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VehicleOffloads",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Offloading",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VehicleLoads",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Loaded",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Draft",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LoadRequestParts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Issues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Open",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FieldJobs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Scheduled",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FieldAssemblies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }
    }
}
