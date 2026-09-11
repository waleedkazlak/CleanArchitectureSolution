using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSpecifiedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Orders_OrderNumber' AND object_id = OBJECT_ID('Orders'))
                    DROP INDEX [UQ_Orders_OrderNumber] ON [Orders];

                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_LoadRequests_RequestNumber' AND object_id = OBJECT_ID('LoadRequests'))
                    DROP INDEX [UQ_LoadRequests_RequestNumber] ON [LoadRequests];

                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_PickRequests_RequestNumber' AND object_id = OBJECT_ID('LoadRequests'))
                    DROP INDEX [UQ_PickRequests_RequestNumber] ON [LoadRequests];

                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_FieldJobs_JobNumber' AND object_id = OBJECT_ID('FieldJobs'))
                    DROP INDEX [UQ_FieldJobs_JobNumber] ON [FieldJobs];

                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Designs_Code' AND object_id = OBJECT_ID('Designs'))
                    DROP INDEX [UQ_Designs_Code] ON [Designs];
            ");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RequestNumber",
                table: "LoadRequests");

            migrationBuilder.DropColumn(
                name: "JobNumber",
                table: "FieldJobs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestNumber",
                table: "LoadRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobNumber",
                table: "FieldJobs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Designs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_LoadRequests_RequestNumber",
                table: "LoadRequests",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FieldJobs_JobNumber",
                table: "FieldJobs",
                column: "JobNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Designs_Code",
                table: "Designs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }
    }
}
