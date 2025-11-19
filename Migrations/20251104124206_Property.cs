using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD010",
                column: "InsuranceType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD011",
                column: "InsuranceType",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD010",
                column: "InsuranceType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD011",
                column: "InsuranceType",
                value: 2);
        }
    }
}
