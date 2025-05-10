using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample9.Application;
using Sample9.Domain;
using Sample9.Persistence;

namespace Sample9.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(new CreateCompanyCommand(Company));

            return RedirectToPage("./Index");
        }
        public record CreateCompanyCommand(Company company) : IRequest<int>;
        public class CreateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateCompanyCommand, int>
        {
            public async Task<int> Handle(CreateCompanyCommand command, CancellationToken token)
            {
                // Для локальной базы InMemory так можно, но для сервера БД нужен автоинкремент
                command.company.Id = await context.NextCompanyId();

                CompanyValidator.CompanyValid(context, command.company);

                await context.Companies.AddAsync(command.company, token);
                await context.SaveChangesAsync(token);
                return command.company.Id;
            }
        }
    }
}
