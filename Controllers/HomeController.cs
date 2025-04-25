using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sample9.DataModels;
using MediatR;
using Sample9.DataAccess;
using Sample9.Slices.Queries;
using Sample9.Slices.Commands;
using System.Threading.Tasks;

namespace Sample9.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public HomeController(IMediator mediator, ApplicationDbContext context)
        { 
            _mediator = mediator;
            _context = context;
        }
        [HttpGet(Name = "index")]
        public async Task<IActionResult> Index(CompaniesListQuery.Query query)
            => View(await _mediator.Send(query));

        [HttpGet("Create", Name = "CreateCompany")]
        public IActionResult CreateCompany()
        {
            Company company = new ();
            return View("Create", company);
        }

        [HttpGet("Update/{id}", Name = "UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            Company company = await _mediator.Send(new GetCompanyQuery(id));
            return View("Update", company);
        }

        [HttpPost()]
        public async Task<IActionResult> SaveCreate(Company company)
        {
            if (ModelState.ErrorCount != 0)
            {
                return View("Create", company);
            }
            await _mediator.Send(new CreateCompanyCommand(company.Name, company.Inn, company.Email));

            return RedirectToAction("");
        }

        [HttpPost()]
        public async Task<IActionResult> SaveUpdate(Company company)
        {
            if (ModelState.ErrorCount != 0)
            {
                return View("Update", company);
            }
            await _mediator.Send(new UpdateCompanyCommand(company.Id, company.Name, company.Inn, company.Email));

            return RedirectToAction("");
        }

        [HttpGet("Delete/{id}", Name = "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCompanyCommand(id));
            return RedirectToAction("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
