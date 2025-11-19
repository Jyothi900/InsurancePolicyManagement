using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyManagement.Data
{
    public class InsuranceManagementContext : DbContext
    {
        public InsuranceManagementContext(DbContextOptions<InsuranceManagementContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PolicyProduct> PolicyProducts { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Nominee> Nominees { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Claim> Claims { get; set; }
        
        
        //public DbSet<MotorPolicy> MotorPolicies { get; set; }
        //public DbSet<MotorClaim> MotorClaims { get; set; }

        public DbSet<UnderwritingCase> UnderwritingCases { get; set; }
        
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<DocumentVerification> DocumentVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Policy relationships
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.User)
                .WithMany(u => u.Policies)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Product)
                .WithMany(pr => pr.Policies)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Proposal relationships
            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.User)
                .WithMany(u => u.Proposals)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.Product)
                .WithMany(pr => pr.Proposals)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Claim relationships
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Policy)
                .WithMany(p => p.Claims)
                .HasForeignKey(c => c.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment relationships
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Policy)
                .WithMany(pol => pol.Payments)
                .HasForeignKey(p => p.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Document relationships
            modelBuilder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Policy)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Proposal)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nominee relationships
            modelBuilder.Entity<Nominee>()
                .HasOne(n => n.Policy)
                .WithMany(p => p.Nominees)
                .HasForeignKey(n => n.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nominee>()
                .HasOne(n => n.Proposal)
                .WithMany(p => p.Nominees)
                .HasForeignKey(n => n.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quote relationships
            modelBuilder.Entity<Quote>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Product)
                .WithMany()
                .HasForeignKey(q => q.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Agent)
                .WithMany()
                .HasForeignKey(q => q.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Proposal Agent relationship
            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.Agent)
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Policy Agent relationship
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Agent)
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Claim Agent and Underwriter relationships
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Agent)
                .WithMany()
                .HasForeignKey(c => c.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Underwriter)
                .WithMany()
                .HasForeignKey(c => c.UnderwriterId)
                .OnDelete(DeleteBehavior.Restrict);

            // DocumentVerification relationships
            modelBuilder.Entity<DocumentVerification>()
                .HasOne(dv => dv.Document)
                .WithMany()
                .HasForeignKey(dv => dv.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocumentVerification>()
                .HasOne(dv => dv.Underwriter)
                .WithMany()
                .HasForeignKey(dv => dv.UnderwriterId)
                .OnDelete(DeleteBehavior.Restrict);



            // UnderwritingCase relationships
            modelBuilder.Entity<UnderwritingCase>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UnderwritingCases)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UnderwritingCase>()
                .HasOne(uc => uc.AssignedUnderwriter)
                .WithMany()
                .HasForeignKey(uc => uc.AssignedUnderwriterId)
                .OnDelete(DeleteBehavior.Restrict);



            // Configure decimal precision
            modelBuilder.Entity<Policy>()
                .Property(p => p.SumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(p => p.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PolicyProduct>()
                .Property(p => p.MinSumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PolicyProduct>()
                .Property(p => p.MaxSumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PolicyProduct>()
                .Property(p => p.BaseRate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.SumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.AnnualIncome)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.Height)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Proposal>()
                .Property(p => p.Weight)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Claim>()
                .Property(c => c.ClaimAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Claim>()
                .Property(c => c.ApprovedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Nominee>()
                .Property(n => n.SharePercentage)
                .HasPrecision(5, 2);

           


            // Quote decimal precision
            modelBuilder.Entity<Quote>()
                .Property(q => q.SumAssured)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Quote>()
                .Property(q => q.PremiumAmount)
                .HasPrecision(18, 2);

            // Seed data
            modelBuilder.Entity<PolicyProduct>().HasData(
                new PolicyProduct { ProductId = "PRD001", ProductName = "Tech Term", Category = PolicyType.TermLife, CompanyName = "LIC", MinAge = 18, MaxAge = 65, MinSumAssured = 500000, MaxSumAssured = 10000000, MinTerm = 10, MaxTerm = 30, BaseRate = 4.5m, HasMaturityBenefit = false, HasDeathBenefit = true, RiskLevel = RiskLevel.Low, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Life },
                new PolicyProduct { ProductId = "PRD002", ProductName = "Jeevan Anand", Category = PolicyType.Endowment, CompanyName = "LIC", MinAge = 18, MaxAge = 50, MinSumAssured = 100000, MaxSumAssured = 5000000, MinTerm = 15, MaxTerm = 35, BaseRate = 45.2m, HasMaturityBenefit = true, HasDeathBenefit = true, RiskLevel = RiskLevel.Low, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Life },
                new PolicyProduct { ProductId = "PRD003", ProductName = "New Money Back 20", Category = PolicyType.MoneyBack, CompanyName = "LIC", MinAge = 13, MaxAge = 45, MinSumAssured = 100000, MaxSumAssured = 3000000, MinTerm = 20, MaxTerm = 20, BaseRate = 52.8m, HasMaturityBenefit = true, HasDeathBenefit = true, RiskLevel = RiskLevel.Low, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Life },
                new PolicyProduct { ProductId = "PRD004", ProductName = "Click2Wealth", Category = PolicyType.ULIP, CompanyName = "HDFC Life", MinAge = 18, MaxAge = 60, MinSumAssured = 500000, MaxSumAssured = 25000000, MinTerm = 10, MaxTerm = 30, BaseRate = 15.5m, HasMaturityBenefit = true, HasDeathBenefit = true, RiskLevel = RiskLevel.High, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Life },
                new PolicyProduct { ProductId = "PRD005", ProductName = "Jeevan Shanti", Category = PolicyType.Pension, CompanyName = "LIC", MinAge = 30, MaxAge = 79, MinSumAssured = 150000, MaxSumAssured = 10000000, MinTerm = 10, MaxTerm = 40, BaseRate = 85.5m, HasMaturityBenefit = true, HasDeathBenefit = true, RiskLevel = RiskLevel.Low, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Life },
                new PolicyProduct { ProductId = "PRD008", ProductName = "Comprehensive Motor Insurance", Category = PolicyType.PersonalAccident, CompanyName = "HDFC ERGO", MinAge = 18, MaxAge = 75, MinSumAssured = 50000, MaxSumAssured = 10000000, MinTerm = 1, MaxTerm = 1, BaseRate = 3.5m, HasMaturityBenefit = false, HasDeathBenefit = false, RiskLevel = RiskLevel.Medium, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Motor },
                new PolicyProduct { ProductId = "PRD009", ProductName = "Third Party Motor Insurance", Category = PolicyType.PersonalAccident, CompanyName = "ICICI Lombard", MinAge = 18, MaxAge = 75, MinSumAssured = 0, MaxSumAssured = 0, MinTerm = 1, MaxTerm = 1, BaseRate = 1.5m, HasMaturityBenefit = false, HasDeathBenefit = false, RiskLevel = RiskLevel.Low, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), InsuranceType = InsuranceType.Motor }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = "USE001", FullName = "Rajesh Kumar Reddy", Email = "rajeshreddy@gmail.com", Password = "Password123!", Role = UserRole.Customer, Mobile = "9876543210", DateOfBirth = new DateOnly(1985, 3, 15), Gender = Gender.Male, AadhaarNumber = "123456789012", PANNumber = "ABCDE1234F", Address = "12-34, MG Road, Hyderabad, Telangana - 500001", KYCStatus = KYCStatus.Verified, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new User { UserId = "USE002", FullName = "Priya Lakshmi Nair", Email = "priyanair@gmail.com", Password = "Password123!", Role = UserRole.Customer, Mobile = "9876543211", DateOfBirth = new DateOnly(1990, 7, 22), Gender = Gender.Female, AadhaarNumber = "123456789013", PANNumber = "BCDEF2345G", Address = "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", KYCStatus = KYCStatus.Verified, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new User { UserId = "USE003", FullName = "Venkatesh Iyer", Email = "venkateshiyer@gmail.com", Password = "Password123!", Role = UserRole.Agent, Mobile = "9876543212", DateOfBirth = new DateOnly(1982, 11, 8), Gender = Gender.Male, AadhaarNumber = "123456789014", PANNumber = "CDEFG3456H", Address = "78-90, Brigade Road, Bangalore, Karnataka - 560001", KYCStatus = KYCStatus.Verified, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new User { UserId = "USE004", FullName = "Meera Krishnan", Email = "meerakrishnan@gmail.com", Password = "Password123!", Role = UserRole.Underwriter, Mobile = "9876543213", DateOfBirth = new DateOnly(1988, 5, 12), Gender = Gender.Female, AadhaarNumber = "123456789015", PANNumber = "DEFGH4567I", Address = "23-45, Marine Drive, Kochi, Kerala - 682001", KYCStatus = KYCStatus.Pending, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
                new User { UserId = "USE005", FullName = "Jyothi Reddy", Email = "jytohireddy@gmail.com", Password = "Admin123!", Role = UserRole.Admin, Mobile = "9876543214", DateOfBirth = new DateOnly(1992, 9, 25), Gender = Gender.Male, AadhaarNumber = "123456789016", PANNumber = "EFGHI5678J", Address = "56-78, IT Corridor, Tirupathi, Andhra Pradesh - 500081", KYCStatus = KYCStatus.Verified, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
            );

            modelBuilder.Entity<Policy>().HasData(
                new Policy { PolicyId = "POL001", PolicyNumber = "LIC/TERM/2024/001", UserId = "USE001", ProductId = "PRD001", PolicyType = PolicyType.TermLife, SumAssured = 5000000, PremiumAmount = 22500, PremiumFrequency = PremiumFrequency.Annual, TermYears = 20, StartDate = new DateTime(2024, 1, 15), ExpiryDate = new DateTime(2044, 1, 15), NextPremiumDue = new DateTime(2025, 1, 15), Status = Status.Active, IssuedDate = new DateTime(2024, 1, 15) },
                new Policy { PolicyId = "POL002", PolicyNumber = "LIC/END/2024/002", UserId = "USE002", ProductId = "PRD002", PolicyType = PolicyType.Endowment, SumAssured = 1000000, PremiumAmount = 45200, PremiumFrequency = PremiumFrequency.Annual, TermYears = 20, StartDate = new DateTime(2024, 2, 10), ExpiryDate = new DateTime(2044, 2, 10), NextPremiumDue = new DateTime(2025, 2, 10), Status = Status.Active, IssuedDate = new DateTime(2024, 2, 10) },
                new Policy { PolicyId = "POL003", PolicyNumber = "HDFC/ULIP/2024/003", UserId = "USE003", ProductId = "PRD004", PolicyType = PolicyType.ULIP, SumAssured = 2500000, PremiumAmount = 38750, PremiumFrequency = PremiumFrequency.Annual, TermYears = 15, StartDate = new DateTime(2024, 3, 5), ExpiryDate = new DateTime(2039, 3, 5), NextPremiumDue = new DateTime(2025, 3, 5), Status = Status.Active, IssuedDate = new DateTime(2024, 3, 5) }
            );

            modelBuilder.Entity<Nominee>().HasData(
                new Nominee { NomineeId = "NOM001", PolicyId = "POL001", FullName = "Lakshmi Rajesh Reddy", Relationship = UnifiedRelationship.Spouse, DateOfBirth = new DateOnly(1987, 6, 20), SharePercentage = 100, AadhaarNumber = "123456789017", Address = "12-34, MG Road, Hyderabad, Telangana - 500001", CreatedAt = new DateTime(2024, 1, 1) },
                new Nominee { NomineeId = "NOM002", PolicyId = "POL002", FullName = "Suresh Kumar Nair", Relationship = UnifiedRelationship.Spouse, DateOfBirth = new DateOnly(1988, 4, 15), SharePercentage = 60, AadhaarNumber = "123456789018", Address = "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", CreatedAt = new DateTime(2024, 1, 1) },
                new Nominee { NomineeId = "NOM003", PolicyId = "POL002", FullName = "Aadhya Priya Nair", Relationship = UnifiedRelationship.Child, DateOfBirth = new DateOnly(2015, 8, 10), SharePercentage = 40, AadhaarNumber = "123456789019", Address = "45-67, Anna Salai, Chennai, Tamil Nadu - 600002", CreatedAt = new DateTime(2024, 1, 1) },
                new Nominee { NomineeId = "NOM004", PolicyId = "POL003", FullName = "Kavitha Venkatesh Iyer", Relationship = UnifiedRelationship.Spouse, DateOfBirth = new DateOnly(1984, 12, 3), SharePercentage = 100, AadhaarNumber = "123456789020", Address = "78-90, Brigade Road, Bangalore, Karnataka - 560001", CreatedAt = new DateTime(2024, 1, 1) }
            );

            modelBuilder.Entity<Proposal>().HasData(
                new Proposal { ProposalId = "POS001", UserId = "USE004", ProductId = "PRD003", SumAssured = 500000, TermYears = 20, PremiumAmount = 26400, PremiumFrequency = PremiumFrequency.Annual, Height = 165, Weight = 60, IsSmoker = false, IsDrinker = false, Occupation = "Software Engineer", AnnualIncome = 800000, Status = Status.Applied, AppliedDate = new DateTime(2024, 4, 1) },
                new Proposal { ProposalId = "POS002", UserId = "USE005", ProductId = "PRD005", SumAssured = 1000000, TermYears = 25, PremiumAmount = 85500, PremiumFrequency = PremiumFrequency.Annual, Height = 175, Weight = 75, IsSmoker = false, IsDrinker = true, Occupation = "Business Analyst", AnnualIncome = 1200000, Status = Status.UnderReview, AppliedDate = new DateTime(2024, 4, 10) }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment { PaymentId = "PAY001", PolicyId = "POL001", UserId = "USE001", Amount = 22500, PaymentType = "Premium", PaymentMethod = "UPI", TransactionId = "TXN001234567890", PaymentGateway = "Razorpay", Status = "Success", PaymentDate = new DateTime(2024, 1, 15), ProcessedDate = new DateTime(2024, 1, 15), DueDate = new DateTime(2024, 1, 15), NextDueDate = new DateTime(2025, 1, 15) },
                new Payment { PaymentId = "PAY002", PolicyId = "POL002", UserId = "USE002", Amount = 45200, PaymentType = "Premium", PaymentMethod = "NetBanking", TransactionId = "TXN001234567891", PaymentGateway = "Paytm", Status = "Success", PaymentDate = new DateTime(2024, 2, 10), ProcessedDate = new DateTime(2024, 2, 10), DueDate = new DateTime(2024, 2, 10), NextDueDate = new DateTime(2025, 2, 10) },
                new Payment { PaymentId = "PAY003", PolicyId = "POL003", UserId = "USE003", Amount = 38750, PaymentType = "Premium", PaymentMethod = "Card", TransactionId = "TXN001234567892", PaymentGateway = "Razorpay", Status = "Success", PaymentDate = new DateTime(2024, 3, 5), ProcessedDate = new DateTime(2024, 3, 5), DueDate = new DateTime(2024, 3, 5), NextDueDate = new DateTime(2025, 3, 5) }
            );

            modelBuilder.Entity<Claim>().HasData(
                new Claim { ClaimId = "CLM001", ClaimNumber = "CLM/2024/001", PolicyId = "POL001", UserId = "USE001", ClaimType = "Death", ClaimAmount = 5000000, Status = "Pending", IncidentDate = new DateTime(2024, 6, 15), FiledDate = new DateTime(2024, 6, 20), CauseOfDeath = "Natural", ClaimantName = "Lakshmi Rajesh Reddy", ClaimantRelation = "Spouse", ClaimantBankDetails = "SBI Account: 12345678901", RequiresInvestigation = false }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document { DocumentId = "DOC001", UserId = "USE001", PolicyId = "POL001", DocumentType = DocumentType.PolicyDocument, FileName = "policy_POL001.pdf", FilePath = "/uploads/documents/policy_POL001.pdf", Status = Status.Verified, UploadedAt = new DateTime(2024, 1, 15), VerifiedAt = new DateTime(2024, 1, 16) },
                new Document { DocumentId = "DOC003", UserId = "USE004", ProposalId = "POS001", DocumentType = DocumentType.Medical, FileName = "medical_report_POS001.pdf", FilePath = "/uploads/documents/medical_report_POS001.pdf", Status = Status.Verified, UploadedAt = new DateTime(2024, 4, 2), VerifiedAt = new DateTime(2024, 4, 3) }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
