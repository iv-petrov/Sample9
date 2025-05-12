using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sample9.Domain;
using Sample9.Persistence;

namespace Sample9.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public IndexModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IList<Company> Companies { get;set; } = default!;

        public async Task OnGetAsync(Query query)
        { 
            Companies = await _mediator.Send(query);
        }
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
