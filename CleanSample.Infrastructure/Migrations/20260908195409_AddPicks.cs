using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPicks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Picks",
                columns: table => new
                {
                    PickId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false),
                    PickRequestPartId = table.Column<long>(type: "bigint", nullable: true),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PickedBy = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    PickDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Picked"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picks", x => x.PickId);
                    table.CheckConstraint("CK_Picks_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_Picks_Driver",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Picks_Parts",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Picks_PickRequestParts",
                        column: x => x.PickRequestPartId,
                        principalTable: "PickRequestParts",
                        principalColumn: "PickRequestPartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Picks_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Picks_PickedBy",
                        column: x => x.PickedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Picks_Vehicle",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Picks_DriverId",
                table: "Picks",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_PartId",
                table: "Picks",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_PickedBy",
                table: "Picks",
                column: "PickedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_PickRequestId",
                table: "Picks",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_PickRequestPartId",
                table: "Picks",
                column: "PickRequestPartId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_VehicleId",
                table: "Picks",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picks");
        }
    }
}
