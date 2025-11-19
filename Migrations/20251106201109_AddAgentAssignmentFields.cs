using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentAssignmentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AgentAssignedDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAgentId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE001",
                columns: new[] { "AgentAssignedDate", "AssignedAgentId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE002",
                columns: new[] { "AgentAssignedDate", "AssignedAgentId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE003",
                columns: new[] { "AgentAssignedDate", "AssignedAgentId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE004",
                columns: new[] { "AgentAssignedDate", "AssignedAgentId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "USE005",
                columns: new[] { "AgentAssignedDate", "AssignedAgentId" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentAssignedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignedAgentId",
                table: "Users");
        }
    }
}
