using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using Document = iText.Layout.Document;

namespace InsurancePolicyManagement.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _repo;
        private readonly IProposalRepository _proposalRepo;
        private readonly IMapper _mapper;

        public PolicyService(IPolicyRepository repo, IProposalRepository proposalRepo, IMapper mapper)
        {
            _repo = repo;
            _proposalRepo = proposalRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PolicyDTO>> GetByUserIdAsync(string userId)
        {
            var policies = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PolicyDTO>>(policies);
        }

        public async Task<PolicyDTO?> GetByIdAsync(string id)
        {
            var policy = await _repo.GetByIdAsync(id);
            return policy == null ? null : _mapper.Map<PolicyDTO>(policy);
        }

        public async Task<IEnumerable<PolicyDTO>> GetAllAsync()
        {
            var policies = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<PolicyDTO>>(policies);
        }



        public async Task<object?> GetPremiumScheduleAsync(string policyId)
        {
            var policy = await _repo.GetByIdAsync(policyId);
            if (policy == null) return null;

            var schedule = new List<object>();
            var currentDate = policy.StartDate;
            
            for (int i = 1; i <= policy.TermYears; i++)
            {
                schedule.Add(new
                {
                    year = i,
                    dueDate = currentDate.AddYears(i),
                    amount = policy.PremiumAmount,
                    status = currentDate.AddYears(i) <= DateTime.Now ? "Due" : "Upcoming"
                });
            }

            return new { policyNumber = policy.PolicyNumber, schedule };
        }

        public async Task<bool> SurrenderAsync(string policyId, string userId)
        {
            var policy = await _repo.GetByIdAsync(policyId);
            if (policy == null || policy.UserId != userId || policy.Status != Status.Active) 
                return false;

            return await _repo.UpdateStatusAsync(policyId, Status.Surrendered);
        }

        public async Task<bool> ReviveAsync(string policyId, string userId)
        {
            var policy = await _repo.GetByIdAsync(policyId);
            if (policy == null || policy.UserId != userId || policy.Status != Status.Lapsed) 
                return false;

          
            var monthsLapsed = (DateTime.Now - policy.NextPremiumDue.Value).Days / 30;
            if (monthsLapsed > 24) return false; 

            return await _repo.UpdateStatusAsync(policyId, Status.Active);
        }

        public async Task<PolicyDTO?> IssueFromProposalAsync(string proposalId)
        {
            try
            {
                var proposal = await _proposalRepo.GetByIdAsync(proposalId);
                if (proposal == null || proposal.Status != Status.Issued) return null;

                var policyNumber = GeneratePolicyNumber(proposal.Product.Category.ToString());
                
                var policy = new Policy
                {
                    PolicyNumber = policyNumber,
                    UserId = proposal.UserId,
                    ProductId = proposal.ProductId,
                    PolicyType = proposal.Product.Category,
                    SumAssured = proposal.SumAssured,
                    PremiumAmount = proposal.PremiumAmount,
                    PremiumFrequency = proposal.PremiumFrequency,
                    TermYears = proposal.TermYears,
                    StartDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(proposal.TermYears),
                    NextPremiumDue = DateTime.Now.AddYears(1),
                    Status = Status.Active
                };

                var created = await _repo.AddAsync(policy);
                
              
                await _proposalRepo.UpdateStatusAsync(proposalId, Status.Success);
                
                return _mapper.Map<PolicyDTO>(created);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]?> GeneratePolicyPDFAsync(string policyId)
        {
            var policy = await _repo.GetByIdAsync(policyId);
            if (policy == null) return null;

            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            document.Add(new Paragraph("INSURANCE POLICY CERTIFICATE")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetFont(boldFont));
            
            document.Add(new Paragraph($"Policy Number: {policy.PolicyNumber}")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(14)
                .SetFont(boldFont));
            
            document.Add(new Paragraph(" "));
            
            document.Add(new Paragraph($"Policyholder: {policy.User?.FullName ?? "N/A"}"));
            document.Add(new Paragraph($"Policy Type: {policy.PolicyType}"));
            document.Add(new Paragraph($"Sum Assured: ₹{policy.SumAssured:N0}"));
            document.Add(new Paragraph($"Premium Amount: ₹{policy.PremiumAmount:N0}"));
            document.Add(new Paragraph($"Term: {policy.TermYears} years"));
            document.Add(new Paragraph($"Start Date: {policy.StartDate:dd/MM/yyyy}"));
            document.Add(new Paragraph($"Expiry Date: {policy.ExpiryDate:dd/MM/yyyy}"));
            document.Add(new Paragraph($"Status: {policy.Status}"));

            document.Close();
            return stream.ToArray();
        }

        private static string GeneratePolicyNumber(string category)
        {
            var prefix = category switch
            {
                "Term Life" => "TERM",
                "Endowment" => "END",
                "ULIP" => "ULIP",
                "Money Back" => "MB",
                _ => "POL"
            };
            
            return $"{prefix}/{DateTime.Now.Year}/{DateTime.Now:MMddHHmmss}";
        }




    }
}
