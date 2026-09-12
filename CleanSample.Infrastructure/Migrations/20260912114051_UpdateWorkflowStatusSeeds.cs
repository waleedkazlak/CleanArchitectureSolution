using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkflowStatusSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Loading completed in good condition", "Loaded" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 3,
                column: "Description",
                value: "Offloaded in good condition at destination");

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 4,
                column: "Description",
                value: "All field assemblies completed");

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Blocked due to damaged or missing parts during loading/offloading", "Blocked" });

            migrationBuilder.InsertData(
                table: "LoadRequestStatuses",
                columns: new[] { "LoadRequestStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Load request cancelled", "Cancelled", null });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2,
                column: "Description",
                value: "Order pending approval");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Order approved by sales manager", "Approved" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Load requests placed for order", "Processing" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "All load requests fulfilled and order completed", "Completed" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order cancelled", "Cancelled", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Loading in progress", "Loading" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 3,
                column: "Description",
                value: "Offloaded at destination");

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 4,
                column: "Description",
                value: "Load request completed");

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Load request cancelled", "Cancelled" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2,
                column: "Description",
                value: "Order pending approval/processing");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Order in processing", "Processing" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Order fulfilled and completed", "Completed" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Order cancelled", "Cancelled" });
        }
    }
}
