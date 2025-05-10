using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample9.Application;
using Sample9.Domain;
using Sample9.Persistence;
using Sample9.Pages.Shared;

namespace Sample9.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Company = await _mediator.Send(new GetCompanyQuery(id));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _mediator.Send(new DeleteCompanyCommand(id));

            return RedirectToPage("./Index");
        }
        public record DeleteCompanyCommand(int Id) : IRequest;
        public class DeleteCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteCompanyCommand>
        {
            public async Task Handle(DeleteCompanyCommand command, CancellationToken token)
            {
                CompanyValidator.CompanyExists(context, command.Id);

                var company = context.Companies.Find(command.Id);
                context.Companies.Remove(company);
                await context.SaveChangesAsync(token);
            }
        }
    }
}
