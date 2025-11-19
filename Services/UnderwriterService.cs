using AutoMapper;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Services
{
    public class UnderwriterService : IUnderwriterService
    {
        private readonly IUnderwriterRepository _underwriterRepository;
        private readonly IMapper _mapper;
        
        public UnderwriterService(IUnderwriterRepository underwriterRepository, IMapper mapper)
        {
            _underwriterRepository = underwriterRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UnderwritingCaseDto>> GetPendingCasesAsync(string underwriterId)
        {
            var cases = await _underwriterRepository.GetPendingCasesAsync(underwriterId);
            return _mapper.Map<IEnumerable<UnderwritingCaseDto>>(cases);
        }
        
        public async Task<UnderwritingCaseDto> ProcessCaseAsync(string caseId, string decision, string reason, decimal? approvedSumAssured = null)
        {
            var underwritingCase = new UnderwritingCase
            {
                CaseId = caseId,
                Decision = decision,
                DecisionReason = reason,
                ApprovedSumAssured = approvedSumAssured,
                Status = "Completed",
                CompletedAt = DateTime.Now
            };
            
            var result = await _underwriterRepository.ProcessCaseAsync(underwritingCase);
            return _mapper.Map<UnderwritingCaseDto>(result);
        }
        
      
        

        
        public async Task<IEnumerable<DocumentDto>> GetPendingDocumentsAsync(string underwriterId)
        {
            var documents = await _underwriterRepository.GetPendingDocumentsAsync(underwriterId);
          
            return documents.Select(dv => _mapper.Map<DocumentDto>(dv.Document)).ToList();
        }
        
        public async Task<DocumentDto> VerifyDocumentAsync(string verificationId, bool isVerified, string notes)
        {
            var verification = new DocumentVerification
            {
                VerificationId = verificationId,
                IsVerified = isVerified,
                VerificationNotes = notes,
                VerificationStatus = isVerified ? "Verified" : "Rejected",
                VerifiedAt = DateTime.Now
            };
            
            var result = await _underwriterRepository.VerifyDocumentAsync(verification);
            return _mapper.Map<DocumentDto>(result.Document);
        }

        public async Task<IEnumerable<DocumentDto>> GetPendingDocumentsByUnderwriterAsync(string underwriterId)
        {
           
            var documents = await _underwriterRepository.GetPendingDocumentsByUnderwriterAsync(underwriterId);
            
            return documents.Select(dv => _mapper.Map<DocumentDto>(dv.Document)).ToList();
        }

        public async Task<IEnumerable<DocumentDto>> GetAllPendingDocumentsAsync()
        {
            var documents = await _underwriterRepository.GetAllPendingDocumentsAsync();
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<IEnumerable<ProposalDto>> GetPendingProposalsAsync()
        {
            var proposals = await _underwriterRepository.GetPendingProposalsAsync();
            return _mapper.Map<IEnumerable<ProposalDto>>(proposals);
        }

        public async Task<bool> UpdateKYCStatusAsync(string userId, string status, string? notes = null)
        {
            return await _underwriterRepository.UpdateKYCStatusAsync(userId, status, notes);
        }

        public async Task<bool> UpdateProposalStatusAsync(string proposalId, string status, string? notes = null)
        {
            return await _underwriterRepository.UpdateProposalStatusAsync(proposalId, status, notes);
        }

        public async Task<IEnumerable<AssignedCustomerDto>> GetAssignedCustomersAsync(string underwriterId)
        {
            var customers = await _underwriterRepository.GetAssignedCustomersAsync(underwriterId);
            return _mapper.Map<IEnumerable<AssignedCustomerDto>>(customers);
        }
    }
}
