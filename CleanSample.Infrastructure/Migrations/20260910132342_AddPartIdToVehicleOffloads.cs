using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartIdToVehicleOffloads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartId",
                table: "VehicleOffloads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOffloads_PartId",
                table: "VehicleOffloads",
                column: "PartId");

            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Parts]) UPDATE [VehicleOffloads] SET [PartId] = (SELECT TOP 1 [PartId] FROM [Parts]) WHERE [PartId] = 0 OR [PartId] NOT IN (SELECT [PartId] FROM [Parts]);");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleOffloads_Parts",
                table: "VehicleOffloads",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "PartId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleOffloads_Parts",
                table: "VehicleOffloads");

            migrationBuilder.DropIndex(
                name: "IX_VehicleOffloads_PartId",
                table: "VehicleOffloads");

            migrationBuilder.DropColumn(
                name: "PartId",
                table: "VehicleOffloads");
        }
    }
}
