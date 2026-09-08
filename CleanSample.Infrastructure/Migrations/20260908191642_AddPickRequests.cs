using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPickRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickRequests",
                columns: table => new
                {
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    RequestedBy = table.Column<int>(type: "int", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Created"),
                    DestinationAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DestinationCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickRequests", x => x.PickRequestId);
                    table.ForeignKey(
                        name: "FK_PickRequests_ClientLocations",
                        column: x => x.ClientLocationId,
                        principalTable: "ClientLocations",
                        principalColumn: "ClientLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequests_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequests_Driver",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequests_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequests_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequests_Vehicle",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_ClientId",
                table: "PickRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_ClientLocationId",
                table: "PickRequests",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_DriverId",
                table: "PickRequests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_OrderId",
                table: "PickRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_RequestedBy",
                table: "PickRequests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_VehicleId",
                table: "PickRequests",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ_PickRequests_RequestNumber",
                table: "PickRequests",
                column: "RequestNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickRequests");
        }
    }
}
