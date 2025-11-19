using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsurancePolicyManagement.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_MotorClaims_MotorClaimId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_MotorPolicies_MotorPolicyId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_MotorPolicies_MotorPolicyId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "MotorClaims");

            migrationBuilder.DropTable(
                name: "PremiumConfigurations");

            migrationBuilder.DropTable(
                name: "MotorPolicies");

            migrationBuilder.DropIndex(
                name: "IX_Payments_MotorPolicyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Documents_MotorClaimId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_MotorPolicyId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MotorPolicyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MotorClaimId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MotorPolicyId",
                table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotorPolicyId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotorClaimId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotorPolicyId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MotorPolicies",
                columns: table => new
                {
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    HasEngineProtection = table.Column<bool>(type: "bit", nullable: false),
                    HasRoadSideAssistance = table.Column<bool>(type: "bit", nullable: false),
                    HasZeroDepreciation = table.Column<bool>(type: "bit", nullable: false),
                    IDV = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturingYear = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCBPercentage = table.Column<int>(type: "int", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoluntaryDeductible = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
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
                name: "PremiumConfigurations",
                columns: table => new
                {
                    ConfigId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConfigKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                name: "MotorClaims",
                columns: table => new
                {
                    MotorClaimId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MotorPolicyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccidentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccidentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClaimType = table.Column<int>(type: "int", nullable: false),
                    EstimatedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FIRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FIRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GarageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GarageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCashlessRepair = table.Column<bool>(type: "bit", nullable: false),
                    PoliceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SurveyorAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ThirdPartyClaimAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ThirdPartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdPartyVehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC001",
                columns: new[] { "MotorClaimId", "MotorPolicyId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "DocumentId",
                keyValue: "DOC003",
                columns: new[] { "MotorClaimId", "MotorPolicyId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY001",
                column: "MotorPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY002",
                column: "MotorPolicyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "PAY003",
                column: "MotorPolicyId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MotorPolicyId",
                table: "Payments",
                column: "MotorPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MotorClaimId",
                table: "Documents",
                column: "MotorClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MotorPolicyId",
                table: "Documents",
                column: "MotorPolicyId");

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
                name: "IX_PremiumConfigurations_ProductId",
                table: "PremiumConfigurations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_MotorClaims_MotorClaimId",
                table: "Documents",
                column: "MotorClaimId",
                principalTable: "MotorClaims",
                principalColumn: "MotorClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_MotorPolicies_MotorPolicyId",
                table: "Documents",
                column: "MotorPolicyId",
                principalTable: "MotorPolicies",
                principalColumn: "MotorPolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_MotorPolicies_MotorPolicyId",
                table: "Payments",
                column: "MotorPolicyId",
                principalTable: "MotorPolicies",
                principalColumn: "MotorPolicyId");
        }
    }
}
