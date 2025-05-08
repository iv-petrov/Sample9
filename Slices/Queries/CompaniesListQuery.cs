using MediatR;
using Microsoft.EntityFrameworkCore;
using Sample9.DataAccess;

namespace Sample9.Slices.Queries
{
    public class CompaniesListQuery
    {
        public record Query : IRequest<List<Company>>;
        public class QueryHandler(ApplicationDbContext context) : IRequestHandler<Query, List<Company>>
        {
            public async Task<List<Company>> Handle(Query query, CancellationToken token)
            {
                return await context.Companies
                    .Select(r => new Company(r.Id, r.Name, r.Inn, r.Email))
                    .ToListAsync(token);

            }
        }
    }
}
