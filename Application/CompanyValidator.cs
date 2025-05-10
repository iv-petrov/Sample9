using Sample9.Domain;
using Sample9.Persistence;

namespace Sample9.Application
{
    public static class CompanyValidator
    {
        public static void CompanyExists(ApplicationDbContext context, int id)
        {
            if (!context.Companies.Any(c => c.Id == id))
                throw new ArgumentException(@"Компания Ид = {id} не найдена");
        }
        public static void CompanyValid(ApplicationDbContext context, Company company)
        {
            if (company == null)
                throw new ArgumentException("Компания не задана"); 
            if (company.Name == null || company.Inn == null)
                throw new ArgumentException("Не задано наименование или ИНН компании");
            if (context.Companies.Any(c => c.Inn == company.Inn && c.Id != company.Id))
                throw new ArgumentException("ИНН не может повтряться");
        }
    }
}
