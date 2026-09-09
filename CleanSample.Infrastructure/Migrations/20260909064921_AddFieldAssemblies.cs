using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldAssemblies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldAssemblies",
                columns: table => new
                {
                    FieldAssemblyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldJobId = table.Column<long>(type: "bigint", nullable: false),
                    ProductVariantId = table.Column<int>(type: "int", nullable: false),
                    ProductBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AssemblyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldAssemblies", x => x.FieldAssemblyId);
                    table.CheckConstraint("CK_FieldAssemblies_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_FieldAssemblies_FieldJobs",
                        column: x => x.FieldJobId,
                        principalTable: "FieldJobs",
                        principalColumn: "FieldJobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAssemblies_ProductVariants",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "ProductVariantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAssemblies_Supervisor",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAssemblies_Technician",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldAssemblies_FieldJobId",
                table: "FieldAssemblies",
                column: "FieldJobId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAssemblies_ProductVariantId",
                table: "FieldAssemblies",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAssemblies_SupervisorId",
                table: "FieldAssemblies",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAssemblies_TechnicianId",
                table: "FieldAssemblies",
                column: "TechnicianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldAssemblies");
        }
    }
}
