using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdToLoadRequestParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "LoadRequestParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE lrp
                SET lrp.ProductId = lrl.ProductId
                FROM dbo.LoadRequestParts lrp
                INNER JOIN dbo.LoadRequestLines lrl ON lrp.LoadRequestLineId = lrl.LoadRequestLineId;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_LoadRequestParts_ProductId",
                table: "LoadRequestParts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadRequestParts_Products",
                table: "LoadRequestParts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadRequestParts_Products",
                table: "LoadRequestParts");

            migrationBuilder.DropIndex(
                name: "IX_LoadRequestParts_ProductId",
                table: "LoadRequestParts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "LoadRequestParts");
        }
    }
}
