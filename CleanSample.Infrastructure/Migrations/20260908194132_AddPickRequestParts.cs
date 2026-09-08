using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPickRequestParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickRequestParts",
                columns: table => new
                {
                    PickRequestPartId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false),
                    PickRequestLineId = table.Column<long>(type: "bigint", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PickedQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickRequestParts", x => x.PickRequestPartId);
                    table.CheckConstraint("CK_PickRequestParts_PickedQuantity", "[PickedQuantity] >= 0");
                    table.CheckConstraint("CK_PickRequestParts_RequiredQuantity", "[RequiredQuantity] > 0");
                    table.ForeignKey(
                        name: "FK_PickRequestParts_Parts",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickRequestParts_PickRequestLines",
                        column: x => x.PickRequestLineId,
                        principalTable: "PickRequestLines",
                        principalColumn: "PickRequestLineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickRequestParts_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PickRequestParts_PartId",
                table: "PickRequestParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequestParts_PickRequestId",
                table: "PickRequestParts",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequestParts_PickRequestLineId",
                table: "PickRequestParts",
                column: "PickRequestLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickRequestParts");
        }
    }
}
