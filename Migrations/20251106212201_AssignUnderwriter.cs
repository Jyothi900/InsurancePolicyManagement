using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class AssignUnderwriter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUnderwriterId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnderwriterAssignedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE001",
                columns: new[] { "AssignedUnderwriterId", "UnderwriterAssignedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE002",
                columns: new[] { "AssignedUnderwriterId", "UnderwriterAssignedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE003",
                columns: new[] { "AssignedUnderwriterId", "UnderwriterAssignedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE004",
                columns: new[] { "AssignedUnderwriterId", "UnderwriterAssignedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE005",
                columns: new[] { "AssignedUnderwriterId", "UnderwriterAssignedDate" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUnderwriterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnderwriterAssignedDate",
                table: "Users");
        }
    }
}
