using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: true),
                    FieldJobId = table.Column<long>(type: "bigint", nullable: true),
                    FieldAssemblyId = table.Column<long>(type: "bigint", nullable: true),
                    IssueType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Medium"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Open"),
                    ReportedBy = table.Column<int>(type: "int", nullable: true),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ResolvedBy = table.Column<int>(type: "int", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_Issues_FieldAssemblies",
                        column: x => x.FieldAssemblyId,
                        principalTable: "FieldAssemblies",
                        principalColumn: "FieldAssemblyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_FieldJobs",
                        column: x => x.FieldJobId,
                        principalTable: "FieldJobs",
                        principalColumn: "FieldJobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_ReportedBy",
                        column: x => x.ReportedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_ResolvedBy",
                        column: x => x.ResolvedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_FieldAssemblyId",
                table: "Issues",
                column: "FieldAssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_FieldJobId",
                table: "Issues",
                column: "FieldJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_PickRequestId",
                table: "Issues",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ReportedBy",
                table: "Issues",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ResolvedBy",
                table: "Issues",
                column: "ResolvedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
