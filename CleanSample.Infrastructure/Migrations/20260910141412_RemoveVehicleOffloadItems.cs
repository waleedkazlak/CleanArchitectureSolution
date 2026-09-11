using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVehicleOffloadItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleOffloadItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleOffloadItems",
                columns: table => new
                {
                    VehicleOffloadItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadId = table.Column<long>(type: "bigint", nullable: true),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    VehicleOffloadId = table.Column<long>(type: "bigint", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    OffloadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleOffloadItems", x => x.VehicleOffloadItemId);
                    table.CheckConstraint("CK_VehicleOffloadItems_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_VehicleOffloadItems_Parts",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleOffloadItems_VehicleLoads",
                        column: x => x.LoadId,
                        principalTable: "VehicleLoads",
                        principalColumn: "LoadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleOffloadItems_VehicleOffloads",
                        column: x => x.VehicleOffloadId,
                        principalTable: "VehicleOffloads",
                        principalColumn: "VehicleOffloadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOffloadItems_LoadId",
                table: "VehicleOffloadItems",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOffloadItems_PartId",
                table: "VehicleOffloadItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOffloadItems_VehicleOffloadId",
                table: "VehicleOffloadItems",
                column: "VehicleOffloadId");
        }
    }
}
