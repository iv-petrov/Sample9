using MediatR;
using Sample9.DataAccess;

namespace Sample9.Slices.Commands
{
    public record DeleteCompanyCommand(int Id) : IRequest;
    public class DeleteCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteCompanyCommand>
    {
        public async Task Handle(DeleteCompanyCommand command, CancellationToken token)
        {
            var company = await context.Companies.FindAsync(command.Id, token);
            if (company != null)
            {
                context.Companies.Remove(company);
                await context.SaveChangesAsync(token);
            }
        }
    }
}
