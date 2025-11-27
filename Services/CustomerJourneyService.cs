using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Services
{
    public class CustomerJourneyService : ICustomerJourneyService
    {
        private readonly IPolicyProductService _productService;
        private readonly IUserService _userService;
        private readonly IProposalService _proposalService;
        private readonly IDocumentService _documentService;
        private readonly ILogger<CustomerJourneyService> _logger;

        public CustomerJourneyService(
            IPolicyProductService productService,
            IUserService userService,
            IProposalService proposalService,
            IDocumentService documentService,
            ILogger<CustomerJourneyService> logger)
        {
            _productService = productService;
            _userService = userService;
            _proposalService = proposalService;
            _documentService = documentService;
            _logger = logger;
        }

        public async Task<object> GetCustomerJourneyDataAsync(string? userId, string? productId, QuoteRequestDto? quoteRequest)
        {
            _logger.LogInformation("Fetching customer journey data for user: {UserId}", userId ?? "Anonymous");

            var products = await _productService.GetAllAsync();
            var insuranceTypes = await _productService.GetInsuranceTypesAsync();
            
            var enums = new
            {
                userRoles = Enum.GetValues<UserRole>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                genders = Enum.GetValues<Gender>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                kycStatuses = Enum.GetValues<KYCStatus>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                statuses = Enum.GetValues<Status>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                policyTypes = Enum.GetValues<PolicyType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                insuranceTypes = Enum.GetValues<InsuranceType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                documentTypes = Enum.GetValues<DocumentType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                paymentMethods = Enum.GetValues<PaymentMethod>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                premiumFrequencies = Enum.GetValues<PremiumFrequency>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList()
            };

            object userData = null;
            object userProposals = null;
            object userDocuments = null;
            object selectedProduct = null;
            object premiumQuote = null;

            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    userData = await _userService.GetByIdAsync(userId);
                    userProposals = await _proposalService.GetByUserIdAsync(userId);
                    userDocuments = await _documentService.GetByUserIdAsync(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to fetch user-specific data for {UserId}: {Error}", userId, ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(productId))
            {
                try
                {
                    selectedProduct = await _productService.GetByIdAsync(productId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to fetch product {ProductId}: {Error}", productId, ex.Message);
                }
            }

            if (quoteRequest != null)
            {
                try
                {
                    premiumQuote = await _productService.CalculatePremiumAsync(quoteRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to calculate premium: {Error}", ex.Message);
                }
            }

            object requiredDocuments = null;
            if (!string.IsNullOrEmpty(userId) && userProposals != null)
            {
                try
                {
                    var proposals = userProposals as IEnumerable<dynamic>;
                    if (proposals?.Any() == true)
                    {
                        var latestProposal = proposals.OrderByDescending(p => p.appliedDate).FirstOrDefault();
                        if (latestProposal?.proposalId != null)
                        {
                            requiredDocuments = await _proposalService.GetRequiredDocumentsAsync(latestProposal.proposalId.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to fetch required documents: {Error}", ex.Message);
                }
            }

            return new
            {
                products,
                insuranceTypes,
                enums,
                userData,
                userProposals,
                userDocuments,
                selectedProduct,
                premiumQuote,
                requiredDocuments,
                timestamp = DateTime.UtcNow
            };
        }

        public async Task<object> CalculatePremiumWithContextAsync(string? userId, QuoteRequestDto quoteRequest)
        {
            return await GetCustomerJourneyDataAsync(userId, quoteRequest?.ProductId, quoteRequest);
        }
    }
}