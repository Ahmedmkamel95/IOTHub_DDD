using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Org.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "org");

            migrationBuilder.CreateTable(
                name: "business_unit_countries",
                schema: "org",
                columns: table => new
                {
                    business_unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_business_unit_countries", x => new { x.business_unit_id, x.country_code });
                });

            migrationBuilder.CreateTable(
                name: "countries",
                schema: "org",
                columns: table => new
                {
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    country_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    default_timezone = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_countries", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "sales_organizations",
                schema: "org",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sales_organization_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_sales_organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "business_units",
                schema: "org",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    business_unit_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    business_unit_name = table.Column<string>(type: "text", nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("pk_business_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_business_units_countries_country_code",
                        column: x => x.country_code,
                        principalSchema: "org",
                        principalTable: "countries",
                        principalColumn: "country_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sales_territories",
                schema: "org",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    territory_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    territory_name = table.Column<string>(type: "text", nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("pk_sales_territories", x => x.id);
                    table.ForeignKey(
                        name: "fk_sales_territories_countries_country_code",
                        column: x => x.country_code,
                        principalSchema: "org",
                        principalTable: "countries",
                        principalColumn: "country_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_business_units_business_unit_code",
                schema: "org",
                table: "business_units",
                column: "business_unit_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_business_units_country_code",
                schema: "org",
                table: "business_units",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "ix_sales_organizations_sales_organization_code",
                schema: "org",
                table: "sales_organizations",
                column: "sales_organization_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sales_territories_country_code",
                schema: "org",
                table: "sales_territories",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "ix_sales_territories_territory_code",
                schema: "org",
                table: "sales_territories",
                column: "territory_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "business_unit_countries",
                schema: "org");

            migrationBuilder.DropTable(
                name: "business_units",
                schema: "org");

            migrationBuilder.DropTable(
                name: "sales_organizations",
                schema: "org");

            migrationBuilder.DropTable(
                name: "sales_territories",
                schema: "org");

            migrationBuilder.DropTable(
                name: "countries",
                schema: "org");
        }
    }
}
