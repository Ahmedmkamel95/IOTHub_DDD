using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Mobile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mobile");

            migrationBuilder.CreateTable(
                name: "device_replacements",
                schema: "mobile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    old_device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    new_device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    replaced_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    replaced_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("pk_device_replacements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "offline_batches",
                schema: "mobile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    technician_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_client_session_id = table.Column<string>(type: "text", nullable: false),
                    action_count = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    processed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("pk_offline_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "offline_action_results",
                schema: "mobile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    offline_batch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_action_id = table.Column<string>(type: "text", nullable: false),
                    action_type = table.Column<string>(type: "text", nullable: false),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_offline_action_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_offline_action_results_offline_batches_offline_batch_id",
                        column: x => x.offline_batch_id,
                        principalSchema: "mobile",
                        principalTable: "offline_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_device_replacements_asset_id_replaced_at_utc",
                schema: "mobile",
                table: "device_replacements",
                columns: new[] { "asset_id", "replaced_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_offline_action_results_offline_batch_id_client_action_id",
                schema: "mobile",
                table: "offline_action_results",
                columns: new[] { "offline_batch_id", "client_action_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_replacements",
                schema: "mobile");

            migrationBuilder.DropTable(
                name: "offline_action_results",
                schema: "mobile");

            migrationBuilder.DropTable(
                name: "offline_batches",
                schema: "mobile");
        }
    }
}
