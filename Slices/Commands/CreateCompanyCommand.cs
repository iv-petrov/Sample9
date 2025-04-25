using MediatR;
using Sample9.DataAccess;
using Sample9.DataModels;

namespace Sample9.Slices.Commands
{
    public record CreateCompanyCommand(string Name, string Inn, string Email) : IRequest<int>;
    public class CreateCompanyCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateCompanyCommand, int>
    {
        public async Task<int> Handle(CreateCompanyCommand command, CancellationToken token)
        {
            if (context.Companies.Any(x => x.Inn == command.Inn))
            {
                throw new Exception("Реквизит ИНН дублируется");
            }
            // Для локальной базы InMemory так можно, но для сервера БД нужен автоинкремент
            int id = context.Companies.Max(x => x.Id) + 1;
            var company = new Company(id, command.Name, command.Inn, command.Email);
            await context.Companies.AddAsync(company,token);
            await context.SaveChangesAsync(token);
            return id;
        }
    }
}
