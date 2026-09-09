using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplicitIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductBOM_ProductVariantId",
                table: "ProductBOM",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_Barcode",
                table: "Picks",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequests_Status",
                table: "PickRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Status",
                table: "Issues",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_Status",
                table: "FieldJobs",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductBOM_ProductVariantId",
                table: "ProductBOM");

            migrationBuilder.DropIndex(
                name: "IX_Picks_Barcode",
                table: "Picks");

            migrationBuilder.DropIndex(
                name: "IX_PickRequests_Status",
                table: "PickRequests");

            migrationBuilder.DropIndex(
                name: "IX_Issues_Status",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_FieldJobs_Status",
                table: "FieldJobs");
        }
    }
}
