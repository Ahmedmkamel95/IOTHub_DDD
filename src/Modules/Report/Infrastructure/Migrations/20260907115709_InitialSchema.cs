using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Report.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "report");

            migrationBuilder.CreateTable(
                name: "export_jobs",
                schema: "report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    export_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    download_url = table.Column<string>(type: "text", nullable: true),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_export_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_definitions",
                schema: "report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    report_code = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    report_type = table.Column<string>(type: "text", nullable: false),
                    configuration_json = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_runs",
                schema: "report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    report_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    result_uri = table.Column<string>(type: "text", nullable: true),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_runs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_report_definitions_report_code",
                schema: "report",
                table: "report_definitions",
                column: "report_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_report_runs_report_definition_id_status",
                schema: "report",
                table: "report_runs",
                columns: new[] { "report_definition_id", "status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "export_jobs",
                schema: "report");

            migrationBuilder.DropTable(
                name: "report_definitions",
                schema: "report");

            migrationBuilder.DropTable(
                name: "report_runs",
                schema: "report");
        }
    }
}
