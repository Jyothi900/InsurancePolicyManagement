using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Services
{
    public class NomineeService : INomineeService
    {
        private readonly INomineeRepository _repo;
        private readonly IMapper _mapper;

        public NomineeService(INomineeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NomineeDto>> GetByPolicyIdAsync(string policyId)
        {
            var nominees = await _repo.GetByPolicyIdAsync(policyId);
            return _mapper.Map<IEnumerable<NomineeDto>>(nominees);
        }

        public async Task<IEnumerable<NomineeDto>> GetByProposalIdAsync(string proposalId)
        {
            var nominees = await _repo.GetByProposalIdAsync(proposalId);
            return _mapper.Map<IEnumerable<NomineeDto>>(nominees);
        }

        public async Task<NomineeDto?> AddAsync(NomineeDto dto, string? policyId = null, string? proposalId = null)
        {
          
            var isValidShare = await _repo.ValidateSharePercentageAsync(policyId, proposalId);
            if (!isValidShare) return null;

            var nominee = new Nominee
            {
                PolicyId = policyId,
                ProposalId = proposalId,
                FullName = dto.FullName,
                Relationship = dto.Relationship,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                SharePercentage = dto.SharePercentage,
                AadhaarNumber = dto.AadhaarNumber,
                Address = dto.Address
            };

            var created = await _repo.AddAsync(nominee);
            return _mapper.Map<NomineeDto>(created);
        }

        public async Task<NomineeDto?> UpdateAsync(string id, NomineeDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            
            var isValidShare = await _repo.ValidateSharePercentageAsync(existing.PolicyId, existing.ProposalId, id);
            if (!isValidShare) return null;

        
            existing.FullName = dto.FullName;
            existing.Relationship = dto.Relationship;
            existing.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth);
            existing.SharePercentage = dto.SharePercentage;
            existing.AadhaarNumber = dto.AadhaarNumber;
            existing.Address = dto.Address;

            await _repo.UpdateAsync(existing);
            return _mapper.Map<NomineeDto>(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<bool> UpdateAllForPolicyAsync(string policyId, List<NomineeDto> nominees)
        {
            
            var totalShare = nominees.Sum(n => n.SharePercentage);
            if (totalShare != 100) return false;

            
            await _repo.DeleteByPolicyIdAsync(policyId);

            foreach (var dto in nominees)
            {
                var nominee = new Nominee
                {
                    PolicyId = policyId,
                    FullName = dto.FullName,
                    Relationship = dto.Relationship,
                    DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                    SharePercentage = dto.SharePercentage,
                    AadhaarNumber = dto.AadhaarNumber,
                    Address = dto.Address
                };

                await _repo.AddAsync(nominee);
            }

            return true;
        }

        public async Task<NomineeDto?> GetByIdAsync(string id)
        {
            var nominee = await _repo.GetByIdAsync(id);
            return nominee == null ? null : _mapper.Map<NomineeDto>(nominee);
        }


    }
}
