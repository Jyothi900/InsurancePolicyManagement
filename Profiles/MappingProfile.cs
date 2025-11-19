using AutoMapper;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegistrationDto, User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));
            
            // Policy mappings
            CreateMap<Policy, PolicyDTO>().ReverseMap();

            
            // PolicyProduct mappings
            CreateMap<PolicyProduct, PolicyProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.PremiumRate, opt => opt.MapFrom(src => src.BaseRate))
                .ForMember(dest => dest.PolicyTerm, opt => opt.MapFrom(src => src.MaxTerm))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ""));
            
            CreateMap<PolicyProductCreateDto, PolicyProduct>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.InsuranceType, opt => opt.MapFrom(src => src.InsuranceType))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskLevel))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());
            
            CreateMap<PolicyProductUpdateDto, PolicyProduct>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
           
            CreateMap<Quote, QuoteDto>().ReverseMap();
            CreateMap<QuoteCreateDto, Quote>();

            CreateMap<Proposal, ProposalDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();
            CreateMap<ProposalCreateDto, Proposal>();
            
           
            CreateMap<Claim, ClaimDto>()
                .ForMember(dest => dest.ClaimType, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ClaimType) ? AllClaimTypes.Death : Enum.Parse<AllClaimTypes>(src.ClaimType, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Status) ? Status.Pending : Enum.Parse<Status>(src.Status, true)))
                .ForMember(dest => dest.CauseOfDeath, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CauseOfDeath) ? CauseOfDeath.Natural : Enum.Parse<CauseOfDeath>(src.CauseOfDeath, true)))
                .ForMember(dest => dest.ClaimantRelation, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ClaimantRelation) ? UnifiedRelationship.Self : Enum.Parse<UnifiedRelationship>(src.ClaimantRelation, true)));
            CreateMap<Claim, ClaimStatusDto>().ReverseMap();
           
    
            CreateMap<Payment, PaymentStatusDto>().ReverseMap();
            
      
            CreateMap<Nominee, NomineeDto>().ReverseMap();
        
            CreateMap<Document, DocumentDto>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
           
            CreateMap<UnderwritingCase, UnderwritingCaseDto>().ReverseMap();

            
            CreateMap<DocumentVerification, DocumentVerificationDto>().ReverseMap();

            CreateMap<User, AssignedCustomerDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.KycStatus, opt => opt.MapFrom(src => src.KYCStatus.ToString()))
                .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => src.UnderwriterAssignedDate ?? DateTime.Now))
                .ForMember(dest => dest.PendingDocuments, opt => opt.MapFrom(src => src.Documents != null ? src.Documents.Count(d => d.Status == Status.Uploaded || d.Status == Status.Pending) : 0));
        }
    }
}
