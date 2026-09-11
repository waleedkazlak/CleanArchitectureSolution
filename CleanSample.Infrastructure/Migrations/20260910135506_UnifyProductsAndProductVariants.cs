using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnifyProductsAndProductVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_ProductBOM_Variant_Part')
                    DROP INDEX [UQ_ProductBOM_Variant_Part] ON [ProductBOM];

                IF OBJECT_ID('dbo.ProductVariants', 'U') IS NOT NULL
                BEGIN
                    UPDATE P
                    SET P.Code = ISNULL(PV.Code, 'PROD-' + CAST(P.ProductId AS VARCHAR(20))),
                        P.Barcode = PV.Barcode,
                        P.Description = PV.Description,
                        P.ColorId = PV.ColorId,
                        P.MaterialId = PV.MaterialId,
                        P.DesignId = PV.DesignId,
                        P.IsActive = PV.IsActive
                    FROM Products P
                    INNER JOIN ProductVariants PV ON P.ProductId = PV.ProductId;

                    UPDATE ol SET ol.ProductVariantId = pv.ProductId
                    FROM OrderLines ol INNER JOIN ProductVariants pv ON ol.ProductVariantId = pv.ProductVariantId;

                    UPDATE lrl SET lrl.ProductVariantId = pv.ProductId
                    FROM LoadRequestLines lrl INNER JOIN ProductVariants pv ON lrl.ProductVariantId = pv.ProductVariantId;

                    UPDATE pb SET pb.ProductVariantId = pv.ProductId
                    FROM ProductBOM pb INNER JOIN ProductVariants pv ON pb.ProductVariantId = pv.ProductVariantId;

                    UPDATE fa SET fa.ProductVariantId = pv.ProductId
                    FROM FieldAssemblies fa INNER JOIN ProductVariants pv ON fa.ProductVariantId = pv.ProductVariantId;
                END

                -- Deduplicate ProductBOM rows before creating unique index
                ;WITH CTE_BOM AS (
                    SELECT ProductBOMId, ROW_NUMBER() OVER(PARTITION BY ProductVariantId, PartId ORDER BY ProductBOMId) AS rn
                    FROM ProductBOM
                )
                DELETE FROM ProductBOM WHERE ProductBOMId IN (SELECT ProductBOMId FROM CTE_BOM WHERE rn > 1);

                UPDATE Products SET Code = 'PROD-' + CAST(ProductId AS VARCHAR(20)) WHERE Code = '' OR Code IS NULL;
            ");

            migrationBuilder.Sql(@"
                DECLARE @sql NVARCHAR(MAX) = N'';
                SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
                               N' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
                FROM sys.foreign_keys
                WHERE name IN (
                    'FK_FieldAssemblies_ProductVariants',
                    'FK_LoadRequestLines_ProductVariants',
                    'FK_PickRequestLines_ProductVariants',
                    'FK_OrderLines_ProductVariants',
                    'FK_ProductBOM_ProductVariants',
                    'FK_Products_Categories_CategoryId',
                    'FK_Products_Categories'
                );
                IF @sql <> N'' EXEC sp_executesql @sql;
            ");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ProductBOM",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "OrderLines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "LoadRequestLines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "FieldAssemblies",
                newName: "ProductId");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LoadRequestLines_ProductVariantId')
                    EXEC sp_rename N'[LoadRequestLines].[IX_LoadRequestLines_ProductVariantId]', N'IX_LoadRequestLines_ProductId', 'INDEX';
                ELSE IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PickRequestLines_ProductVariantId')
                    EXEC sp_rename N'[LoadRequestLines].[IX_PickRequestLines_ProductVariantId]', N'IX_LoadRequestLines_ProductId', 'INDEX';

                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_FieldAssemblies_ProductVariantId')
                    EXEC sp_rename N'[FieldAssemblies].[IX_FieldAssemblies_ProductVariantId]', N'IX_FieldAssemblies_ProductId', 'INDEX';

                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OrderLines_ProductVariantId')
                    EXEC sp_rename N'[OrderLines].[IX_OrderLines_ProductVariantId]', N'IX_OrderLines_ProductId', 'INDEX';

                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ProductBOM_ProductVariantId')
                    EXEC sp_rename N'[ProductBOM].[IX_ProductBOM_ProductVariantId]', N'IX_ProductBOM_ProductId', 'INDEX';

                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_ProductBOM_Product_Part')
                    CREATE UNIQUE INDEX [UQ_ProductBOM_Product_Part] ON [ProductBOM] ([ProductId], [PartId]);
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorId",
                table: "Products",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DesignId",
                table: "Products",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MaterialId",
                table: "Products",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "UQ_Products_Barcode",
                table: "Products",
                column: "Barcode",
                unique: true,
                filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldAssemblies_Products",
                table: "FieldAssemblies",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadRequestLines_Products",
                table: "LoadRequestLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Products",
                table: "OrderLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBOM_Products",
                table: "ProductBOM",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Colors",
                table: "Products",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Designs",
                table: "Products",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "DesignId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Materials",
                table: "Products",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldAssemblies_Products",
                table: "FieldAssemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadRequestLines_Products",
                table: "LoadRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Products",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBOM_Products",
                table: "ProductBOM");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Colors",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Designs",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Materials",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ColorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DesignId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MaterialId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UQ_Products_Barcode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UQ_Products_Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductBOM",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "UQ_ProductBOM_Product_Part",
                table: "ProductBOM",
                newName: "UQ_ProductBOM_Variant_Part");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBOM_ProductId",
                table: "ProductBOM",
                newName: "IX_ProductBOM_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderLines",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLines_ProductId",
                table: "OrderLines",
                newName: "IX_OrderLines_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "LoadRequestLines",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_LoadRequestLines_ProductId",
                table: "LoadRequestLines",
                newName: "IX_LoadRequestLines_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "FieldAssemblies",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldAssemblies_ProductId",
                table: "FieldAssemblies",
                newName: "IX_FieldAssemblies_ProductVariantId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    ProductVariantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorId = table.Column<int>(type: "int", nullable: true),
                    DesignId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.ProductVariantId);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Colors",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "ColorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Designs",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "DesignId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Materials",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ColorId",
                table: "ProductVariants",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_DesignId",
                table: "ProductVariants",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_MaterialId",
                table: "ProductVariants",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductVariants_Barcode",
                table: "ProductVariants",
                column: "Barcode",
                unique: true,
                filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductVariants_Code",
                table: "ProductVariants",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldAssemblies_ProductVariants",
                table: "FieldAssemblies",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadRequestLines_ProductVariants",
                table: "LoadRequestLines",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ProductVariants",
                table: "OrderLines",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBOM_ProductVariants",
                table: "ProductBOM",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
