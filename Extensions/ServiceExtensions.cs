using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Repository;
using InsurancePolicyManagement.Services;

namespace InsurancePolicyManagement.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPolicyProductRepository, PolicyProductRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<INomineeRepository, NomineeRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IUnderwriterRepository, UnderwriterRepository>();
            
            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPolicyProductService, PolicyProductService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IPolicyService, PolicyService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<INomineeService, NomineeService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IUnderwriterService, UnderwriterService>();
            services.AddScoped<ICustomerJourneyService, CustomerJourneyService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IEnumService, EnumService>();
          
            services.AddScoped<IToken, TokenService>();
            services.AddScoped<IUser, UserAuthService>();
            
            return services;
        }
    }
}