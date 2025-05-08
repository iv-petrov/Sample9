using MediatR;
using Sample9.DataAccess;

namespace Sample9.Slices.Commands
{
    public record CreateCompanyCommand(Company company) : IRequest<int>;
    public class CreateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateCompanyCommand, int>
    {
        public async Task<int> Handle(CreateCompanyCommand command, CancellationToken token)
        {
            if (context.Companies.Any(x => x.Inn == command.company.Inn))
            {
                throw new Exception("Реквизит ИНН дублируется");
            }
            // Для локальной базы InMemory так можно, но для сервера БД нужен автоинкремент
            int id = context.Companies.Max(x => x.Id) + 1;
            var company = new Company(id, command.company.Name, command.company.Inn, command.company.Email);
            await context.Companies.AddAsync(company,token);
            await context.SaveChangesAsync(token);
            return id;
        }
    }
}
