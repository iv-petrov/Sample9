using MediatR;
using Microsoft.EntityFrameworkCore;
using Sample9.DataAccess;
using Sample9.DataModels;

namespace Sample9.Slices.Queries
{
    public record GetCompanyQuery(int Id) : IRequest<Company>;

    public class GetCompanyQueryHandler(ApplicationDbContext context) : IRequestHandler<GetCompanyQuery, Company>
    {
        public async Task<Company?> Handle(GetCompanyQuery query, CancellationToken token)
        {
            return await context.Companies.FirstOrDefaultAsync(x => x.Id == query.Id, token);
        }
    }
}
