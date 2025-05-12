using MediatR;
using Microsoft.EntityFrameworkCore;
using Sample9.Persistence;
using Sample9.Domain;

namespace Sample9.Pages.Shared
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
