using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldJobs",
                columns: table => new
                {
                    FieldJobId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PickRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Scheduled"),
                    Verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldJobs", x => x.FieldJobId);
                    table.ForeignKey(
                        name: "FK_FieldJobs_ClientLocations",
                        column: x => x.ClientLocationId,
                        principalTable: "ClientLocations",
                        principalColumn: "ClientLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldJobs_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldJobs_PickRequests",
                        column: x => x.PickRequestId,
                        principalTable: "PickRequests",
                        principalColumn: "PickRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldJobs_Supervisor",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldJobs_Technician",
                        column: x => x.TechnicianId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_ClientId",
                table: "FieldJobs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_ClientLocationId",
                table: "FieldJobs",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_PickRequestId",
                table: "FieldJobs",
                column: "PickRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_SupervisorId",
                table: "FieldJobs",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldJobs_TechnicianId",
                table: "FieldJobs",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "UQ_FieldJobs_JobNumber",
                table: "FieldJobs",
                column: "JobNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldJobs");
        }
    }
}
