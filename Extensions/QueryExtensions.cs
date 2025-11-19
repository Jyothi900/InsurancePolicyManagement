using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Extensions
{
    public static class QueryExtensions
    {
       
        public static IQueryable<Policy> WithStandardIncludes(this IQueryable<Policy> query)
        {
            return query
                .Include(p => p.User)
                .Include(p => p.Product);
        }

        public static IQueryable<Policy> WithAllIncludes(this IQueryable<Policy> query)
        {
            return query
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Nominees)
                .Include(p => p.Payments)
                .Include(p => p.Claims)
                .Include(p => p.Documents);
        }

       
        public static IQueryable<Claim> WithStandardIncludes(this IQueryable<Claim> query)
        {
            return query
                .Include(c => c.Policy)
                .Include(c => c.User)
                .Include(c => c.Documents);
        }

        public static IQueryable<Payment> WithStandardIncludes(this IQueryable<Payment> query)
        {
            return query
                .Include(p => p.Policy)
                .Include(p => p.User);
        }
    }
}