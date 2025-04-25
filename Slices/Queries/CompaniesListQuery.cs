using MediatR;
using Microsoft.EntityFrameworkCore;
using Sample9.DataAccess;
using Sample9.DataModels;

namespace Sample9.Slices.Queries
{
    public class CompaniesListQuery
    {
        public record Query : IRequest<List<CompanyDto>>;
        public class QueryHandler(ApplicationDbContext context) : IRequestHandler<Query, List<CompanyDto>>
        {
            public async Task<List<CompanyDto>> Handle(Query query, CancellationToken token)
            {
                return await context.Companies
                    .Select(r => new CompanyDto(r.Id, r.Name, r.Inn, r.Email))
                    .ToListAsync(token);

            }
        }
    }
}
