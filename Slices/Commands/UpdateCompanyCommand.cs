using MediatR;
using Sample9.DataAccess;

namespace Sample9.Slices.Commands
{
    public record UpdateCompanyCommand(Company company) : IRequest;
    public class UpdateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateCompanyCommand>
    {
        public async Task Handle(UpdateCompanyCommand command, CancellationToken token)
        {
            var company = await context.Companies.FindAsync(command.company.Id, token);
            if (company == null)
            {
                throw new ArgumentException(@"Компания '{command.Name}' не найдена");
            }
            company.Name = command.company.Name;
            company.Inn = command.company.Inn;
            company.Email = command.company.Email;
            context.Companies.Update(company);
            await context.SaveChangesAsync();
        }
    }
}
