using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class noproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PropertyClaims_PropertyClaimId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "PropertyClaims");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PropertyClaimId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "PropertyClaimId",
                table: "Documents");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyClaimId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertyClaims",
                columns: table => new
                {
                    PropertyClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CauseOfLoss = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClaimType = table.Column<int>(type: "int", nullable: false),
                    ContractorContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DamageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedLoss = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FIRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FIRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FireDepartmentReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncidentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestigationFindings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepairCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairEstimate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RepairStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiresInvestigation = table.Column<bool>(type: "bit", nullable: false),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SurveyorAssessment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyClaims", x => x.PropertyClaimId);
                    table.ForeignKey(
                        name: "FK_PropertyClaims_PropertyPolicies_PropertyPolicyId",
                        column: x => x.PropertyPolicyId,
                        principalTable: "PropertyPolicies",
                        principalColumn: "PropertyPolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC001",
                column: "PropertyClaimId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC003",
                column: "PropertyClaimId",
                value: null);

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

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PropertyClaimId",
                table: "Documents",
                column: "PropertyClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClaims_PropertyPolicyId",
                table: "PropertyClaims",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClaims_UserId",
                table: "PropertyClaims",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PropertyClaims_PropertyClaimId",
                table: "Documents",
                column: "PropertyClaimId",
                principalTable: "PropertyClaims",
                principalColumn: "PropertyClaimId");
        }
    }
}
