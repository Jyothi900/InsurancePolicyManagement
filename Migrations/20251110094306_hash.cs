using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class hash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases");

            migrationBuilder.DropTable(
                name: "MedicalExaminations");

            migrationBuilder.DropTable(
                name: "PolicyEligibilities");

            migrationBuilder.DropIndex(
                name: "IX_UnderwritingCases_EligibilityId",
                table: "UnderwritingCases");

            migrationBuilder.DropColumn(
                name: "EligibilityId",
                table: "UnderwritingCases");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "EligibilityId",
                table: "UnderwritingCases",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PolicyEligibilities",
                columns: table => new
                {
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CalculatedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    MaxEligibleSumAssured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedSumAssured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiredDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresIncomeProof = table.Column<bool>(type: "bit", nullable: false),
                    RequiresMedicalExam = table.Column<bool>(type: "bit", nullable: false),
                    RequiresUnderwriting = table.Column<bool>(type: "bit", nullable: false),
                    RiskMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyEligibilities", x => x.EligibilityId);
                    table.ForeignKey(
                        name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyEligibilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalExaminations",
                columns: table => new
                {
                    ExaminationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedUnderwriterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BMI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodTestResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChestXRayResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECGResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExaminationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MedicalCenter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalOpinion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalRiskScore = table.Column<int>(type: "int", nullable: false),
                    PremiumLoadingPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PulseRate = table.Column<int>(type: "int", nullable: false),
                    RecommendedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrineTestResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalExaminations", x => x.ExaminationId);
                    table.ForeignKey(
                        name: "FK_MedicalExaminations_PolicyEligibilities_EligibilityId",
                        column: x => x.EligibilityId,
                        principalTable: "PolicyEligibilities",
                        principalColumn: "EligibilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalExaminations_Users_AssignedUnderwriterId",
                        column: x => x.AssignedUnderwriterId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalExaminations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnderwritingCases_EligibilityId",
                table: "UnderwritingCases",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_AssignedUnderwriterId",
                table: "MedicalExaminations",
                column: "AssignedUnderwriterId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_EligibilityId",
                table: "MedicalExaminations",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_UserId",
                table: "MedicalExaminations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_ProductId",
                table: "PolicyEligibilities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_UserId",
                table: "PolicyEligibilities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases",
                column: "EligibilityId",
                principalTable: "PolicyEligibilities",
                principalColumn: "EligibilityId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
