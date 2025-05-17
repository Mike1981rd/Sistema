using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaContable.Models;
using SistemaContable.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaContable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Set empresaId in session if not already set
            if (!HttpContext.Session.TryGetValue("EmpresaId", out _))
            {
                var empresa = await _context.Empresas.FirstOrDefaultAsync();
                if (empresa != null)
                {
                    HttpContext.Session.SetInt32("EmpresaId", empresa.Id);
                    _logger.LogInformation($"Set EmpresaId in session: {empresa.Id}");
                }
            }
            
            // TODO: Replace with actual data from database
            var viewModel = new DashboardViewModel
            {
                FinancialSummary = new FinancialSummary
                {
                    Sales = 8459.00m,
                    Expenses = 3285.00m,
                    Profits = 5174.00m,
                    PendingPayments = 2560.00m,
                    SalesTrend = 12.5m,
                    ExpensesTrend = 8.3m,
                    ProfitsTrend = 15.2m,
                    PendingPaymentsTrend = -3.7m
                },
                PendingInvoices = new List<PendingInvoice>
                {
                    new PendingInvoice
                    {
                        InvoiceNumber = "0001",
                        ClientName = "Cliente A",
                        Amount = 1250.00m,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = "Vence en 5 días"
                    },
                    new PendingInvoice
                    {
                        InvoiceNumber = "0002",
                        ClientName = "Cliente B",
                        Amount = 850.00m,
                        DueDate = DateTime.Now.AddDays(3),
                        Status = "Vence en 3 días"
                    },
                    new PendingInvoice
                    {
                        InvoiceNumber = "0003",
                        ClientName = "Cliente C",
                        Amount = 2100.00m,
                        DueDate = DateTime.Now.AddDays(7),
                        Status = "Vence en 7 días"
                    }
                }
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
