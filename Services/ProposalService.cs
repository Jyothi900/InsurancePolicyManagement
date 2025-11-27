using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IProposalRepository _repo;
        private readonly IPolicyProductRepository _productRepo;
        private readonly IMapper _mapper;

        // Valid statuses for proposals only
        private static readonly Status[] ValidProposalStatuses = {
            Status.Applied, Status.UnderReview, Status.Approved, Status.Rejected, Status.Issued
        };

        public ProposalService(IProposalRepository repo, IPolicyProductRepository productRepo, IMapper mapper)
        {
            _repo = repo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<ProposalDto?> SubmitAsync(string userId, ProposalCreateDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId);
            if (product == null) return null;

           
            var calculatedPremium = CalculatePremium(dto, product);

            var proposal = new Proposal
            {
                UserId = userId,
                ProductId = dto.ProductId,
                SumAssured = dto.SumAssured,
                TermYears = dto.TermYears,
                PremiumAmount = calculatedPremium,
                PremiumFrequency = dto.PremiumFrequency,
                Height = dto.Height,
                Weight = dto.Weight,
                IsSmoker = dto.IsSmoker,
                IsDrinker = dto.IsDrinker,
                PreExistingConditions = dto.PreExistingConditions,
                Occupation = dto.Occupation,
                AnnualIncome = dto.AnnualIncome,
                Status = DetermineInitialStatus(dto)
            };

            var created = await _repo.AddAsync(proposal);
            return _mapper.Map<ProposalDto>(created);
        }

        public async Task<IEnumerable<ProposalDto>> GetAllAsync()
        {
            var proposals = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProposalDto>>(proposals);
        }

        public async Task<IEnumerable<ProposalDto>> GetByUserIdAsync(string userId)
        {
            var proposals = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ProposalDto>>(proposals);
        }

        public async Task<ProposalDto?> GetByIdAsync(string id)
        {
            var proposal = await _repo.GetByIdAsync(id);
            return proposal == null ? null : _mapper.Map<ProposalDto>(proposal);
        }

        public async Task<object?> GetStatusAsync(string id)
        {
            var proposal = await _repo.GetByIdAsync(id);
            if (proposal == null) return null;

            return new
            {
                proposalId = proposal.ProposalId,
                status = proposal.Status.ToString(),
                appliedDate = proposal.AppliedDate,
                reviewedDate = proposal.ReviewedDate,
                notes = proposal.UnderwritingNotes,
                premiumAmount = proposal.PremiumAmount
            };
        }

        public async Task<IEnumerable<string>> GetRequiredDocumentsAsync(string id)
        {
            var proposal = await _repo.GetByIdAsync(id);
            if (proposal == null) return new List<string>();

            var requiredDocs = new List<string>
            {
                "Proposal Form",
                "Age Proof",
                "Address Proof"
            };

            // Add conditional documents based on proposal details
            if (proposal.AnnualIncome > 1000000)
                requiredDocs.Add("Income Proof");

            if (proposal.SumAssured > 2500000)
                requiredDocs.Add("Medical Reports");

            if (proposal.IsSmoker || (!string.IsNullOrEmpty(proposal.PreExistingConditions) && proposal.PreExistingConditions != "None"))
                requiredDocs.Add("Health Declaration");

            if (proposal.Occupation?.ToLower().Contains("hazardous") == true)
                requiredDocs.Add("Occupation Certificate");

            return requiredDocs;
        }

        public async Task<bool> UpdateStatusAsync(string proposalId, string status, string? notes = null)
        {
            if (!Enum.TryParse<Status>(status, out var statusEnum) || 
                !ValidProposalStatuses.Contains(statusEnum))
                return false;
                
            return await _repo.UpdateStatusAsync(proposalId, statusEnum, notes);
        }

        private static Status DetermineInitialStatus(ProposalCreateDto dto)
        {
            // Auto-underwriting logic - proposals requiring manual review
            if (dto.IsSmoker || dto.IsDrinker || 
                (!string.IsNullOrEmpty(dto.PreExistingConditions) && dto.PreExistingConditions != "None"))
                return Status.UnderReview;

            if (dto.SumAssured > 5000000 || dto.AnnualIncome < dto.SumAssured / 10)
                return Status.UnderReview;

            // Default status for new proposals
            return Status.Applied;
        }

        private static decimal CalculatePremium(ProposalCreateDto dto, PolicyProduct product)
        {
            decimal basePremium = (dto.SumAssured / 1000) * product.BaseRate;

            // Risk factors
            if (dto.IsSmoker) basePremium *= 1.75m;
            if (dto.IsDrinker) basePremium *= 1.25m;
            if (!string.IsNullOrEmpty(dto.PreExistingConditions) && dto.PreExistingConditions != "None")
                basePremium *= 1.5m;

            // BMI calculation (if height/weight provided)
            if (dto.Height > 0 && dto.Weight > 0)
            {
                var bmi = dto.Weight / ((dto.Height / 100) * (dto.Height / 100));
                if (bmi > 30) basePremium *= 1.2m; // Obesity factor
            }

            // Income vs Sum Assured ratio
            if (dto.AnnualIncome > 0 && dto.SumAssured > dto.AnnualIncome * 10)
                basePremium *= 1.3m; // High coverage factor

            return Math.Round(basePremium, 2);
        }


    }
}
