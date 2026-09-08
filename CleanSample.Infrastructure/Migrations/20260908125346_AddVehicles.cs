using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CapacityKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Vehicles_PlateNumber",
                table: "Vehicles",
                column: "PlateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Vehicles_VehicleNumber",
                table: "Vehicles",
                column: "VehicleNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
