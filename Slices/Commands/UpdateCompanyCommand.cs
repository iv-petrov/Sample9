using MediatR;
using Sample9.DataAccess;

namespace Sample9.Slices.Commands
{
    public record UpdateCompanyCommand(int Id, string Name, string Inn, string Email) : IRequest;
    public class UpdateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateCompanyCommand>
    {
        public async Task Handle(UpdateCompanyCommand command, CancellationToken token)
        {
            var company = await context.Companies.FindAsync(command.Id, token);
            if (company == null)
            {
                throw new ArgumentException(@"Компания '{command.Name}' не найдена");
            }
            company.Name = command.Name;
            company.Inn = command.Inn;
            company.Email = command.Email;
            context.Companies.Update(company);
            await context.SaveChangesAsync();
        }
    }
}
