using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.CustomerOutlet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customer_outlet");

            migrationBuilder.CreateTable(
                name: "customer_clusters",
                schema: "customer_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cluster_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    cluster_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_customer_clusters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_relationships",
                schema: "customer_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    primary_customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    related_customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relationship_type = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_customer_relationships", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "customer_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_name1 = table.Column<string>(type: "text", nullable: true),
                    customer_name2 = table.Column<string>(type: "text", nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    vat_number = table.Column<string>(type: "text", nullable: true),
                    wholesaler_code = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    customer_cluster_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_customer_clusters_customer_cluster_id",
                        column: x => x.customer_cluster_id,
                        principalSchema: "customer_outlet",
                        principalTable: "customer_clusters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "outlets",
                schema: "customer_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    outlet_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    outlet_type = table.Column<string>(type: "text", nullable: true),
                    address_line = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    sales_territory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sub_trade_channel = table.Column<string>(type: "text", nullable: true),
                    segmentation = table.Column<string>(type: "text", nullable: true),
                    seasonality = table.Column<string>(type: "text", nullable: true),
                    payer_code = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_outlets", x => x.id);
                    table.ForeignKey(
                        name: "fk_outlets_customers_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "customer_outlet",
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "outlet_notes",
                schema: "customer_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    related_asset_id = table.Column<Guid>(type: "uuid", nullable: true),
                    note_body = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_outlet_notes", x => x.id);
                    table.ForeignKey(
                        name: "fk_outlet_notes_outlets_outlet_id",
                        column: x => x.outlet_id,
                        principalSchema: "customer_outlet",
                        principalTable: "outlets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_clusters_cluster_code",
                schema: "customer_outlet",
                table: "customer_clusters",
                column: "cluster_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_customer_cluster_id",
                schema: "customer_outlet",
                table: "customers",
                column: "customer_cluster_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_customer_code",
                schema: "customer_outlet",
                table: "customers",
                column: "customer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outlet_notes_outlet_id",
                schema: "customer_outlet",
                table: "outlet_notes",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "ix_outlets_customer_id",
                schema: "customer_outlet",
                table: "outlets",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_outlets_outlet_code",
                schema: "customer_outlet",
                table: "outlets",
                column: "outlet_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_relationships",
                schema: "customer_outlet");

            migrationBuilder.DropTable(
                name: "outlet_notes",
                schema: "customer_outlet");

            migrationBuilder.DropTable(
                name: "outlets",
                schema: "customer_outlet");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "customer_outlet");

            migrationBuilder.DropTable(
                name: "customer_clusters",
                schema: "customer_outlet");
        }
    }
}
