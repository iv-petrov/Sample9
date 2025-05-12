using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample9.Application;
using Sample9.Domain;
using Sample9.Persistence;
using Sample9.Pages.Shared;

namespace Sample9.Pages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public EditModel(ApplicationDbContext context, IMediator mediator)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mediator.Send(new UpdateCompanyCommand(Company));

            return RedirectToPage("./Index");
        }
        public record UpdateCompanyCommand(Company company) : IRequest;
        public class UpdateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateCompanyCommand>
        {
            public async Task Handle(UpdateCompanyCommand command, CancellationToken token)
            {
                CompanyValidator.CompanyExists(context, command.company.Id);

                CompanyValidator.CompanyValid(context, command.company);

                context.Companies.Update(command.company);
                await context.SaveChangesAsync(token);
            }
        }
    }
}
