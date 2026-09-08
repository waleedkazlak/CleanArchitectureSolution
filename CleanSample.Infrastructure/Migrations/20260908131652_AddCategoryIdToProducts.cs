using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure at least one default category exists
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Categories)
                BEGIN
                    INSERT INTO Categories (Name, Description, CreatedAt)
                    VALUES ('General', 'Default Category', SYSUTCDATETIME());
                END
            ");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Update existing products with the first available category
            migrationBuilder.Sql(@"
                DECLARE @DefaultCategoryId INT = (SELECT TOP 1 CategoryId FROM Categories ORDER BY CategoryId ASC);
                UPDATE Products SET CategoryId = @DefaultCategoryId WHERE CategoryId = 0 OR CategoryId NOT IN (SELECT CategoryId FROM Categories);
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");
        }
    }
}
