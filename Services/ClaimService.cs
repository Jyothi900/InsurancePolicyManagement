using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _repo;
        private readonly IPolicyRepository _policyRepo;
        private readonly IMapper _mapper;

        public ClaimService(IClaimRepository repo, IPolicyRepository policyRepo, IMapper mapper)
        {
            _repo = repo;
            _policyRepo = policyRepo;
            _mapper = mapper;
        }

        public async Task<ClaimDto?> FileClaimAsync(string userId, ClaimCreateDto dto)
        {
            var policy = await _policyRepo.GetByIdAsync(dto.PolicyId);
            if (policy == null || policy.UserId != userId || policy.Status != Status.Active) 
                return null;

            var claimNumber = GenerateClaimNumber();

            var claim = new Claim
            {
                ClaimNumber = claimNumber,
                PolicyId = dto.PolicyId,
                UserId = userId,
                AgentId = policy.AgentId,
                ClaimType = dto.ClaimType.ToString(),
                ClaimAmount = policy.SumAssured,
                Status = Status.Pending.ToString(),  
                IncidentDate = dto.IncidentDate,
                CauseOfDeath = dto.CauseOfDeath.ToString(),
                ClaimantName = dto.ClaimantName,
                ClaimantRelation = dto.ClaimantRelation.ToString(),
                ClaimantBankDetails = dto.ClaimantBankDetails,
                RequiresInvestigation = DetermineInvestigationRequired(dto)
            };



            var created = await _repo.AddAsync(claim);
            
            // Return ClaimDto with numeric enums for frontend
            return new ClaimDto
            {
                ClaimId = created.ClaimId,
                ClaimNumber = created.ClaimNumber,
                PolicyId = created.PolicyId,
                UserId = created.UserId,
                AgentId = created.AgentId,
                ClaimType = dto.ClaimType,  // Return original enum value
                ClaimAmount = created.ClaimAmount,
                Status = Status.Pending,
                IncidentDate = created.IncidentDate,
                FiledDate = created.FiledDate,
                CauseOfDeath = dto.CauseOfDeath,
                ClaimantName = created.ClaimantName,
                ClaimantRelation = dto.ClaimantRelation,
                ClaimantBankDetails = created.ClaimantBankDetails
            };
        }

        public async Task<IEnumerable<ClaimDto>> GetAllAsync()
        {
            var claims = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ClaimDto>>(claims);
        }

        public async Task<IEnumerable<ClaimDto>> GetByUserIdAsync(string userId)
        {
            var claims = await _repo.GetByUserIdAsync(userId);
            return claims.Select(c => new ClaimDto
            {
                ClaimId = c.ClaimId,
                ClaimNumber = c.ClaimNumber,
                PolicyId = c.PolicyId,
                UserId = c.UserId,
                AgentId = c.AgentId,
                ClaimType = Enum.Parse<AllClaimTypes>(c.ClaimType),
                ClaimAmount = c.ClaimAmount,
                Status = string.IsNullOrEmpty(c.Status) ? Status.Pending : Enum.Parse<Status>(c.Status),
                IncidentDate = c.IncidentDate,
                FiledDate = c.FiledDate,
                ProcessedDate = c.ProcessedDate,
                ApprovedAmount = c.ApprovedAmount,
                CauseOfDeath = string.IsNullOrEmpty(c.CauseOfDeath) ? CauseOfDeath.Natural : Enum.Parse<CauseOfDeath>(c.CauseOfDeath),
                ClaimantName = c.ClaimantName ?? string.Empty,
                ClaimantRelation = string.IsNullOrEmpty(c.ClaimantRelation) ? UnifiedRelationship.Self : Enum.Parse<UnifiedRelationship>(c.ClaimantRelation),
                ClaimantBankDetails = c.ClaimantBankDetails ?? string.Empty
            });
        }

        public async Task<ClaimStatusDto?> GetStatusAsync(string claimNumber)
        {
            var claim = await _repo.GetByClaimNumberAsync(claimNumber);
            return claim == null ? null : _mapper.Map<ClaimStatusDto>(claim);
        }

        public async Task<IEnumerable<string>> GetRequiredDocumentsAsync(string claimNumber)
        {
            var claim = await _repo.GetByClaimNumberAsync(claimNumber);
            if (claim == null) return new List<string>();

            var requiredDocs = new List<string> 
            { 
                "Death Certificate", 
                "Policy Document", 
                "Claimant ID Proof", 
                "Bank Details" 
            };

           
            if (claim.CauseOfDeath == CauseOfDeath.Accidental.ToString() || claim.CauseOfDeath == CauseOfDeath.Murder.ToString())
                requiredDocs.Add("FIR Copy");

            if (claim.CauseOfDeath == CauseOfDeath.Suicide.ToString() || claim.RequiresInvestigation)
                requiredDocs.Add("Post Mortem Report");

            if (claim.IncidentDate > DateTime.Now.AddDays(-30))
                requiredDocs.Add("Medical Records");

            return requiredDocs;
        }

        public async Task<IEnumerable<object>> GetTimelineAsync(string claimNumber)
        {
            var claim = await _repo.GetByClaimNumberAsync(claimNumber);
            if (claim == null) return new List<object>();

            var timeline = new List<object>
            {
                new { date = claim.FiledDate, status = "Pending", notes = "Claim submitted by claimant" }
            };

            if (claim.Status != "Pending")
            {
                timeline.Add(new { date = claim.FiledDate.AddDays(1), status = "Under Review", notes = "Documents verification in progress" });
            }

            if (claim.RequiresInvestigation)
            {
                timeline.Add(new { date = claim.FiledDate.AddDays(3), status = "Investigation", notes = "Investigation assigned" });
            }

            if (claim.ProcessedDate.HasValue)
            {
                timeline.Add(new { date = claim.ProcessedDate, status = claim.Status, notes = claim.InvestigationNotes ?? "Claim processed" });
            }

            return timeline.OrderBy(t => ((dynamic)t).date);
        }

        public async Task<bool> UpdateStatusAsync(string claimId, string status, string? notes = null)
        {
            return await _repo.UpdateStatusAsync(claimId, status, notes);
        }

        public async Task<bool> ApproveClaimAsync(string claimId, decimal approvedAmount)
        {
            return await _repo.ApproveClaimAsync(claimId, approvedAmount);
        }

        private static string GenerateClaimNumber()
        {
            return $"CLM{DateTime.Now:yyyy}{DateTime.Now:MMddHHmmss}";
        }

        private static bool DetermineInvestigationRequired(ClaimCreateDto dto)
        {
            // Investigation required for suspicious cases
            if (dto.CauseOfDeath == CauseOfDeath.Suicide || dto.CauseOfDeath == CauseOfDeath.Murder) return true;
            if (dto.IncidentDate > DateTime.Now.AddDays(-90)) return true; // Recent policy
            if (dto.CauseOfDeath == CauseOfDeath.Accidental) return true;
            
            return false;
        }


    }
}
