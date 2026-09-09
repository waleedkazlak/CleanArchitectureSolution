using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleLoads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleLoads",
                columns: table => new
                {
                    VehicleLoadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    LoadDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Loading"),
                    Verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLoads", x => x.VehicleLoadId);
                    table.ForeignKey(
                        name: "FK_VehicleLoads_Drivers",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLoads_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLoads_Vehicles",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLoads_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleLoadItems",
                columns: table => new
                {
                    VehicleLoadItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleLoadId = table.Column<long>(type: "bigint", nullable: false),
                    PickId = table.Column<long>(type: "bigint", nullable: true),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    LoadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLoadItems", x => x.VehicleLoadItemId);
                    table.CheckConstraint("CK_VehicleLoadItems_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_VehicleLoadItems_Parts",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLoadItems_Picks",
                        column: x => x.PickId,
                        principalTable: "Picks",
                        principalColumn: "PickId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleLoadItems_VehicleLoads",
                        column: x => x.VehicleLoadId,
                        principalTable: "VehicleLoads",
                        principalColumn: "VehicleLoadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoadItems_PartId",
                table: "VehicleLoadItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoadItems_PickId",
                table: "VehicleLoadItems",
                column: "PickId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoadItems_VehicleLoadId",
                table: "VehicleLoadItems",
                column: "VehicleLoadId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoads_DriverId",
                table: "VehicleLoads",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoads_PickRequestId",
                table: "VehicleLoads",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoads_VehicleId",
                table: "VehicleLoads",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoads_VerifiedBy",
                table: "VehicleLoads",
                column: "VerifiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleLoadItems");

            migrationBuilder.DropTable(
                name: "VehicleLoads");
        }
    }
}
