using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PropertyPolicies_PropertyPolicyId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_PolicyEligibilities_EligibilityId",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_Users_UserId",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_Users_VerifiedBy",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalExaminations_Users_UserId",
                table: "MedicalExaminations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PropertyPolicies_PropertyPolicyId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                table: "PolicyEligibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyEligibilities_Users_UserId1",
                table: "PolicyEligibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases");

            migrationBuilder.DropForeignKey(
                name: "FK_UnderwritingCases_Users_AssignedUnderwriterId",
                table: "UnderwritingCases");

            migrationBuilder.DropTable(
                name: "FraudAlerts");

            migrationBuilder.DropTable(
                name: "PropertyPolicies");

            migrationBuilder.DropTable(
                name: "UserRiskProfiles");

            migrationBuilder.DropIndex(
                name: "IX_PolicyEligibilities_UserId1",
                table: "PolicyEligibilities");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PropertyPolicyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVerifications_EligibilityId",
                table: "DocumentVerifications");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVerifications_UserId",
                table: "DocumentVerifications");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PropertyPolicyId",
                table: "Documents");

            migrationBuilder.DeleteData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD010");

            migrationBuilder.DeleteData(
                table: "PolicyProducts",
                keyColumn: "ProductId",
                keyValue: "PRD011");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PolicyEligibilities");

            migrationBuilder.DropColumn(
                name: "PropertyPolicyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EligibilityId",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "IsAutoVerified",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "IsDocumentAuthentic",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "IsDocumentClear",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocumentVerifications");

            migrationBuilder.DropColumn(
                name: "PropertyPolicyId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "VerifiedBy",
                table: "DocumentVerifications",
                newName: "UnderwriterId");

            migrationBuilder.RenameColumn(
                name: "IsDocumentComplete",
                table: "DocumentVerifications",
                newName: "IsVerified");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentVerifications_VerifiedBy",
                table: "DocumentVerifications",
                newName: "IX_DocumentVerifications_UnderwriterId");

            migrationBuilder.AddColumn<string>(
                name: "AgentId",
                table: "Proposals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentId",
                table: "Policies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUnderwriterId",
                table: "MedicalExaminations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ConfidenceScore",
                table: "DocumentVerifications",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AgentId",
                table: "Claims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnderwriterId",
                table: "Claims",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    QuoteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SumAssured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TermYears = table.Column<int>(type: "int", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumFrequency = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConverted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.QuoteId);
                    table.ForeignKey(
                        name: "FK_Quotes_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotes_Users_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: "CLM001",
                columns: new[] { "AgentId", "Status", "UnderwriterId" },
                values: new object[] { null, "Pending", null });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "PolicyId",
                keyValue: "POL001",
                column: "AgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "PolicyId",
                keyValue: "POL002",
                column: "AgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "PolicyId",
                keyValue: "POL003",
                column: "AgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "ProposalId",
                keyValue: "POS001",
                column: "AgentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "ProposalId",
                keyValue: "POS002",
                column: "AgentId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_AgentId",
                table: "Proposals",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_AgentId",
                table: "Policies",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_AssignedUnderwriterId",
                table: "MedicalExaminations",
                column: "AssignedUnderwriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_AgentId",
                table: "Claims",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UnderwriterId",
                table: "Claims",
                column: "UnderwriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_AgentId",
                table: "Quotes",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_ProductId",
                table: "Quotes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_UserId",
                table: "Quotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_AgentId",
                table: "Claims",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_UnderwriterId",
                table: "Claims",
                column: "UnderwriterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_Users_UnderwriterId",
                table: "DocumentVerifications",
                column: "UnderwriterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalExaminations_Users_AssignedUnderwriterId",
                table: "MedicalExaminations",
                column: "AssignedUnderwriterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalExaminations_Users_UserId",
                table: "MedicalExaminations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Users_AgentId",
                table: "Policies",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                table: "PolicyEligibilities",
                column: "ProductId",
                principalTable: "PolicyProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Users_AgentId",
                table: "Proposals",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases",
                column: "EligibilityId",
                principalTable: "PolicyEligibilities",
                principalColumn: "EligibilityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnderwritingCases_Users_AssignedUnderwriterId",
                table: "UnderwritingCases",
                column: "AssignedUnderwriterId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_AgentId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_UnderwriterId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_Users_UnderwriterId",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalExaminations_Users_AssignedUnderwriterId",
                table: "MedicalExaminations");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalExaminations_Users_UserId",
                table: "MedicalExaminations");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Users_AgentId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                table: "PolicyEligibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Users_AgentId",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases");

            migrationBuilder.DropForeignKey(
                name: "FK_UnderwritingCases_Users_AssignedUnderwriterId",
                table: "UnderwritingCases");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_AgentId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Policies_AgentId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_MedicalExaminations_AssignedUnderwriterId",
                table: "MedicalExaminations");

            migrationBuilder.DropIndex(
                name: "IX_Claims_AgentId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_UnderwriterId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "AssignedUnderwriterId",
                table: "MedicalExaminations");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "UnderwriterId",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "UnderwriterId",
                table: "DocumentVerifications",
                newName: "VerifiedBy");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "DocumentVerifications",
                newName: "IsDocumentComplete");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentVerifications_UnderwriterId",
                table: "DocumentVerifications",
                newName: "IX_DocumentVerifications_VerifiedBy");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "PolicyEligibilities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyPolicyId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConfidenceScore",
                table: "DocumentVerifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "EligibilityId",
                table: "DocumentVerifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoVerified",
                table: "DocumentVerifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentAuthentic",
                table: "DocumentVerifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentClear",
                table: "DocumentVerifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "DocumentVerifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DocumentVerifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyPolicyId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FraudAlerts",
                columns: table => new
                {
                    AlertId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AlertReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedInvestigator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetectionMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvidenceDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraudScore = table.Column<int>(type: "int", nullable: false),
                    InvestigationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccountBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsClaimRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsReportedToAuthorities = table.Column<bool>(type: "bit", nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraudAlerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_FraudAlerts_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "ClaimId");
                    table.ForeignKey(
                        name: "FK_FraudAlerts_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId");
                    table.ForeignKey(
                        name: "FK_FraudAlerts_PolicyEligibilities_EligibilityId",
                        column: x => x.EligibilityId,
                        principalTable: "PolicyEligibilities",
                        principalColumn: "EligibilityId");
                    table.ForeignKey(
                        name: "FK_FraudAlerts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyPolicies",
                columns: table => new
                {
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuiltUpArea = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConstructionType = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasFireCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasFireSafety = table.Column<bool>(type: "bit", nullable: false),
                    HasNaturalCalamityCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasSecuritySystem = table.Column<bool>(type: "bit", nullable: false),
                    HasTerrorismCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasTheftCoverage = table.Column<bool>(type: "bit", nullable: false),
                    IsInEarthquakeZone = table.Column<bool>(type: "bit", nullable: false),
                    IsInFloodProneArea = table.Column<bool>(type: "bit", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfFloors = table.Column<int>(type: "int", nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PropertyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyAge = table.Column<int>(type: "int", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    PropertyValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SumInsured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VoluntaryDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyPolicies", x => x.PropertyPolicyId);
                    table.ForeignKey(
                        name: "FK_PropertyPolicies_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyPolicies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRiskProfiles",
                columns: table => new
                {
                    UserRiskProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgeRiskScore = table.Column<int>(type: "int", nullable: false),
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumesAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreditScore = table.Column<int>(type: "int", nullable: false),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialRiskScore = table.Column<int>(type: "int", nullable: false),
                    HasPreviousClaims = table.Column<bool>(type: "bit", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: false),
                    LifestyleRiskScore = table.Column<int>(type: "int", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalRiskScore = table.Column<int>(type: "int", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccupationRiskScore = table.Column<int>(type: "int", nullable: false),
                    OverallRiskScore = table.Column<int>(type: "int", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousClaimsAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousClaimsCount = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRiskProfiles", x => x.UserRiskProfileId);
                    table.ForeignKey(
                        name: "FK_UserRiskProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRiskProfiles_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: "CLM001",
                column: "Status",
                value: "Filed");

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC001",
                column: "PropertyPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC003",
                column: "PropertyPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY001",
                column: "PropertyPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY002",
                column: "PropertyPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY003",
                column: "PropertyPolicyId",
                value: null);

            migrationBuilder.InsertData(
                table: "PolicyProducts",
                columns: new[] { "ProductId", "BaseRate", "Category", "CompanyName", "CreatedAt", "HasDeathBenefit", "HasMaturityBenefit", "InsuranceType", "IsActive", "MaxAge", "MaxSumAssured", "MaxTerm", "MinAge", "MinSumAssured", "MinTerm", "ProductName", "RiskLevel" },
                values: new object[,]
                {
                    { "PRD010", 2.5m, 6, "Bajaj Allianz", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 2, true, 75, 100000000m, 1, 18, 100000m, 1, "Home Insurance Comprehensive", 1 },
                    { "PRD011", 4.0m, 6, "New India Assurance", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 2, true, 75, 500000000m, 1, 18, 500000m, 1, "Commercial Property Insurance", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_UserId1",
                table: "PolicyEligibilities",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PropertyPolicyId",
                table: "Payments",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_EligibilityId",
                table: "DocumentVerifications",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_UserId",
                table: "DocumentVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PropertyPolicyId",
                table: "Documents",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_FraudAlerts_ClaimId",
                table: "FraudAlerts",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_FraudAlerts_EligibilityId",
                table: "FraudAlerts",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FraudAlerts_PolicyId",
                table: "FraudAlerts",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_FraudAlerts_UserId",
                table: "FraudAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPolicies_ProductId",
                table: "PropertyPolicies",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPolicies_UserId",
                table: "PropertyPolicies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRiskProfiles_UserId",
                table: "UserRiskProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRiskProfiles_UserId1",
                table: "UserRiskProfiles",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PropertyPolicies_PropertyPolicyId",
                table: "Documents",
                column: "PropertyPolicyId",
                principalTable: "PropertyPolicies",
                principalColumn: "PropertyPolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_PolicyEligibilities_EligibilityId",
                table: "DocumentVerifications",
                column: "EligibilityId",
                principalTable: "PolicyEligibilities",
                principalColumn: "EligibilityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_Users_UserId",
                table: "DocumentVerifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_Users_VerifiedBy",
                table: "DocumentVerifications",
                column: "VerifiedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalExaminations_Users_UserId",
                table: "MedicalExaminations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PropertyPolicies_PropertyPolicyId",
                table: "Payments",
                column: "PropertyPolicyId",
                principalTable: "PropertyPolicies",
                principalColumn: "PropertyPolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                table: "PolicyEligibilities",
                column: "ProductId",
                principalTable: "PolicyProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyEligibilities_Users_UserId1",
                table: "PolicyEligibilities",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                table: "UnderwritingCases",
                column: "EligibilityId",
                principalTable: "PolicyEligibilities",
                principalColumn: "EligibilityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnderwritingCases_Users_AssignedUnderwriterId",
                table: "UnderwritingCases",
                column: "AssignedUnderwriterId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
