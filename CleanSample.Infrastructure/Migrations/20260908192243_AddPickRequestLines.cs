using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPickRequestLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickRequestLines",
                columns: table => new
                {
                    PickRequestLineId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ProductVariantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickRequestLines", x => x.PickRequestLineId);
                    table.CheckConstraint("CK_PickRequestLines_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_PickRequestLines_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickRequestLines_ProductVariants",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "ProductVariantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PickRequestLines_PickRequestId",
                table: "PickRequestLines",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequestLines_ProductVariantId",
                table: "PickRequestLines",
                column: "ProductVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickRequestLines");
        }
    }
}
