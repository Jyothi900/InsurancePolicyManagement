using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyProducts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    InsuranceType = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    MinSumAssured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxSumAssured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinTerm = table.Column<int>(type: "int", nullable: false),
                    MaxTerm = table.Column<int>(type: "int", nullable: false),
                    BaseRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HasMaturityBenefit = table.Column<bool>(type: "bit", nullable: false),
                    HasDeathBenefit = table.Column<bool>(type: "bit", nullable: false),
                    RiskLevel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyProducts", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PANNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KYCStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "PremiumConfigurations",
                columns: table => new
                {
                    ConfigId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConfigType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumConfigurations", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_PremiumConfigurations_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotorPolicies",
                columns: table => new
                {
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturingYear = table.Column<int>(type: "int", nullable: false),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    IDV = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    NCBPercentage = table.Column<int>(type: "int", nullable: false),
                    HasZeroDepreciation = table.Column<bool>(type: "bit", nullable: false),
                    HasEngineProtection = table.Column<bool>(type: "bit", nullable: false),
                    HasRoadSideAssistance = table.Column<bool>(type: "bit", nullable: false),
                    VoluntaryDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorPolicies", x => x.MotorPolicyId);
                    table.ForeignKey(
                        name: "FK_MotorPolicies_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MotorPolicies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    SumAssured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumFrequency = table.Column<int>(type: "int", nullable: false),
                    TermYears = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextPremiumDue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_Policies_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Policies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PolicyEligibilities",
                columns: table => new
                {
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestedSumAssured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxEligibleSumAssured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalculatedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RiskMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiresMedicalExam = table.Column<bool>(type: "bit", nullable: false),
                    RequiresIncomeProof = table.Column<bool>(type: "bit", nullable: false),
                    RequiresUnderwriting = table.Column<bool>(type: "bit", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyEligibilities", x => x.EligibilityId);
                    table.ForeignKey(
                        name: "FK_PolicyEligibilities_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolicyEligibilities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyEligibilities_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PropertyPolicies",
                columns: table => new
                {
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    PropertyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SumInsured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConstructionType = table.Column<int>(type: "int", nullable: false),
                    PropertyAge = table.Column<int>(type: "int", nullable: false),
                    BuiltUpArea = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    NumberOfFloors = table.Column<int>(type: "int", nullable: true),
                    HasSecuritySystem = table.Column<bool>(type: "bit", nullable: false),
                    HasFireSafety = table.Column<bool>(type: "bit", nullable: false),
                    IsInFloodProneArea = table.Column<bool>(type: "bit", nullable: false),
                    IsInEarthquakeZone = table.Column<bool>(type: "bit", nullable: false),
                    HasFireCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasTheftCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasNaturalCalamityCoverage = table.Column<bool>(type: "bit", nullable: false),
                    HasTerrorismCoverage = table.Column<bool>(type: "bit", nullable: false),
                    VoluntaryDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                name: "Proposals",
                columns: table => new
                {
                    ProposalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SumAssured = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TermYears = table.Column<int>(type: "int", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumFrequency = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: false),
                    IsDrinker = table.Column<bool>(type: "bit", nullable: false),
                    PreExistingConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UnderwritingNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.ProposalId);
                    table.ForeignKey(
                        name: "FK_Proposals_PolicyProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PolicyProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proposals_Users_UserId",
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
                    AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: false),
                    ConsumesAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasPreviousClaims = table.Column<bool>(type: "bit", nullable: false),
                    PreviousClaimsCount = table.Column<int>(type: "int", nullable: false),
                    PreviousClaimsAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditScore = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeRiskScore = table.Column<int>(type: "int", nullable: false),
                    MedicalRiskScore = table.Column<int>(type: "int", nullable: false),
                    OccupationRiskScore = table.Column<int>(type: "int", nullable: false),
                    LifestyleRiskScore = table.Column<int>(type: "int", nullable: false),
                    FinancialRiskScore = table.Column<int>(type: "int", nullable: false),
                    OverallRiskScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "MotorClaims",
                columns: table => new
                {
                    MotorClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimType = table.Column<int>(type: "int", nullable: false),
                    AccidentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SurveyorAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ThirdPartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdPartyVehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdPartyClaimAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    GarageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GarageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCashlessRepair = table.Column<bool>(type: "bit", nullable: false),
                    FIRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorClaims", x => x.MotorClaimId);
                    table.ForeignKey(
                        name: "FK_MotorClaims_MotorPolicies_MotorPolicyId",
                        column: x => x.MotorPolicyId,
                        principalTable: "MotorPolicies",
                        principalColumn: "MotorPolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MotorClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FiledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CauseOfDeath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClaimantRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimantBankDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresInvestigation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_Claims_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Users_UserId",
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExaminationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalCenter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BMI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PulseRate = table.Column<int>(type: "int", nullable: false),
                    BloodTestResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrineTestResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECGResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChestXRayResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalOpinion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecommendedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalRiskScore = table.Column<int>(type: "int", nullable: false),
                    PremiumLoadingPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_MedicalExaminations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnderwritingCases",
                columns: table => new
                {
                    CaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedUnderwriterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedSumAssured = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderwriterNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskAssessment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnderwritingCases", x => x.CaseId);
                    table.ForeignKey(
                        name: "FK_UnderwritingCases_PolicyEligibilities_EligibilityId",
                        column: x => x.EligibilityId,
                        principalTable: "PolicyEligibilities",
                        principalColumn: "EligibilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnderwritingCases_Users_AssignedUnderwriterId",
                        column: x => x.AssignedUnderwriterId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UnderwritingCases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentGateway = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_MotorPolicies_MotorPolicyId",
                        column: x => x.MotorPolicyId,
                        principalTable: "MotorPolicies",
                        principalColumn: "MotorPolicyId");
                    table.ForeignKey(
                        name: "FK_Payments_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PropertyPolicies_PropertyPolicyId",
                        column: x => x.PropertyPolicyId,
                        principalTable: "PropertyPolicies",
                        principalColumn: "PropertyPolicyId");
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyClaims",
                columns: table => new
                {
                    PropertyClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimType = table.Column<int>(type: "int", nullable: false),
                    IncidentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedLoss = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SurveyorAssessment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CauseOfLoss = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresInvestigation = table.Column<bool>(type: "bit", nullable: false),
                    InvestigationFindings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FireDepartmentReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepairEstimate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RepairStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Nominees",
                columns: table => new
                {
                    NomineeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProposalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Relationship = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    SharePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    AadhaarNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominees", x => x.NomineeId);
                    table.ForeignKey(
                        name: "FK_Nominees_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nominees_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FraudAlerts",
                columns: table => new
                {
                    AlertId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FraudScore = table.Column<int>(type: "int", nullable: false),
                    DetectionMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvidenceDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedInvestigator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccountBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsClaimRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsReportedToAuthorities = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProposalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerificationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotorClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PropertyClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PropertyPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_MotorClaims_MotorClaimId",
                        column: x => x.MotorClaimId,
                        principalTable: "MotorClaims",
                        principalColumn: "MotorClaimId");
                    table.ForeignKey(
                        name: "FK_Documents_MotorPolicies_MotorPolicyId",
                        column: x => x.MotorPolicyId,
                        principalTable: "MotorPolicies",
                        principalColumn: "MotorPolicyId");
                    table.ForeignKey(
                        name: "FK_Documents_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_PropertyClaims_PropertyClaimId",
                        column: x => x.PropertyClaimId,
                        principalTable: "PropertyClaims",
                        principalColumn: "PropertyClaimId");
                    table.ForeignKey(
                        name: "FK_Documents_PropertyPolicies_PropertyPolicyId",
                        column: x => x.PropertyPolicyId,
                        principalTable: "PropertyPolicies",
                        principalColumn: "PropertyPolicyId");
                    table.ForeignKey(
                        name: "FK_Documents_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVerifications",
                columns: table => new
                {
                    VerificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EligibilityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDocumentClear = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentComplete = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentAuthentic = table.Column<bool>(type: "bit", nullable: false),
                    VerificationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAutoVerified = table.Column<bool>(type: "bit", nullable: false),
                    ConfidenceScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVerifications", x => x.VerificationId);
                    table.ForeignKey(
                        name: "FK_DocumentVerifications_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentVerifications_PolicyEligibilities_EligibilityId",
                        column: x => x.EligibilityId,
                        principalTable: "PolicyEligibilities",
                        principalColumn: "EligibilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentVerifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentVerifications_Users_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PolicyProducts",
                columns: new[] { "ProductId", "BaseRate", "Category", "CompanyName", "CreatedAt", "HasDeathBenefit", "HasMaturityBenefit", "InsuranceType", "IsActive", "MaxAge", "MaxSumAssured", "MaxTerm", "MinAge", "MinSumAssured", "MinTerm", "ProductName", "RiskLevel" },
                values: new object[,]
                {
                    { "PRD001", 4.5m, 0, "LIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, 0, true, 65, 10000000m, 30, 18, 500000m, 10, "Tech Term", 0 },
                    { "PRD002", 45.2m, 1, "LIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 0, true, 50, 5000000m, 35, 18, 100000m, 15, "Jeevan Anand", 0 },
                    { "PRD003", 52.8m, 3, "LIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 0, true, 45, 3000000m, 20, 13, 100000m, 20, "New Money Back 20", 0 },
                    { "PRD004", 15.5m, 2, "HDFC Life", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 0, true, 60, 25000000m, 30, 18, 500000m, 10, "Click2Wealth", 2 },
                    { "PRD005", 85.5m, 4, "LIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 0, true, 79, 10000000m, 40, 30, 150000m, 10, "Jeevan Shanti", 0 },
                    { "PRD008", 3.5m, 6, "HDFC ERGO", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 1, true, 75, 10000000m, 1, 18, 50000m, 1, "Comprehensive Motor Insurance", 1 },
                    { "PRD009", 1.5m, 6, "ICICI Lombard", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 1, true, 75, 0m, 1, 18, 0m, 1, "Third Party Motor Insurance", 0 },
                    { "PRD010", 2.5m, 6, "Bajaj Allianz", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 2, true, 75, 100000000m, 1, 18, 100000m, 1, "Home Insurance Comprehensive", 1 },
                    { "PRD011", 4.0m, 6, "New India Assurance", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 2, true, 75, 500000000m, 1, 18, 500000m, 1, "Commercial Property Insurance", 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AadhaarNumber", "Address", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IsActive", "KYCStatus", "Mobile", "PANNumber", "Password", "ProfileImagePath", "Role" },
                values: new object[,]
                {
                    { "USE001", "123456789012", "12-34, MG Road, Hyderabad, Telangana - 500001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1985, 3, 15), "rajeshreddy@gmail.com", "Rajesh Kumar Reddy", 0, true, 1, "9876543210", "ABCDE1234F", "Password123!", null, 0 },
                    { "USE002", "123456789013", "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1990, 7, 22), "priyanair@gmail.com", "Priya Lakshmi Nair", 1, true, 1, "9876543211", "BCDEF2345G", "Password123!", null, 0 },
                    { "USE003", "123456789014", "78-90, Brigade Road, Bangalore, Karnataka - 560001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1982, 11, 8), "venkateshiyer@gmail.com", "Venkatesh Iyer", 0, true, 1, "9876543212", "CDEFG3456H", "Password123!", null, 1 },
                    { "USE004", "123456789015", "23-45, Marine Drive, Kochi, Kerala - 682001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1988, 5, 12), "meerakrishnan@gmail.com", "Meera Krishnan", 1, true, 0, "9876543213", "DEFGH4567I", "Password123!", null, 3 },
                    { "USE005", "123456789016", "56-78, IT Corridor, Tirupathi, Andhra Pradesh - 500081", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1992, 9, 25), "jytohireddy@gmail.com", "Jyothi Reddy", 0, true, 1, "9876543214", "EFGHI5678J", "Admin123!", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "PolicyId", "ExpiryDate", "IssuedDate", "NextPremiumDue", "PolicyNumber", "PolicyType", "PremiumAmount", "PremiumFrequency", "ProductId", "StartDate", "Status", "SumAssured", "TermYears", "UserId" },
                values: new object[,]
                {
                    { "POL001", new DateTime(2044, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC/TERM/2024/001", 0, 22500m, 3, "PRD001", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 5000000m, 20, "USE001" },
                    { "POL002", new DateTime(2044, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC/END/2024/002", 1, 45200m, 3, "PRD002", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1000000m, 20, "USE002" },
                    { "POL003", new DateTime(2039, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "HDFC/ULIP/2024/003", 2, 38750m, 3, "PRD004", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 2500000m, 15, "USE003" }
                });

            migrationBuilder.InsertData(
                table: "Proposals",
                columns: new[] { "ProposalId", "AnnualIncome", "AppliedDate", "Height", "IsDrinker", "IsSmoker", "Occupation", "PreExistingConditions", "PremiumAmount", "PremiumFrequency", "ProductId", "ReviewedDate", "Status", "SumAssured", "TermYears", "UnderwritingNotes", "UserId", "Weight" },
                values: new object[,]
                {
                    { "POS001", 800000m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 165m, false, false, "Software Engineer", null, 26400m, 3, "PRD003", null, 20, 500000m, 20, null, "USE004", 60m },
                    { "POS002", 1200000m, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 175m, true, false, "Business Analyst", null, 85500m, 3, "PRD005", null, 21, 1000000m, 25, null, "USE005", 75m }
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimId", "ApprovedAmount", "CauseOfDeath", "ClaimAmount", "ClaimNumber", "ClaimType", "ClaimantBankDetails", "ClaimantName", "ClaimantRelation", "FiledDate", "IncidentDate", "InvestigationNotes", "PolicyId", "ProcessedDate", "RejectionReason", "RequiresInvestigation", "Status", "UserId" },
                values: new object[] { "CLM001", null, "Natural", 5000000m, "CLM/2024/001", "Death", "SBI Account: 12345678901", "Lakshmi Rajesh Reddy", "Spouse", new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "POL001", null, null, false, "Filed", "USE001" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "DocumentId", "ClaimId", "DocumentType", "FileHash", "FileName", "FilePath", "MotorClaimId", "MotorPolicyId", "PolicyId", "PropertyClaimId", "PropertyPolicyId", "ProposalId", "Status", "UploadedAt", "UserId", "VerificationNotes", "VerifiedAt" },
                values: new object[,]
                {
                    { "DOC001", null, 10, null, "policy_POL001.pdf", "/uploads/documents/policy_POL001.pdf", null, null, "POL001", null, null, null, 19, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "USE001", null, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "DOC003", null, 4, null, "medical_report_POS001.pdf", "/uploads/documents/medical_report_POS001.pdf", null, null, null, null, null, "POS001", 19, new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "USE004", null, new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Nominees",
                columns: new[] { "NomineeId", "AadhaarNumber", "Address", "CreatedAt", "DateOfBirth", "FullName", "PolicyId", "ProposalId", "Relationship", "SharePercentage" },
                values: new object[,]
                {
                    { "NOM001", "123456789017", "12-34, MG Road, Hyderabad, Telangana - 500001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1987, 6, 20), "Lakshmi Rajesh Reddy", "POL001", null, 1, 100m },
                    { "NOM002", "123456789018", "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1988, 4, 15), "Suresh Kumar Nair", "POL002", null, 1, 60m },
                    { "NOM003", "123456789019", "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2015, 8, 10), "Aadhya Priya Nair", "POL002", null, 2, 40m },
                    { "NOM004", "123456789020", "78-90, Brigade Road, Bangalore, Karnataka - 560001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1984, 12, 3), "Kavitha Venkatesh Iyer", "POL003", null, 1, 100m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "DueDate", "FailureReason", "MotorPolicyId", "NextDueDate", "PaymentDate", "PaymentGateway", "PaymentMethod", "PaymentType", "PolicyId", "ProcessedDate", "PropertyPolicyId", "Status", "TransactionId", "UserId" },
                values: new object[,]
                {
                    { "PAY001", 22500m, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razorpay", "UPI", "Premium", "POL001", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Success", "TXN001234567890", "USE001" },
                    { "PAY002", 45200m, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paytm", "NetBanking", "Premium", "POL002", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Success", "TXN001234567891", "USE002" },
                    { "PAY003", 38750m, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razorpay", "Card", "Premium", "POL003", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Success", "TXN001234567892", "USE003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PolicyId",
                table: "Claims",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserId",
                table: "Claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClaimId",
                table: "Documents",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MotorClaimId",
                table: "Documents",
                column: "MotorClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MotorPolicyId",
                table: "Documents",
                column: "MotorPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PolicyId",
                table: "Documents",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PropertyClaimId",
                table: "Documents",
                column: "PropertyClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PropertyPolicyId",
                table: "Documents",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProposalId",
                table: "Documents",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_DocumentId",
                table: "DocumentVerifications",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_EligibilityId",
                table: "DocumentVerifications",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_UserId",
                table: "DocumentVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVerifications_VerifiedBy",
                table: "DocumentVerifications",
                column: "VerifiedBy");

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
                name: "IX_MedicalExaminations_EligibilityId",
                table: "MedicalExaminations",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_UserId",
                table: "MedicalExaminations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorClaims_MotorPolicyId",
                table: "MotorClaims",
                column: "MotorPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorClaims_UserId",
                table: "MotorClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorPolicies_ProductId",
                table: "MotorPolicies",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorPolicies_UserId",
                table: "MotorPolicies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominees_PolicyId",
                table: "Nominees",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominees_ProposalId",
                table: "Nominees",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MotorPolicyId",
                table: "Payments",
                column: "MotorPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PolicyId",
                table: "Payments",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PropertyPolicyId",
                table: "Payments",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_ProductId",
                table: "Policies",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_UserId",
                table: "Policies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_ProductId",
                table: "PolicyEligibilities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_UserId",
                table: "PolicyEligibilities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyEligibilities_UserId1",
                table: "PolicyEligibilities",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_PremiumConfigurations_ProductId",
                table: "PremiumConfigurations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClaims_PropertyPolicyId",
                table: "PropertyClaims",
                column: "PropertyPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyClaims_UserId",
                table: "PropertyClaims",
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
                name: "IX_Proposals_ProductId",
                table: "Proposals",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_UserId",
                table: "Proposals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnderwritingCases_AssignedUnderwriterId",
                table: "UnderwritingCases",
                column: "AssignedUnderwriterId");

            migrationBuilder.CreateIndex(
                name: "IX_UnderwritingCases_EligibilityId",
                table: "UnderwritingCases",
                column: "EligibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_UnderwritingCases_UserId",
                table: "UnderwritingCases",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentVerifications");

            migrationBuilder.DropTable(
                name: "FraudAlerts");

            migrationBuilder.DropTable(
                name: "MedicalExaminations");

            migrationBuilder.DropTable(
                name: "Nominees");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PremiumConfigurations");

            migrationBuilder.DropTable(
                name: "UnderwritingCases");

            migrationBuilder.DropTable(
                name: "UserRiskProfiles");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "PolicyEligibilities");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "MotorClaims");

            migrationBuilder.DropTable(
                name: "PropertyClaims");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "MotorPolicies");

            migrationBuilder.DropTable(
                name: "PropertyPolicies");

            migrationBuilder.DropTable(
                name: "PolicyProducts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
